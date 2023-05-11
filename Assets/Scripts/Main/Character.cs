//Holden Ernest -date close to project beginning-
//A base for all characters, keeps track of stats and things

// everything is loaded and saved to a seprate characterCreator class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using UnityEngine.U2D.Animation;

public class Character : MonoBehaviour {
    private string path;
    private bool charIsPlayer = false;
    private Controller controls;
    private GameObject statsUI; // player only (current level and HP UI stuff)
    private CustomSlider hpUI;
    private CustomSlider xpUI;
    public Hotbar[] hotbarItems;
    public List<Equipable> equips = new List<Equipable>(); // list of all equiped items, when equipping new items update createCharacter() to update the character model if needed
    public string[] startingSkills = new string[5];
    public Skill[] usingSkills; // the players 'hotbar' of skills
    public List<Effect> activeEffects = new List<Effect>();

    public string name = "Fella";
    public string title = "The farmer";
    public string description = "An old buntown farmer from the old times";
    public int baseMaxHp = 100;
    public string bodyTexture;
    public Stats userStats = new Stats();

    public Inventory inventory = new Inventory(); // keep track of new obtained items, skills, and quests

    private int hp;
    
    private int higherPlane = 0; // if you are on a higher plane than the ground floor (gets bouses like range and view // can shoot through the wall collision)

    private Coroutine hideHPRoutine;
    private float closeTime = 10.0f; // time for hp to hide after not taking damage

    private Character lastHitCharacter;

    void Awake() {
        controls = GetComponent<Controller>();
    }

    public void createCharacter() { //create new texture from equips textures(if they have one)
        Texture2D tex = Knowledge.getBodyTexture(bodyTexture);
        foreach (Equipable e in equips) {
            if (e.updateTexture()) { // if the texture is updated
                tex = ImageMerge.mergeTextures(tex, e.getTexture());
            }
            tex.filterMode = FilterMode.Point;
        }
        gameObject.GetComponent<SpritemapAnimation>().updateTexture(tex);
        
    }

    //-----------------------------------Setter/Getter------------------------------
    public List<Equipable> getEquipables() { // return array of currently equiped items
        return equips;
    }
    public Skill getSkill(int i) { // return skill that should be used
        return usingSkills[i];
    }
    public int getLevel() {
        return userStats.getLevel();
    }
    public Controller GetController() {
        return controls;
    }
    public int getHigherPlane() {
        return higherPlane;
    }
    public void setHigherPlane(int i) {
        higherPlane = i;
    }
    public bool isPlayer() {
        return charIsPlayer;
    }
    public void setPlayer() {
        charIsPlayer = true;
        updateAll();

        // temp nonesense for OPness
        userStats.speed = 10;
        updateSpeed();
    }
    public float getHealthPercentage() {
        //Debug.Log("current HP: " + hp + " and max is " + userStats.getMaxHp(baseMaxHp));
        return hp / userStats.getMaxHp(baseMaxHp);
    }
    public int getHp() {
        return hp;
    }
    //Health
    public void fullHealth() {
        hp = userStats.getMaxHp(baseMaxHp);
        updateHpSlider();
    }
    public void loseHealth(int health) {
        hp -= health;
    }
    public void addHealth(int health) {
        hp += health;
    }
    public void addXp(int amt) {
        userStats.addXp(amt);
        if (xpUI)
            updateXpSlider();
    }
    public void kill() {
        hp = 0;
        updateDeath();
    }
    public void setLastHit (Character c) {
        lastHitCharacter = c;
    }
    public void setStatsUI (GameObject g) {
        statsUI = g;
    }
    public float getAvgSkillDist() {
        float total = 0;
        int totSkls = 0;
        foreach (Skill s in usingSkills) {
            if (s.baseDamage > 0) {
                totSkls++;
                total += s.minRange + s.maxRange;
            }
        }
        return total / (totSkls * 2); // * 2 for avg of minRange and maxRange
    }
    public float getLowSkillDist() { // get the range for the smallest ranged skill the charcter has
        float lowest = -1;
        foreach (Skill s in usingSkills) {
            if (s.baseDamage > 0)
            if (s.minRange < lowest || lowest == -1) lowest = s.minRange;
        }
        return lowest;
    }
    public float getHighSkillDist() { // get the range for the highest ranged skill the charcter has
        float highest = -1;
        foreach (Skill s in usingSkills) {
            if (s.baseDamage > 0)
            if (s.maxRange > highest) highest = s.maxRange;
        }
        return highest;
    }

    // SETTERS ONLY CALLED WHEN CREATING CHARACTER::
    public void setName(string n) {
        name = n ?? "Unknown";
    }
    public void setTitle(string s) {
        title = s ?? "";
    }
    public void setBaseHp(int n) {
        if  (String.IsNullOrEmpty(n.ToString())) n = 100;
        else if (n <= 0 ) n = 1;
        else if (n > 10000 ) n = 10000; // max base hp = 10000?
        baseMaxHp = n;
    }
    public void setStats(Stats s) {
        userStats = s;
        userStats.updateLevel();
    }
    public void setEquips(string[] e) {
        List<Equipable> es = new List<Equipable>();
        foreach (string ee in e) {
            es.Add(Knowledge.getEquipable(ee));
        }
        equipAll(es);
    }
    public void setItems(ItemSave[] i) {
        if (!inventory)
        inventory = new Inventory();
        inventory.setInventory(i);
    }
    public void setStartingSkills(string[] s) { // max of 5 skills active
        if (s == null) return;
        for (int i = 0; i < s.Length; i++) {
            startingSkills[i] = s[i];
        }
    }
    public void setDescription(string s) {
        description = s ?? "Unknown Description";
    }
    public void setBodyTex(string s) {
        bodyTexture = s;
    }
    public void setCharIcon(string s) {
        //TODO, set this characters icon
    }
    public void setHotbar(Hotbar[] h) {
        hotbarItems = h;
    }
    //-----------------------------------Update values------------------------------
    // used when creating a new character, to initially update the passed values
    public void updateAll() {
        controls = GetComponent<Controller>();
        updateSpeed();
        setupUI();
        setStartSkills();
        if (isPlayer()) {
            updateHotbarIcons();
        }
        fullHealth();
        createCharacter();
    }
    private void updateSkillsUsers() { // once a skill is obtained set its user to it works properly
        foreach (Skill skl in usingSkills) {
            if (!GameObject.ReferenceEquals( skl, null))
                skl.setUser(this.gameObject);
        }
    }
    public void updateHotbarIcons() {
        
        for (int i = 0; i < usingSkills.Length; i++) {
            Texture2D tex = tex = Knowledge.getSkillIcon("noSkill");
            if (!usingSkills[i].isEmpty()) {
                Debug.Log(usingSkills[i].skillName + " was gathered");
                string n = usingSkills[i].getPath();
                tex = Knowledge.getSkillIcon(n.Substring(n.LastIndexOf("/")+1));
            }
            hotbarItems[i].updateIcon(tex);
        }
    }
    public void updateSpeed() {
        float val = userStats.getSpeed();
        controls.updateSpeed();
    }
    public float getSpeed() {
        return userStats.getSpeed();
    }
    public void updateHpSlider() {
        if (hpUI) {
            hpUI.updateSlider(hp, userStats.getMaxHp(baseMaxHp));
            showHealthbar();
        }
        if (!isPlayer()) {
            if (Knowledge.tools)
            Knowledge.tools.updateHealth();
        }
    }
    public void updateXpSlider() {
        xpUI.updateSlider(userStats.getXp(), userStats.getMaxXp());
    }
    private void updateDeath() {
        if (hp <= 0) {
            giveXpToLastHit();
            if (lastHitCharacter.isPlayer()) { // player killed this character.
                inventory.updateAllQuests("kill", name);
            }
            Destroy(this.gameObject);
        }
    }
    private void giveXpToLastHit() {
        if (lastHitCharacter) {
            lastHitCharacter.addXp(userStats.getOnKillXp());
        }
    }
    private void setupUI() { // set the UI scripts based on the specified UI gameobjects
        GameObject newHp;
        if (!isPlayer()) { // if its an AI, create a floating healthbar
            newHp = Instantiate(Knowledge.getHpUI(), transform);
            hpUI = newHp.transform.GetChild(0).GetComponent<CustomSlider>();
        } else {
            GameObject newXp;
            newHp = statsUI.transform.GetChild(0).gameObject;
            newXp = statsUI.transform.GetChild(1).gameObject;
            hpUI = newHp.GetComponent<CustomSlider>();
            xpUI = newXp.GetComponent<CustomSlider>();
        }
        if (hpUI)
            updateHpSlider();
        if (xpUI)
            updateXpSlider();
    }
    private void setStartSkills() { // setup your hotbar of skills based on inputed "startingSkills"
        usingSkills = new Skill[5];
        for (int i = 0; i < startingSkills.Length; i++) {
            usingSkills[i] = Knowledge.getSkill(startingSkills[i]);
        }
        updateSkillsUsers();   // set any used skills you have to have this character as the user
    }
    public void equipAll(List<Equipable> es) {
        foreach (Equipable e in es) {
            equip(e);
        }
    }
    public void equip(Equipable e) { // Equip all items from here

        for (int i = 0; i < equips.Count; i++) {
            if (equips[i].itemType.Equals(e.itemType)) { // if there are multiple of the same type
                unequip(equips[i]);
                i--;
            }
        }
        equips.Add(e);
        e.equip(this);
        if (e.hasTexture()) createCharacter(); // if the equipped item had a model texture, reload the character texture asset.
    }
    public void unequip(Equipable e) {
        int rIndex = indexFromList(equips, e);
        if (rIndex != -1) {
            equips.RemoveAt(rIndex);
            e.unequip(this);
            if (e.hasTexture()) createCharacter();
        } else {
            Debug.Log("CANNOT FIND THIS OBJECT IN THE LIST?");
        }
    }

    private int indexFromList(List<Equipable> l, Equipable e) {
        for (int i = 0; i < l.Count; i++) {
            if (l[i].isEqual(e)) return i; // must be compared with the internal values rather than pointers or it will believe all Equippables are "e"
        }
        return -1;
    }

    //Timers
    public void invokeSkillCooldown(Skill s) { // set a reload for cooldowns
        StartCoroutine(skillCooldown(s));
    }
    public IEnumerator skillCooldown(Skill s) {
        s.setReloading(true);
        //Debug.Log(this.gameObject.name + " is reloading " + s.skillName);
        if (isPlayer()) {
            try {
                hotbarItems[Controller.skillInUse].startReload(s.getReloadTime());
            } catch (Exception e) {
                Debug.Log("hotbarItems not init: " + e);
            }
        }
        yield return new WaitForSeconds(s.getReloadTime());
        s.setReloading(false);
    }
    private IEnumerator handleEffects(Effect e) { // recursive timed loop to affect the players hp
        //Debug.Log(e.effectType + e.effectTier + " damage with " + e.duration);
        yield return new WaitForSeconds(1);
        if (e.duration > 0 && e.isActive()) {
            damageByType(e.getDamageAfterUser(), e.effectType);
            e.duration--;
            StartCoroutine(handleEffects(e));
        } else { // the effect is done -> stop and delete
            activeEffects.Remove(e);
            e = null;
        }
        //REMOVE EFFECT FROM Player.activeEffects[] AND DELETE EFFECT
    }
    // shows but sets/resets a timer to close after x seconds
    private void showHealthbar() {
        if (hideHPRoutine != null) StopCoroutine(hideHPRoutine);
        hideHPRoutine = StartCoroutine("hideHealthbar");
    }
    private IEnumerator hideHealthbar() {
        hpUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(closeTime);
        hpUI.gameObject.SetActive(false);
    }

    public void addEffect(string eType, int eTier, GameObject caster) {
        Effect oldEffect = null;
        foreach (Effect e in activeEffects) { // find if there is an existing effect of that type to replace if new effect is higher tier
            if (eType.ToLower().Equals(e.effectType.ToLower())) {
                oldEffect = e;
            }
        }
        if (!GameObject.ReferenceEquals( oldEffect, null)) {
            if (eTier > oldEffect.effectTier) {
                oldEffect.endEffect();                                      //<<<<<<<<<<<<<<<<<<<<< might not be working(stop coroutine) >> maybe new is called before it can register the effect in the array
                startNewEffect(eType, eTier, caster);// replace with the better tier effect instead
            } else {
                //exact same effect is already in use, refresh durration of effect instead!!!!!!!!!!
            }
        } else { // if there is no existing effect like it, create the new type of effect and handle it
            startNewEffect(eType, eTier, caster);
        }
    }

    public void startNewEffect(string eType, int eTier, GameObject caster) { // use addEffect() to effect the character instead to check if the same type of effect is already in use
            Effect newEffect = Knowledge.getEffect(eType, eTier);
            newEffect.user = caster;
            activeEffects.Add(newEffect);
            StartCoroutine(handleEffects(newEffect));
    }

    public void damageByType(int d, string type) { // pass in base damage and damage type to damage with resistences and such
        int m = userStats.calculateDamageRecieve(d, type);
        damage(m);
    }
    public int attackByType(int d, string type) { // base damage * damage type multipliers >> new damage (right now effects use this which is way too powerful)
        int m = userStats.calculateDamageAttack(d,type);
        return m;
    }

    public void damage(int damage) { // deal damage to the player with either a physical or magical attack
        if (damage == 0) return;
        hp -= damage;
        Debug.Log(name + " damaged for it " + damage);
        updateHpSlider();
        updateDeath();
    }

    // converts this character to a CharacterCreator for saving purposes
    public CharacterCreator toBlueprint() {
        CharacterCreator cc = new CharacterCreator();
        cc.name = name;
        cc.title = title;
        cc.baseHealth = baseMaxHp;
        cc.team = gameObject.GetComponent<Team>().getTeam();
        cc.stats = userStats;
        cc.homePos = new float[2];
        cc.homePos[0] = gameObject.transform.position.x;
        cc.homePos[1] = gameObject.transform.position.y;
        cc.equipment = equipsToString();
        cc.items = new ItemSave[0]; //TODO SET ITEMS FOR NON-PLAYERS!!!!!
        cc.startingSkills = usingSkillsToString();
        cc.bodyTexture = bodyTexture;
        //TODO cc.icon = new string for current icon 
        return cc;
    }
    private string[] equipsToString() {
        string[] eArr = new string[equips.Count];
        for(int i = 0; i < equips.Count; i++) {
            string file = equips[i].getPath();
            int delimit = file.LastIndexOf('/') + 1;
            file = file.Substring(delimit, file.Length - delimit);
            eArr[i] = file;
        }
        return eArr;
    }
    private string[] usingSkillsToString() {
        string[] eArr = new string[5];
        for(int i = 0; i < usingSkills.Length; i++) {
            if (usingSkills[i].isEmpty()) continue;
            string file = usingSkills[i].getPath();
            int delimit = file.LastIndexOf('/') + 1;
            file = file.Substring(delimit, file.Length - delimit);
            eArr[i] = file;
        }
        return eArr;
    }

    /*
    public void addAllEquipStats() {
        foreach (Equipable e in equips) {
            constitution += e.getConstStat();
            speed += e.getSpeedStat();
            intelligence += e.getIntelStat();
            strength += e.getStrengthStat();
            evasion += e.getEvadeStat();
        }
    }
    public void removeAllEquipStats() {
        foreach (Equipable e in equips) {
            constitution -= e.getConstStat();
            speed -= e.getSpeedStat();
            intelligence -= e.getIntelStat();
            strength -= e.getStrengthStat();
            evasion -= e.getEvadeStat();
        }
    }
    */

    // so it can be overwritten later
    public string getPath() {
        return path;
    }
    public void setPath(string p) {
        path = p;
    }
}
