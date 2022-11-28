using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{

    public GameObject contactPrefab;

    //Variables passed from skill info
    public int damage;
    public string damageType;
    public float speed = 1f;
    public int pierce = 1; // how many enemy characters it can pierce before it dissapates 
    public float maxRange = 3f;
    public float areaDamage; // the bullet will always be the same size. areaDamage dictates how big the area will be on each contact(aka if a bullet explosion is needed)
    public GameObject user;
    public Vector2 target;
    public string[] setEffects;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void addAllEffects(Character c) { // effects the user with any effects the skill has
        foreach (string s in setEffects) {
            string eName = s.Substring(0,s.Length-1);
            int eTier = Int32.Parse(s.Substring(s.Length-1));
            c.addEffect(eName, eTier, user);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveForward(); // maybe change to frictionless rb.addforce so Update() isnt needed
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (isCharacter(col.gameObject)) {
            if (col.gameObject.GetComponent<Team>().getTeam() != user.GetComponent<Team>().getTeam()) {
                Character target = col.gameObject.GetComponent<Character>();
                target.damageByType(user.GetComponent<Character>().attackByType(damage, damageType), damageType);
                addAllEffects(target);
                pierce--;
            } else {
                //Debug.Log("hit friendly");
            }
        } else {
            if (col.gameObject.GetComponent<TilemapCollider2D>() != null)
                if (!col.gameObject.GetComponent<TilemapCollider2D>().isTrigger && !userOnHigherPlane())
                    stop(); // runs into a collidable object
        }
        if (pierce <= 0) {
            stop();
        }
    }

    public void radiusCheck(Vector2 pos, float radius) { // collects any hit colliders within the attackArea and sends out the damage( * user strength ect.)
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, radius);
        foreach (Collider2D cols in hitColliders) {
            if (isCharacter(cols.gameObject)) {
                if (cols.gameObject.GetComponent<Team>().getTeam() != user.GetComponent<Team>().getTeam()) {
                    //Debug.Log(user.name + " hit " + cols.gameObject.name);
                    Character target = cols.gameObject.GetComponent<Character>();

                    target.damageByType(user.GetComponent<Character>().attackByType(damage, damageType), damageType);
                    addAllEffects(target);
                }
            }
        }
    }

    public Vector2 getMagnitude() { // returns the magnitude the projectile is heading in. (1, 1) = 45degrees - up,right
        Vector2 userPos = user.transform.position;

        float dirX = userPos.x - target.x;
        float dirY = userPos.y - target.y;

        if (Math.Abs(dirX) > Math.Abs(dirY)) { // (-10, 4)
            dirX /= Math.Abs(dirX); // -1
            dirY /= Math.Abs(dirX); // 0.4
        } else {
            dirX /= Math.Abs(dirY);
            dirY /= Math.Abs(dirY);
        }
        return new Vector2(dirX, dirY);
    }

    public bool userOnHigherPlane() {
        return user.GetComponent<Character>().getHigherPlane() > 0;
    }

    public bool isCharacter(GameObject g) {
        return g.GetComponent<Character>() != null;
    }
    public void moveForward() {
        if (target != null) {
            if (transform.position.Equals(target)) {
                stop();
            }
            Vector2 newPosition = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed * 20);
            rb.MovePosition(newPosition);
        }
    }
    public void explode() {
        if (areaDamage > 0f) { // if attackArea of skill is 0.0 then it isnt an exploding skill.
            radiusCheck(this.gameObject.transform.position, areaDamage);
        }
    }
    public void stop() {
        explode();
        Destroy(this.gameObject);
    }
}
