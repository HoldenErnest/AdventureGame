using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Controller : MonoBehaviour
{
    public GameObject escUI;
    public Rigidbody2D rb; // not used in inspector
    public Animator anim; // not used in inspector
    public Character user; // not used in inspector
    public GameObject invUI; // UI for inventory
    public Vector2 movement; // (-1, -1)||(1, 1), to multiply speed
    public static int skillInUse = 0;
    public bool moveOverride = false;
    public Vector2 target;
    public float dashSpeed = 1;
    public bool onWall = false;


    private bool editingText = false;
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        user = GetComponent<Character>();
        anim.SetFloat("animSpeed", user.getSpeed());
    }

    public virtual void Update() {
        if (!canRecieveInput()) {
            stopMoving();
            return;
        }
        attackInputs();
        UIInputs();
    }
    public virtual void FixedUpdate() {
        updateMovement();
    }

    public Vector2 getPosition() {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public void UIInputs() {
        if (Input.GetKeyDown(KeyCode.E)) { // toggle inventory UI
            if (invUI.activeSelf) {
                invUI.SetActive(false);
                getRCMenu().SetActive(false);
            } else {
                invUI.SetActive(true);
                //update inventory
                invUI.GetComponent<InventoryUI>().updateCells();
            }
        }
    }

    void updateMovement() { // player movement only -> set direction then send it to move()
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (moveOverride) { // dash override
            Vector2 dir = -(getPosition() - Vector2.MoveTowards(getPosition(), target, 1));
            dir.Normalize();
            rb.velocity = dir * Time.deltaTime * 140 * dashSpeed;
            if (getLineLength(getPosition(),target) <= 0.1f*dashSpeed) { // stop dash
                moveOverride = false;
            }
            return;
        }

        if (h == 0 && v == 0) { // if standing still
            rb.velocity = new Vector2(0, 0);
            anim.SetBool("buttonDown", false);
            return;
        }
        anim.SetBool("buttonDown", true);

        if (Math.Abs(h) + Math.Abs(v) > 1) { //if youre moving horizontal and vertical at the same time
            movement = new Vector2(h*0.71f,v*0.71f);
        } else movement = new Vector2(h,v);
        move();

        anim.SetFloat("horizontal", h);
        anim.SetFloat("vertical", v);
    }
    public void move() {
        rb.velocity = movement * user.getSpeed() * (140) * Time.fixedDeltaTime;
    }
    public void stopMoving() {
        anim.SetBool("buttonDown", false);
    } 
    public void dash(Vector2 targetPos, float spd) { // dash with - speed for - seconds
        if (spd == 0f) { // a teleport, so check where youre landing so its not in a wall
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetPos, 0.3f);
            foreach (Collider2D cols in hitColliders) {
                if (cols.gameObject.GetComponent<TilemapCollider2D>() != null) {
                    if (cols.gameObject.GetComponent<higherPlane>() != null)
                        break;
                    else
                    return;
                }
            }
            user.transform.position = new Vector2(targetPos.x, targetPos.y + 0.5f);
            if (user.gameObject)
            return;
        } // otherwise just start dash

        dashSpeed = spd;
        target = targetPos;
        moveOverride = true;//start dash
    }

    public void attackInputs() { // all buttons that fire one of the skills in the hotbar
        if (!canAttack()) return;

        if (Input.GetKey("1")) {
            skillInUse = 0;
        }
        if (Input.GetKey("2")) {
            skillInUse = 1;
        }
        if (Input.GetKey("3")) {
            skillInUse = 2;
        }
        if (Input.GetKey("4")) {
            skillInUse = 3;
        }
        if (Input.GetKey("5")) {
            skillInUse = 4;
        }

        if (Input.GetKeyDown(KeyCode.Return)) { // move to the next dialogue if there is one
            DialogueManager.next();
        }

        if (Input.GetMouseButton(0)) {
            if (!user.getSkill(skillInUse).isEmpty()) { // use the selected skill
                user.getSkill(skillInUse).updateTargetPosition(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 8)));
                user.getSkill(skillInUse).useSkill();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            toggleEscMenu();
        }
    }

    private void toggleEscMenu() {
        escUI.SetActive(!escUI.activeSelf);
    }

    public void updateSpeed() { // sets the player movementSpeed
        try {
            anim.SetFloat("animSpeed", user.getSpeed());
        } catch (Exception e) {
            Debug.Log(e);
        }
    }

    public Vector2 getMagnitude() { // returns velocity in terms of 0 and 1
        Vector2 mag;
        float dirX = rb.velocity.x;
        float dirY = rb.velocity.y;
        if (Math.Abs(dirX) > Math.Abs(dirY)) {
            if (dirX != 0)
                dirX /= Math.Abs(dirX);
            if (dirY != 0)
                dirY /= Math.Abs(dirX);
        } else {
            if (dirX != 0)
                dirX /= Math.Abs(dirY);
            if (dirY != 0)
                dirY /= Math.Abs(dirY);
        }

        mag = new Vector2(dirX, dirY);

        return mag;
    }

    public GameObject getRCMenu() {
        return invUI.GetComponent<InventoryUI>().rcItemMenu;
    }

    //Math and things
    private float getLineLength(Vector2 v1, Vector2 v2) { // distance between 2 points
        return (float)Math.Sqrt(Math.Pow(v1.x-v2.x, 2) + Math.Pow(v1.y-v2.y, 2));
    }
    public bool canAttack() { // includes continueDialogue I guess which works
        if (invUI.activeSelf) return false;
        return true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        StartCoroutine(isOnWall());
    }
    void OnCollisionExit2D(Collision2D col)
    {
        onWall = false;
    }
    IEnumerator isOnWall() { // small check to know if the user was on the wall for .3 seconds >(dont want them to get stuck so re-enable movement)
        onWall = true;
        yield return new WaitForSeconds(0.3f);
        if (onWall)
            moveOverride = false;
    }

    private bool canRecieveInput() {
        return !editingText;
    }
    public void setEditingText(bool b) {
        editingText = b;
    }
}
