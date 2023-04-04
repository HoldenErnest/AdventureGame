using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
    All skills are established from the Knowledge class with a file name

*/

[Serializable]
public class Skill : MonoBehaviour {
    //each skill has an animation that it plays
    private string path;
    public string skillName = "Unassigned Skill";
    public string damageType = "null"; // "movement", "summon", "physical, "magical"
    public int baseDamage;
    public int manaCost;
    public float reloadTime; // time to regenerate a stack
    public float maxRange;
    public float minRange;
    public float attackArea; // radius of the circle hitbox used to detect hits
    public int levelReq;
    public string[] setEffects; // effects tied to each hit
    public string prefabName = "Unknown Prefab";

    public GameObject user;
    public GameObject animPrefab; // animation / turret / effectcloud / ect.
    

    public float projectileSpeed = 0f; // also used for movement speed
    public int pierce = 1;

    private bool reloading = false;
    private Vector2 userPosition, targetPosition;

    //CONSTSRUCTORS
    public Skill() {
        skillName = "Unknown Skill";
    }
    public Skill(GameObject caster) {
        skillName = "Unknown Skill";
        baseDamage = 12;
        manaCost = 5;
        reloadTime = 0.5f;
        maxRange = 3f;
        minRange = 1f;
        attackArea = 0.5f;
        levelReq = 0;
        user = caster;
    }
    public Skill(string name, int damage, int mana, int lvl, float reload, float max, float min, float radius) {
        skillName = name;
        baseDamage = damage;
        manaCost = mana;
        reloadTime = reload;
        maxRange = max;
        minRange = min;
        attackArea = radius;
        levelReq = lvl;
    }

    public void useSkill() {
        if (!reloading && userLevelReq()) {
            updateUserPosition();
            drawDebugLines();
            getUserCharacter().invokeSkillCooldown(this); // skill cooldown sent to Player to handle(cant start without a Gameobject)

            if (damageType.Equals("movement")) {
                dash();
                return;
            } else if (projectileSpeed > 0f) { // if the attack is not a projectile
                createProjectile(getRangePoint(minRange));
            } else {
                //find where the hit is actually landing(min and max range) and send it
                getHits(posInRange(), attackArea);
            }
        }
    }
    public void getHits(Vector2 pos, float radius) { // collects any hit colliders within the attackArea and sends out the damage( * user strength ect.)
        createPrefab(pos);
        if (radius == 0f) return;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, radius);
        foreach (Collider2D cols in hitColliders) {
            if (isHittable(cols.gameObject)) {
                if (cols.gameObject.GetComponent<Team>().getTeam() != getUserTeam()) {
                    Debug.Log(user.name + " hit " + cols.gameObject.name);
                    Character target = cols.gameObject.GetComponent<Character>();
                    
                    target.setLastHit(user.GetComponent<Character>());
                    target.damageByType(user.GetComponent<Character>().attackByType(baseDamage, damageType), damageType);
                    addAllEffects(target);
                } else {
                    Debug.Log(user.name + " hit a friendly " + cols.gameObject.name);
                }
            }
        }
    }
    public void addAllEffects(Character c) { // effects the user with any effects the skill has
        foreach (string s in setEffects) {
            string eName = s.Substring(0,s.Length-1);
            int eTier = Int32.Parse(s.Substring(s.Length-1));
            c.addEffect(eName, eTier, user);
        }
    }
    public Skill duplicateSkill() {
        return new Skill(skillName, baseDamage, manaCost, levelReq, reloadTime, maxRange, minRange, attackArea);
    }
    public Vector2 posInRange() { // get target position using max and min distance
        float targetDist = getLineLength(userPosition, targetPosition);
        if (targetDist > maxRange) {
            return getRangePoint(maxRange);
        } else if (targetDist < minRange) {
            return getRangePoint(minRange);
        }
        return targetPosition;
    }

    public void dash() {
        Debug.Log(user.name + " tried to dash");
        user.GetComponent<Controller>().dash(posInRange(), projectileSpeed);
    }

    public void createPrefab(Vector2 target) {
        GameObject visual;
        if (damageType == "summon") {
            CharacterCreator cc = Knowledge.getCharBlueprint(prefabName);
            cc.setSpawn(target);
            visual = cc.createCharacter();
        } else {
            visual = Instantiate(Knowledge.getSkillPrefab(prefabName), target, Quaternion.identity);
        }
        if (visual.GetComponent<Character>() == null) {
            Destroy(visual, 3f);
        } else {
            Debug.Log(user.name + " summoned a " + visual.GetComponent<Character>().name);
        }
    }
    public void createProjectile(Vector2 target) {
        GameObject visual = Instantiate(Knowledge.getSkillPrefab(prefabName), target, Quaternion.identity);
        if (visual.GetComponent<Projectile>() != null) {
            Projectile p = visual.GetComponent<Projectile>();
            p.areaDamage = attackArea;
            p.damage = baseDamage;
            p.damageType = damageType;
            p.speed = projectileSpeed;
            p.pierce = pierce;
            p.user = user;
            p.target = getRangePoint(maxRange);
            p.setEffects = setEffects;
        }
    }

    //Basic Getters and Setters------------
    public int getUserTeam() {
        return user.GetComponent<Team>().getTeam();
    }
    public void setUser(GameObject gm) {
        user = gm;
    }
    public void setDamage(int d) {
        baseDamage = d;
    }
    public Character getUserCharacter() {
        return user.GetComponent<Character>();
    }
    public void setReloading(bool r) {
        reloading = r;
    }
    public void updateTargetPosition(Vector2 targetPos) { // update within the controller - represents original target stop NOT calulating max and min ect.
        targetPosition = targetPos;
    }
    public void updateUserPosition() {
        userPosition = user.GetComponent<Transform>().position;
    }
    public float getReloadTime() {
        return reloadTime;
    }
    public int getBaseDamage() {
        return baseDamage;
    }
    public int getManaCost() {
        return manaCost;
    }
    public string getSkillName() {
        return skillName;
    }
    public bool userLevelReq() {
        return getUserCharacter().getLevel() >= levelReq;
    }
    public bool isReloading() {
        return reloading;
    }
    public bool isHittable(GameObject g) {
        return g.GetComponent<Character>() != null;
    }
    public bool isProjectile() { // its a projectile if ps isnt nothing, the projectile it explodes if the attackArea is also not nothing
        return projectileSpeed > 0f;
    }
    public bool dTypeEquals(string[] ss) { // if any string in the array = damageType
        foreach (string s in ss) {
            if (damageType.Equals(s)) return true;
        }
        return false;
    }
    // returns if the skill should be null
    public bool isEmpty() {
        return skillName.Equals("Unknown Skill");
    }


    //Math and things-----------------
    private Vector2 getDirection() {
        return -(userPosition - targetPosition);
    }
    private double getAngle() {
        return  Math.Atan2(userPosition.y - targetPosition.y, userPosition.x - targetPosition.x);
    }
    private Vector2 getRangePoint(float range) { // returns a point at the specified line length but in the same direction
        double angle = getAngle();
        Vector2 v = new Vector2();
        v.x = (float)(userPosition.x - range * Math.Cos(angle));
        v.y = (float)(userPosition.y - range * Math.Sin(angle));

        return v;
    }
    private float getLineLength(Vector2 v1, Vector2 v2) {
        return (float)Math.Sqrt(Math.Pow(v1.x-v2.x, 2) + Math.Pow(v1.y-v2.y, 2));
    }

    //testing draw method----------------------------------
    public void drawPoint(Vector2 v, float radius) {
        Debug.DrawLine(new Vector2(v.x-radius, v.y), new Vector2(v.x+radius, v.y), Color.blue, 1f);
        Debug.DrawLine(new Vector2(v.x, v.y-radius), new Vector2(v.x, v.y+radius), Color.blue, 1f);
    }
    public void drawDebugLines() {
        Debug.DrawLine(getRangePoint(maxRange), getRangePoint(minRange), Color.green, 1f);
        float targetDist = getLineLength(userPosition, targetPosition);
        if (targetDist > maxRange) {
            drawPoint(getRangePoint(maxRange), attackArea);
        } else if (targetDist < minRange) {
            drawPoint(getRangePoint(minRange), attackArea);
        } else {
            drawPoint(targetPosition, attackArea);
        }
    }

    // save path object came from
    public string getPath() {
        return path;
    }
    public void setPath(string p) {
        path = p;
    }
}
