using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stats {

    private System.Random rand = new System.Random();

    // GLOBAL Multipliers for stats >> how much impact each point actually has
    public static readonly float strMult = 0.05f; // each point adds 5% more damage vvv ect.
    public static readonly int constMult = 10;
    public static readonly float dexSpeedMult = 0.01f;
    public static readonly float dexStrMult = 0.15f;
    public static readonly float intelMult = 0.15f;
    public static readonly float evadeMult = 0.05f;
    public static readonly float armorMult = 0.10f;
    public static readonly float speedMult = 0.12f;
    public static readonly float resistMult = 0.05f; // resistance mult for everything
    public static readonly float necroMult = 1.5f; // how much worse necro is for everyone

    //per character variables:
    public int constitution = 0; // increases max health
    public int strength = 0; // more AD
    public int dexterity = 0; //small speed increase and better ranged damage
    public int intelligence = 0; // more magic damage >> affects responses?
    public int evasion = 0; // chance to actually get hit
    public int speed = 0; // affects controller speed value >> clothing weight can lower this depending on strength

    public int xp = 1; // total xp gathered

    //set by armors or runes or something, cant spec into
    public int armor = 0; // % resistence >> based on clothing
    public int poisonResist = 0;
    public int psychResist = 0;

    //stats determined by other factors
    private int level = 1; // + stat points, + BaseConstitution, + SmallStrength
    private int attrPoints = 0;
    private int carriedWeight = 0;


    public Stats() {

    }

    public int getMaxHp(int baseMax) {
       return baseMax + (constitution * constMult) + (level * constMult);
    }
    public float getSpeed() { // returns the float value for speed, not the stat int
        float val = 1 + (speed * speedMult) + (dexterity * dexSpeedMult);
        if (val < 1)
            val = 1/Math.Abs(val-2); //negative speeds give you a fraction instead

        return val;
    }

    public void setLevel(int l) { // set current xp based on a specified wanted level.
        xp = (int)Math.Pow(l - 1, 3);
        level = l;
    }
    public void addXp(int amt) {
        xp += amt;
        int newLevel = (int)Math.Floor(Math.Cbrt(xp)) + 1;
        if (newLevel > level) { // level up
            attrPoints += (newLevel - level)*2;
        } else if (newLevel < level) { // character somehow lost levels
            resetAllAttr();
        }
        Debug.Log($"added {amt} xp");
        //Debug.Log("level: " + level + ", with " + xp + " xp!");
        //Debug.Log("current XP: " + getXp() + " out of " + getMaxXp());
        
    }
    public int getXp() { // xp gained in the current level (!! NOT TOTAL XP !!)
        return xp - (int)Math.Floor(Math.Pow(level - 1, 3));
    }
    public int getMaxXp() { // total xp getting to a new level would take
        return (int)Math.Pow(level, 3) - (int)Math.Pow(level - 1, 3);
    }
    public int getLevel() {
        return level;
    }
    public int getNeededXp() { // xp currently needed to level up
        return (int)Math.Pow(level, 3) - xp;
    }
    public int getOnKillXp() { // only used to represent how much xp someone gets for killing this character
        int specifiedXp = (int)Mathf.Ceil(getMaxXp() / 10); // MIGHT NEED TO FIX THIS! kill avg of 20 guys the same level as you to level up or maybe 9 or so, 1 level up.
        int addRandom = (int)rand.Next((int)Mathf.Floor(-specifiedXp / 5), (int)Mathf.Ceil(specifiedXp / 5));
        return specifiedXp + addRandom;
    }
    private int getTotalAttr() {// Hypothetical ammount to available if playing by the rules
        return getLevel()*2;
    }
    private int getTotalAttrUsed() {
        return constitution+strength+dexterity+intelligence+evasion+speed;
    }
    public int getAvailableAttr() {
        return (getTotalAttr() - getTotalAttrUsed());
    }

    public void resetAllAttr() {
        constitution = 0;
        strength = 0;
        dexterity = 0;
        intelligence = 0;
        evasion = 0;
        speed = 0;

        attrPoints = getTotalAttr();
    }

    // Calculates and returns damage based on the type of attack and the users armor
    public int calculateDamageRecieve(int baseDamage, string type) {
        switch (type) {
            case "physical":
                if(evaded()) return 0;
                return getArmorResist(baseDamage);

            case "magical":
                if(evaded()) return 0;
                return baseDamage;

            case "bleed":
                return baseDamage;

            case "poison":
                return variableDamage(getPoisonResist(baseDamage), baseDamage/10);

            case "necro":
                return baseDamage;

            case "heal":
                return -baseDamage;
            
        }

        return baseDamage;
    }
    // Calculates and returns damage based on the type of attack and the pairing stats that may improve it
    public int calculateDamageAttack(int baseDamage, string type) {
        switch (type) {
            case "physical":
                return baseDamage+(int)(baseDamage * strMult * strength);

            case "magical":
                return baseDamage+(int)(baseDamage * intelMult * intelligence);

            case "bleed":
                return baseDamage+(int)(baseDamage * strMult * strength);

            case "poison":
                return baseDamage+(int)(baseDamage * intelMult * intelligence);

            case "necro":
                return baseDamage+(int)(baseDamage * necroMult);

            case "heal":
                return baseDamage;
            
        }
        return baseDamage;
    }

    //Calculate resistances
    public int getPoisonResist(int damage) {
        return damage - (int)(damage * poisonResist * resistMult);
    }
    public int getArmorResist(int damage) {
        return damage - (int)(damage * armor * armorMult);
    }
    public int getPsychResist(int damage) {
        return damage - (int)(damage * psychResist * resistMult);
    }

    public int variableDamage(int damage, int var) { // apply variability to damage, +- var
        return damage + rand.Next(-var, var);
    }
    public bool evaded() { // return an evade attempt
        if ((evasion * evadeMult) < (float)rand.Next(0,100)/100) {
            return false;
        }
        //Debug.Log("evaded with a " + (evasion * evadeMult) + "% chance");
        return true;
    }

}
