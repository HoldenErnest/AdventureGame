using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIController : Controller {

    private int team;
    public float noticeRange = 1.0f; // maybe make a wits stat which affects this and things like dodging 
    public Vector2 homePosition; // default where the character goes back to when its done for the night.
    public Vector2 targetPos;
    public Vector2 targetLastSeenPos;
    Skill usingSkill = null;

    private float averageSkillDistance; // used for perfered distance from nearest enemy.
    private float lowestSkillDistance;
    private float highestSkillDistance;

    private bool dashQueued; // dont want to try to strafe but then run away because guy got too close
    int smarts = 5; // TEMP!@!@@!@!@!@!@! how "tough" the opponent is, controls how well they can predict dodges ect.

    //stupid bool to make sure the corroutine only activates once.
    private bool waitingForOpponent = false;

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
        team = GetComponent<Team>().team;
        targetLastSeenPos = homePosition;
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
        stopMoving();
        //do nothing because its idle lmao.
    }
    private void goHome() { // I work alone
        if (reachedVector(homePosition)) {
            setState("Idle");
            return;
        }
        moveTowardsVector(true, homePosition);
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
        drawPoint(targetLastSeenPos, 0.3f);
        // get closest bullet in the area to test if you should move right or left of it.
        GameObject closestEnemy = getClosestEnemy();
        GameObject closestProj = getClosestEnemyProjectile();
        if (GameObject.ReferenceEquals(closestProj, null) && GameObject.ReferenceEquals(closestEnemy, null)) { // nothing to dodge from go to last time something was seen
            
            if (!reachedVector(targetLastSeenPos)) {
                Debug.Log("has not reached last seen");
                moveTowardsVector(true, targetLastSeenPos);
            } else if (!waitingForOpponent) {
                StartCoroutine(goHomeAfterWait());
            }
            
            return;
        }
        waitingForOpponent = false; // back to fighting so dont try to go home anymore.

        if (moveFromWall()) return; // move away from any walls because it might interfear with fighting. But only after you tried to get to last seen

        Vector2 enemyPos;

        if (!GameObject.ReferenceEquals(closestProj, null)) {
            Vector2 bulletPos = new Vector2(closestProj.transform.position.x, closestProj.transform.position.y);

            if (!GameObject.ReferenceEquals(closestEnemy, null)) {
                enemyPos = new Vector2(closestEnemy.transform.position.x, closestEnemy.transform.position.y);

                if(getLineLength(getPos(), enemyPos) < 2f)
                    moveTowardsVector(false, enemyPos);
                else
                    strafe(enemyPos, pointIsRightOfLine(getPos(), enemyPos, bulletPos), bulletPos);
            } else { // no enemy in range just run from the bullet
                moveTowardsVector(false, bulletPos);
            }
        } else if (!GameObject.ReferenceEquals(closestEnemy, null)) { // no projectile but an enemy
            enemyPos = new Vector2(closestEnemy.transform.position.x, closestEnemy.transform.position.y);
            if (!inDanger(closestEnemy.GetComponent<Character>()))
                moveToAttackRange();
            else {// the enemy is better, so run
                moveTowardsVector(false, enemyPos);
            }
        }

        if (!inDanger()) {
            setState("Attack Hard");
        } else {
            setState("Attack Soft");
        }
        //if no immediate threat: no bullet, no player, wait for some time then set state to wander!
    }
    private void AttackHard() {
        GameObject temp = getClosestEnemy(); // the gameobject the user will be trying to hit
        if (!GameObject.ReferenceEquals(temp, null)) {
            targetPos = temp.transform.position;
            //moveToAttackRange();
        
            if (equipAttack()) {
                attackUserWithPredict(temp.GetComponent<Character>());
            }
        }
    
        setState("Dodging");
        
    }
    private void AttackSoft() {
        GameObject temp = getClosestEnemy();
        if (!GameObject.ReferenceEquals(temp, null)) {
            targetPos = temp.transform.position;
            //moveToAttackRange();
            if (equipAttack()) {
                attackUserWithPredict(temp.GetComponent<Character>());
            }
        }
        setState("Dodging");
    }

    //END STATE PERFORMANCES::_________________________________________________________________________________

    public void move(float h, float v) {
        if (moveOverride) {
            Vector2 dir = -(getPos() - Vector2.MoveTowards(getPos(), target, 1));
            
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

        if (Math.Abs(h) + Math.Abs(v) > 1.3f) { //if youre moving horizontal and vertical at the same time
            movement = new Vector2(h*0.71f,v*0.71f);
        } else movement = new Vector2(h,v);
        rb.velocity = movement * user.getSpeed()*(140) * Time.deltaTime;

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
        return withinRadiusTarget(0.2f);
    }
    private bool reachedVector(Vector2 v) { // returns true when the user is at(close enough) to the target position.
        return withinRadiusOfVector(v, 0.2f);
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
            if (getLineLength(getPos(), tar) < getPrefCloseDistance() || inDanger()) { // if youre too close or your health is low
                rollTowardsVector(new Vector2(h,v)+ getPos()); // try to roll away first
            }
        } else {
            if (getLineLength(getPos(), tar) >= getPrefCloseDistance()+4) {// if youre far enough away roll to go faster
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
            Debug.Log("equipped a roll!");
            attackPosition(v);
            //equipAttack(); // maybe put this in the attacking state instead>>>>>>>>>
        }
    }

    public void stopMoving() {
        rb.velocity = new Vector2(0, 0);
        anim.SetBool("buttonDown", false);
    }

    public void attackPosition(Vector2 v) { // generic attack, if in range with any available attacks, use that skill
        if (!canSee(targetPos)) return;
        Debug.Log("casting skill " + usingSkill.skillName + " at position: " + v);
        usingSkill.updateTargetPosition(v);
        usingSkill.useSkill();
    }

    public void attack() {
        attackPosition(targetPos);
    }
    public void attackUserWithPredict(Character theUser) { // Attack a user based on their velocity and your projectile speed
        float projSpeed = usingSkill.projectileSpeed;
        Vector2 theUserPos = theUser.GetController().getPosition();
        float theSpeed = theUser.getSpeed();
        Vector2 prediciton = theUserPos;

        if (projSpeed != 0 || theSpeed == 0f) { // there is actually something to predict.
            
            float distanceBetween = getLineLength(getPosition(), theUserPos);
            
            Vector2 theUserMag = theUser.GetController().getMagnitude();
            theSpeed = theUser.getSpeed();

            float timeToImpact = distanceBetween / projSpeed; // find the time till inpact
            prediciton = theUserPos + (theUserMag * (theSpeed * timeToImpact)); // how far will the target travel in that time? *(mag) and in which direction.

        }

        attackPosition(prediciton);
        
    }

    public void attackInputs() {
        //override method
    }
    public bool canSee(Vector2 trgt) { // draws a ray to see if there is not objects between the user and the target destination
        
        //Debug.DrawRay(getPos(), getDirection(trgt), Color.blue, 0.2f);
        drawPoint(getPos(), 0.2f);
        float rayLength = getLineLength(getPos(), trgt);
        if (rayLength > 1) {
            rayLength -= 1;
            RaycastHit2D hit = Physics2D.Raycast(getPos(), getDirection(trgt), rayLength, LayerMask.GetMask("Collision"));
            
            if (hit.collider != null) {
                return false;
            }
        }
        targetLastSeenPos = trgt;
        return true;
    }
    public bool moveFromWall() { //move the character if there are any walls nearby, returns if the user was actually moved.
        Vector2 v = getNearWalls();
        if (v != Vector2.zero) {
            Debug.Log("moved from the wall(s) at " + v);
            moveTowardsVector(false, (getPos() + v));
            return true;
        }
        return false;
    }
    public Vector2 getNearWalls() { // returns ({-1,0,1},{-1,0,1}) for where walls are reletive to the user. returns 0 if no walls in that direction or both walls are equadistant from user.
        float rayLength = 0.8f;
        Vector2 walls = new Vector2(0,0);

        RaycastHit2D up = Physics2D.Raycast(getPos(), Vector2.up, rayLength, LayerMask.GetMask("Collision"));
        RaycastHit2D down = Physics2D.Raycast(getPos(), Vector2.down, rayLength, LayerMask.GetMask("Collision"));
        RaycastHit2D right = Physics2D.Raycast(getPos(), Vector2.right, rayLength, LayerMask.GetMask("Collision"));
        RaycastHit2D left = Physics2D.Raycast(getPos(), Vector2.left, rayLength, LayerMask.GetMask("Collision"));

        if (up.collider != null) walls.y++;
        if (down.collider != null) walls.y--;
        if (right.collider != null) walls.x++;
        if (left.collider != null) walls.x--;

        return walls;
    }

    private Vector2 getDirection(Vector2 v) {
        return (v - getPos());
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
        if (hitColliders != null)
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
        Vector2 v = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 0.5f);
        return v;
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
    private bool inDanger(Character closestEnemy) { //low health ect, used to attack from distance or afar.
        return user.getHealthPercentage() <= 0.33f && !morehpThan(closestEnemy);
    }
    private bool inDanger() { //the more parameters passed the more accurate it can be to sensing danger-------
        return user.getHealthPercentage() <= 0.33f;
    }
    private bool morehpThan(Character c) { // your health is more than the specified characters
        return user.getHealthPercentage() >= c.getHealthPercentage();
    }
    private bool equipDash() {
        foreach (Skill skl in user.usingSkills) {
            if (skl.damageType.Equals("movement")) {
                if (!skl.isReloading()) {
                    usingSkill = skl;
                    return true;
                }
            }
        }
        return false;
    }
    private bool equipAttack() { // equip any attack >>> Eventually change to selective attacks like heavy or light attacks
        string[] ss = {"physical", "magical", "bleed", "poison", "necro"};
        foreach (Skill skl in user.usingSkills) {
            
            if (skl.dTypeEquals(ss)) {
                if (!skl.isReloading()) {
                    usingSkill = skl;
                    return true;
                }
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
    Vector2 normalizeVector(Vector2 v) {
        float dirX = v.x;
        float dirY = v.y;
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
        return new Vector2(dirX, dirY);
    }

    //WAITINGS
    public IEnumerator idleForSeconds(float s, string prevState) {
        setState("Idle");
        yield return new WaitForSeconds(s);
        setState(prevState);
    }
    public IEnumerator goHomeAfterWait() {
        waitingForOpponent = true;
        yield return new WaitForSeconds(3f);
        if (waitingForOpponent) {
            setState("Home");
            Debug.Log("going back home");
        }
    }

    //EVENTS
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
        if (onWall) {
            moveOverride = false;
        }
    }

    //TESTING
    public void drawPoint(Vector2 v, float radius) {
        Debug.DrawLine(new Vector2(v.x-radius, v.y), new Vector2(v.x+radius, v.y), Color.blue, 1f);
        Debug.DrawLine(new Vector2(v.x, v.y-radius), new Vector2(v.x, v.y+radius), Color.blue, 1f);
    }

}