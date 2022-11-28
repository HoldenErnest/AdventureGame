using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIController : Controller {

    private int team;
    public float noticeRange = 1.0f; // maybe make a wits stat which affects this and things like dodging 
    public Vector2 homePosition; // default where the character goes back to when its done for the night.
    public Vector2 targetPos;
    Skill usingSkill = null;

    private float averageSkillDistance; // used for perfered distance from nearest enemy.
    private float lowestSkillDistance;
    private float highestSkillDistance;

    private bool dashQueued; // dont want to try to strafe but then run away because guy got too close

    int smarts = 5; // TEMP!@!@@!@!@!@!@! how "tough" the opponent is, controls how well they can predict dodges ect.

    private System.Random rand = new System.Random();

    private string[] states = {
        "Idle", //user doesnt move
        "Wandering", //user moves randomly within a radius
        "Raging", //user is running around constantly looking for the nearest thing to attack
        "Home", // user returns to their home point.
        "Dodging",
        "Attack Hard", // user fights close range with big attack moves
        "Attack Soft" // user fights while keeping as much distance possible
    };
    public int currentState;

    void Start() {
        currentState = 4;
        speed = 1.1f;
        team = GetComponent<Team>().team;
        setSkillToUse(); //TEMPORARYYYYYYYYYYYYYYYYYYYY!!!!!!!!!!!!!!!!!!!!!
    }
    public override void FixedUpdate() {
        updatePrefDistances();
        performStateActions();
        
    }
    public override void Update() {
        //do nothing
    }
    //everything a character does while in a certain state


    public void setState(string s) {
        for (int i = 0; i < states.Length; i++) {
            if (s.Equals(states[i]))
                currentState = i;
        }
    }
    public string getCurrentState() {
        return states[currentState];
    }
    private void performStateActions() {
        if (targetPos.x == 0 && targetPos.y == 0) targetPos = homePosition;
        switch (currentState) {
            case 0:
                idle();
                break;
            case 1:
                wandering();
                break;
            case 2:
                raging(); // maybe if raging only initiate dodge if smarts is high enough. (brainless tank will just stand there and mow down their opponent) 
                break;
            case 3:
                goHome();
                break;
            case 4:
                dodging();
                break;
            case 5:
                AttackHard();
                break;
            case 6:
                AttackSoft();
                break;
        }
    }
    //STATE PERFORMANCES::_____________________________________________________________________________________
    
    private void idle() {
        //do nothing because its idle lmao.
    }
    private void goHome() { // I work alone
        if (reachedTarget()) {
            setState("Idle");
            return;
        }
        targetPos = homePosition;
        moveTowardsTarget(true);
    }
    private void wandering() {
        
        float wanderRadius = 5f;
        if (!reachedTarget()) {
            moveTowardsTarget(true);
        } else {
            float dx = Math.Abs(Math.Abs(homePosition.x) - Math.Abs(getPos().x));//absolute distance x and y from the home position
            float dy = Math.Abs(Math.Abs(homePosition.y) - Math.Abs(getPos().y));
            dx /= wanderRadius;//rad = 5.. 5/5 = 100% chance(go right)
            dy /= wanderRadius;
            if (dx > 1) dx = 1;
            if (dy > 1) dy = 1;
            if (homePosition.x < getPos().x) dx *= -1;// if target less than position, change direction to towards it.
            if (homePosition.y < getPos().y) dy *= -1;
            randomizeTarget(wanderRadius,dx,dy);
            //idle there for a few seconds
            stopMoving();
            StartCoroutine(idleForSeconds(3f, getCurrentState()));
        }
    }
    private void raging() {
        GameObject temp = getClosestEnemy();
        if (!GameObject.ReferenceEquals(temp, null)) {
            targetPos = temp.transform.position;
            moveToAttackRange();
        }
    }
    private void dodging() {
        // get closest bullet in the area to test if you should move right or left of it.
        GameObject closestEnemy = getClosestEnemy();
        GameObject closestProj = getClosestEnemyProjectile();
        if (GameObject.ReferenceEquals(closestProj, null) && GameObject.ReferenceEquals(closestEnemy, null)) return; //nothing to dodge from

        if (!GameObject.ReferenceEquals(closestProj, null)) {
            Vector2 bulletPos = new Vector2(closestProj.transform.position.x, closestProj.transform.position.y);

            if (!GameObject.ReferenceEquals(closestEnemy, null)) {
                Vector2 enemyPos = new Vector2(closestEnemy.transform.position.x, closestEnemy.transform.position.y);

                if(Vector2.Distance(getPos(), enemyPos) < 2f)
                    moveTowardsVector(false, enemyPos);
                else
                    strafe(enemyPos, pointIsRightOfLine(getPos(), enemyPos, bulletPos), bulletPos);
            } else { // no enemy in range just run from the bullet
                moveTowardsVector(false, bulletPos);
            }
        } else if (!GameObject.ReferenceEquals(closestEnemy, null)) { // no projectile but an enemy
            moveToAttackRange();
        }

        //if no immediate threat (bullet moving away from player), move randomly?
    }
    private void AttackHard() {
        GameObject temp = getClosestEnemy();
        if (!GameObject.ReferenceEquals(temp, null)) {
            targetPos = temp.transform.position;
            moveToAttackRange();
        }
    }
    private void AttackSoft() {
        GameObject temp = getClosestEnemy();
        if (!GameObject.ReferenceEquals(temp, null)) {
            targetPos = temp.transform.position;
            moveToAttackRange();
        }
    }

    //END STATE PERFORMANCES::_________________________________________________________________________________

    public void move(float h, float v) {

        if (moveOverride) {
            Vector2 dir = -(getPos() - Vector2.MoveTowards(getPos(), target, 1));
            
            dir.Normalize();
            Debug.Log(dir + "dir");
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

        if (Math.Abs(h) + Math.Abs(v) > 1f) { //if youre moving horizontal and vertical at the same time
            movement = new Vector2(h*0.71f,v*0.71f);
        } else movement = new Vector2(h,v);
        rb.velocity = movement * speed*(140) * Time.fixedDeltaTime;

        anim.SetFloat("horizontal", h);
        anim.SetFloat("vertical", v);
    }

    public void moveToAttackRange() {

        if (Vector2.Distance(targetPos, getPos()) > getPrefFarDistance()) { // if distance is greater than the max range
            moveTowardsTarget(true);
        } else if (Vector2.Distance(targetPos, getPos()) < getPrefCloseDistance()){ // if distance is less than the minRange
            moveTowardsTarget(false);
        } else {
            stopMoving();
        }
    }
    private bool reachedTarget() { // returns true when the user is at(close enough) to the target position.
        return withinRadiusTarget(0.1f);
    }
    private bool reachedVector(Vector2 v) { // returns true when the user is at(close enough) to the target position.
        return withinRadiusOfVector(v, 0.1f);
    }
    private bool withinRadiusTarget(float maxDistance) { // returns if the user is within a specified radius of the target position.
        return withinRadiusOfVector(targetPos, maxDistance);
    }
    private bool withinRadiusOfVector(Vector2 tar, float maxDistance) { // returns if user is within tar

        float x = getPos().x;
        float y = getPos().y;
        float dx = Math.Abs(Math.Abs(x) - Math.Abs(tar.x));
        float dy = Math.Abs(Math.Abs(y) - Math.Abs(tar.y));
        float dxy = (dx + dy)/2;

        if (dxy < maxDistance) return true;
        return false;
    }
    public void randomizeTarget(float d) {
        randomizeTarget(d,0f,0f);
    }
    public void randomizeTarget(float d, float weightX, float weightY) { // randomize the target max of 'd' distance from the current target (weights from -1.0 to 1.0)
        float dx = (float)rand.Next(0,(int)(d*100))/100; // *100 / 100 to get more precise to 2 decimals
        float dy = (float)rand.Next(0,(int)(d*100))/100;
        if (weightX != 0 && weightY != 0) {
            bool wx = rand.Next(0,10) <= (int)Math.Abs(weightX)*10; // whether the weights are a success
            bool wy = rand.Next(0,10) <= (int)Math.Abs(weightY)*10;
            if (wx){
                if (weightX < 0 )
                    dx *= -1;
            }
            if (wy) {
                if (weightY < 0)
                    dy *= -1;
            }
        }
        targetPos = new Vector2(targetPos.x + dx, targetPos.y + dy);
        //Debug.Log("target" + targetPos);
    }
    public void setSkillToUse() { // select a skill to use - depending on the damage range ect of the skill
        usingSkill = user.getSkill(0);
    }

    public void moveTowardsTarget(bool towards) {
        moveTowardsVector(towards, targetPos);
    }
    private void moveTowardsVector(bool towards, Vector2 tar) {
        float tempX = tar.x - getPos().x;
        float tempY = tar.y - getPos().y;
        float h = 0, v = 0;
        if (tempX != 0)
            h = tempX/Math.Abs(tempX);

        if (tempY != 0)
            v = tempY/Math.Abs(tempY);

        //get variability(move on diagonals when needed)
        if (Math.Abs(tempX) >= Math.Abs(tempY)*2) // if xDist is at least double yDist, move along the X axis
            v = 0;
        if (Math.Abs(tempY) >= Math.Abs(tempX)*2) // if yDist is at least double xDist, stop moving along the X axis
            h = 0;
        if (!towards) {
            h *= -1;
            v *= -1;
            if (Vector2.Distance(getPos(), tar) < getPrefFarDistance() || user.getHealthPercentage() <= 0.3) { // if youre too close or your health is low
                Debug.Log("RUN AWAWY!!!!! x:"+h+" y:"+v);
                rollTowardsVector(new Vector2(h,v)+ getPos()); // try to roll towards the target first
            }
        } else {
            if (Vector2.Distance(getPos(), tar) >= getPrefFarDistance()+5) {// if youre far enough away roll to go faster
                Debug.Log("THIS IS STUPID        x:"+h+" y:"+v);
                rollTowardsVector(new Vector2(h,v)+ getPos()); // try to roll towards the target first
            }
        }
        
        move (h, v);
    }

    private void strafe(Vector2 enemyPos, bool goRight, Vector2 bulletPos) { // strafe either left or right of the direction to closest enemy
        int d = 1;
        if (!goRight) d *= -1;
        
        Vector2 tar = perpendicularToDirection(enemyPos, getPos(), d);
        if (Vector2.Distance(getPos(), bulletPos) < 2f) {
            rollTowardsVector(tar); // try rolling to the strafe position first
        }
        moveTowardsVector(true, tar);
    }

    private void rollTowardsVector(Vector2 v) { // only if skill minimum distance allows ??
        if (equipDash()) {
            attackPosition(v);
            equipAttack(); // maybe put this in the attacking state instead>>>>>>>>>
        }
    }

    public void stopMoving() {
        rb.velocity = new Vector2(0, 0);
        anim.SetBool("buttonDown", false);
    }

    public void attackPosition(Vector2 v) {
        if (equipAttack()) {
            Debug.Log("casting skill " + usingSkill.skillName + " at position: " + v);
            usingSkill.updateTargetPosition(v);
            usingSkill.useSkill();
        }
    }

    public void attack() {
        attackPosition(targetPos);
    }

    public void attackInputs() {
        //override method
    }
    public bool canSee(Vector2 trgt) {
        RaycastHit2D hit = Physics2D.Raycast(getPos(), getDirection(trgt), noticeRange, LayerMask.GetMask("Collision"));
        if (hit.collider != null) {
            return false;
        }
        return true;
    }
    private Vector2 getDirection(Vector2 v) {
        return -(getPos() - v);
    }

    public GameObject getClosestEnemy() { // collects any hit colliders within the attackArea and sends out the damage( * user strength ect.)
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(getPos(), noticeRange);
        GameObject closest = null;
        foreach (Collider2D cols in hitColliders) {
            GameObject target = cols.gameObject;
            if (target.GetComponent<Character>() != null) {
                if (isEnemy(target)) { //find any opponents within range
                    if (!GameObject.ReferenceEquals(closest, null)) { // new closest
                        if (Vector2.Distance(closest.transform.position, getPos()) > Vector2.Distance(target.transform.position, getPos())) { // if closest isnt nothing compare it to the previous closest, otherwise this is new closest
                            if (canSee(target.transform.position))
                                closest = target;
                            //else Debug.Log("cant see");
                        }
                    } else {
                        if (canSee(target.transform.position))
                            closest = target;
                        else closest = null;
                    }
                }
            }
        }
        return closest;
    }

    public GameObject getClosestEnemyProjectile() { // collects any hit colliders within the attackArea
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(getPos(), noticeRange);
        GameObject closest = null;
        foreach (Collider2D cols in hitColliders) {
            GameObject target = cols.gameObject;
            if (target.GetComponent<Projectile>() != null) {
                if (isEnemy(target.GetComponent<Projectile>().user)) {
                    if (!GameObject.ReferenceEquals(closest, null)) { // new closest
                        if (Vector2.Distance(closest.transform.position, getPos()) > Vector2.Distance(target.transform.position, getPos())) { // if closest isnt nothing compare it to the previous closest, otherwise this is new closest
                            if (canSee(target.transform.position))
                                closest = target;
                            //else Debug.Log("cant see");
                        }
                    } else {
                        if (canSee(target.transform.position))
                            closest = target;
                        else closest = null;
                    }
                }
            }
        }
        return closest;
    }

    public bool isEnemy(GameObject g) {
        return g.GetComponent<Team>().getTeam() != team;
    }

    public bool skillIsProjectile() {
        return usingSkill.isProjectile();
    }

    private Vector2 getPos() {
        return this.gameObject.transform.position;
    }
    private float getPrefCloseDistance() { // prefered distances to the nearest opponent.
        return lowestSkillDistance;
    }
    private float getPrefFarDistance() {
        return highestSkillDistance;
    }
    private void updatePrefDistances() {
        lowestSkillDistance = user.getLowSkillDist();
        highestSkillDistance = user.getHighSkillDist();
        averageSkillDistance = user.getAvgSkillDist();
    }
    private bool equipDash() {
        foreach (Skill skl in user.usingSkills) {
            if (skl.damageType.Equals("movement"))
                if (!skl.isReloading()) {
                usingSkill = skl;
                return true;
            }
        }
        return false;
    }
    private bool equipAttack() {
        foreach (Skill skl in user.usingSkills) {
            if (!skl.damageType.Equals("movement") && !skl.damageType.Equals("heal"))
                if (!skl.isReloading()) {
                usingSkill = skl;
                return true;
            }
        }
        return false;
    }

    //Math--------
    private Vector2 getRangePoint(Vector2 pos1, Vector2 pos2, float range) {
        double angle = getAngle(pos1, pos2);
        Vector2 v = new Vector2();
        v.x = (float)(pos1.x - range * Math.Cos(angle));
        v.y = (float)(pos1.y - range * Math.Sin(angle));

        return v;
    }
    private Vector2 perpendicularToDirection(Vector2 pos1, Vector2 pos2, float distance) { // goes specified distance from pos2 perpendicular to line pos1 pos2. NEGATE DISTANCE to go left instead of right.
        Vector2 v = pos2 - pos1; // scale down to pos1 as orgin
        Vector2 c = new Vector2(-v.y, v.x) / (float)(Math.Sqrt(Math.Pow(v.x,2) + Math.Pow(v.y,2)) * distance);

        return c + pos2; // scale back up from orgin to pos2
    }
    private double getAngle(Vector2 pos1, Vector2 pos2) {
        return  Math.Atan2(pos1.y - pos2.y, pos1.x - pos2.x);
    }
    private bool pointIsRightOfLine(Vector2 pos1, Vector2 pos2, Vector2 point) {
        pos2.x -= pos1.x;
        pos2.y -= pos1.y;
        point.x -= pos1.x;
        point.y -= pos1.y;
 
        // Determining cross Product
        float cross_product = pos2.x * point.y - pos2.y * point.x;
 
        // return RIGHT if cross product is positive
        return cross_product > 0;
    }
    private float getLineLength(Vector2 v1, Vector2 v2) { // distance between 2 points
        return (float)Math.Sqrt(Math.Pow(v1.x-v2.x, 2) + Math.Pow(v1.y-v2.y, 2));
    }

    //WAITINGS
    public IEnumerator idleForSeconds(float s, string prevState) {
        setState("Idle");
        yield return new WaitForSeconds(s);
        setState(prevState);
    }

}
