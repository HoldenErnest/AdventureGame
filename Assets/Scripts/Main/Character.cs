//Holden Ernest -date close to project beginning-
//A base for all characters, keeps track of stats and things

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using UnityEngine.U2D.Animation;


public class Character : MonoBehaviour {

    private Controller controls;
    public Tools theTools; // TEMP -- initializing tools for Knowledge
    public GameObject statsUI; // empty gameobject encompasing the personal stats overlay (health and xp bars ect)
    private CustomSlider hpUI;
    private CustomSlider xpUI;
    public Hotbar[] hotbarItems;
    public GameObject otherHealthbar;
    public List<Equipable> equips = new List<Equipable>(); // list of all equiped items, when equipping new items update createCharacter() to update the character model if needed
    public string[] startingSkills = new string[5];
    public Skill[] usingSkills; // the players 'hotbar' of skills
    public List<Effect> activeEffects = new List<Effect>();

    public string name = "Fella";
    public string title = "The farmer";
    public int baseMaxHp = 100;
    public string bodyTexture;
    public Stats userStats = new Stats();
    
    private int hp;
    
    private int higherPlane = 0; // if you are on a higher plane than the ground floor (gets bouses like range and view // can shoot through the wall collision)

    private Character lastHitCharacter;

    void Start() {
        controls = GetComponent<Controller>();
        

        if (isPlayer()) {
            updateAll();
            userStats.speed = 10;
            updateSpeed();
            updateHotbarIcons();
            Knowledge.player = this;
            Knowledge.tools = theTools;
             // set this to the old saved inventory from xml
            Knowledge.inventory.addItem("apple");
            Knowledge.inventory.addItem("orange");
            Knowledge.inventory.addItem("apple");
            Knowledge.inventory.addItem("orange");
            Knowledge.inventory.addEquip("pointed_stick");
            Knowledge.inventory.addEquip("blue_pants");
            Knowledge.inventory.addItem("orange");
            Knowledge.inventory.addItem("orange");
            Knowledge.inventory.addItem("apple");
            Knowledge.inventory.addEquip("red_cap");
            Knowledge.inventory.addItem("apple");
            Knowledge.inventory.addItem("orange");
            Knowledge.inventory.addItem("apple");
            Knowledge.inventory.addEquip("red_shirt");
            Knowledge.inventory.addEquip("green_shirt");
            Knowledge.inventory.addEquip("grey_shirt");
            Knowledge.inventory.addItem("apple");
            Knowledge.inventory.addItem("apple");
            Knowledge.inventory.addItem("apple");


            Knowledge.inventory.addQuest("hunter");Knowledge.inventory.addQuest("intro");Knowledge.inventory.addQuest("intro");Knowledge.inventory.addQuest("intro");Knowledge.inventory.addQuest("intro");Knowledge.inventory.addQuest("intro");Knowledge.inventory.addQuest("intro");
            Knowledge.inventory.addQuest("test1");
            Knowledge.inventory.addQuest("hunter");Knowledge.inventory.addQuest("hunter");Knowledge.inventory.addQuest("hunter");Knowledge.inventory.addQuest("hunter");Knowledge.inventory.addQuest("hunter");
            //Knowledge.effectToJson(new Effect());
            //Knowledge.skillToJson(new Skill());
            //Knowledge.questToJson(new Quest());
            //Knowledge.itemToJson(new Item());
            //Knowledge.equipToJson(new Equipable());
            //Knowledge.overwriteInventoryJson(); // LEGACY, characters and their items are stored in the same CharacterCreator JSON
        }
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
    public Skill getSkill(int i) { // return array of currently equiped items
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
        return gameObject.name.Equals("Player");
    }
    public float getHealthPercentage() {
        //Debug.Log("current HP: " + hp + " and max is " + userStats.getMaxHp(baseMaxHp));
        return hp / userStats.getMaxHp(baseMaxHp);
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
    public void setItems(string[] i) {
        foreach (string s in i) {
            Debug.Log(s);
            Debug.Log(Knowledge.inventory.inventory);
            Knowledge.inventory.addItem(s);
        }
    }
    public void setStartingSkills(string[] s) { // max of 5 skills active
        if (s == null) return;
        for (int i = 0; i < s.Length; i++) {
            startingSkills[i] = s[i];
        }
    }
    public void setBodyTex(string s) {
        bodyTexture = s;
    }
    public void setCharIcon(string s) {
        //set this characters icon
    }
    //-----------------------------------Update values------------------------------
    // used when creating a new character, to initially update the passed values
    public void updateAll() {
        controls = GetComponent<Controller>();
        updateSpeed();
        setupUI();
        setStartSkills();

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
                Texture2D tex = null;
                try {
                    tex = Knowledge.getSkillIcon(usingSkills[i].skillName);
                } catch {
                    tex = Knowledge.getSkillIcon("noSkill");
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
        hpUI.updateSlider(hp, userStats.getMaxHp(baseMaxHp));
    }
    public void updateXpSlider() {
        xpUI.updateSlider(userStats.getXp(), userStats.getMaxXp());
    }
    private void updateDeath() {
        if (hp <= 0) {
            giveXpToLastHit();
            if (lastHitCharacter.isPlayer()) { // player killed this character.
                Knowledge.inventory.updateAllQuests("kill", name);
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
            newHp = Instantiate(otherHealthbar, transform);
            hpUI = newHp.GetComponent<CustomSlider>();
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
        e.equip();
        if (e.hasTexture()) createCharacter(); // if the equipped item had a model texture, reload the character texture asset.
    }
    public void unequip(Equipable e) {
        int rIndex = indexFromList(equips, e);
        if (rIndex != -1) {
            equips.RemoveAt(rIndex);
            e.unequip();
            if (e.hasTexture()) createCharacter();
        } else {
            Debug.Log("CANNOT FIND THIS OBJECT IN THE LIST?");
        }
    }

    private int indexFromList(List<Equipable> l, Equipable e) {
        for (int i = 0; i < l.Count; i++) {
            if (l[i].sid == e.sid && l[i].itemName == e.itemName) return i; // must be compared with the internal values rather than pointers or it will believe all Equippables are "e"
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
        if (isPlayer())
            hotbarItems[Controller.skillInUse].startReload(s.getReloadTime());
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
        updateHpSlider();
        updateDeath();
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
}
