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
    public static readonly float speedMult = 0.03f;
    public static readonly float resistMult = 0.05f; // resistance mult for everything
    public static readonly float necroMult = 1.5f; // how much worse necro is for everyone

    //per character variables:
    public int constitution = 0; // increases max health
    public int strength = 0; // more AD
    public int dexterity = 0; //small speed increase and better ranged damage
    public int intelligence = 0; // more magic damage >> affects responses?
    public int evasion = 0; // chance to actually get hit
    public int armor = 0; // % resistence >> based on clothing
    public int speed = 0; // affects controller speed value >> clothing weight can lower this depending on strength

    public int poisonResist = 0;
    public int psychResist = 0;

    public int xp = 0;
    public int level = 1; // + stat points, + BaseConstitution, + SmallStrength
    public int carriedWeight = 0;


    public Stats() {

    }

    public int getMaxHp(int oldMax) {
       return oldMax + (constitution * constMult) + (level * constMult);
    }
    public float getSpeed() {
        float val = (speed * speedMult) + (dexterity * dexSpeedMult);

        if (val == 0)
            return 1;
        else if (val < 0)
            val = 1/Math.Abs(val); //negative speeds give you a fraction instead

        return val;
    }


    public void addXp(int amt) {
        xp += amt;
        level = (int)Math.Cbrt(xp);
        Debug.Log("level: " + level + ", with " + xp + " xp!");
        Debug.Log("xp till next level: " + (Math.Pow(level+1, 3) % xp));
        

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
