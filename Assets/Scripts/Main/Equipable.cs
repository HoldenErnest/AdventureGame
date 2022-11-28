//Holden Ernest - May 6, 2022

// A type of 'Item' that, unlike an item, can be used(in different ways).
// all equipables have different stats

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Equipable : Item
{
    public string texture; //only used to initialize tex
    public string itemType; // Tell which slot its able to be equiped in?
    public string skillName; // only used to initialize grantSkill v

    private Texture2D tex; // a spritesheet texture that animates with the player >> NOT to be confused with 'Item.icon'.
    private Skill grantSkill;

    //Weapon base stats
    public int damage = 0; // base damage for this weapon attack, has nothing to do with skills
    public int armorStat = 0;

    public int weight = 0; // weight only matters for equipped things, no items in storage slow the user down

    // boost stats >> based on reforges
    public int speedStat = 0;
    public int constStat = 0;
    public int intelStat = 0;
    public int strengthStat = 0;
    public int evadeStat = 0;

    public int poisonResist = 0;
    public int necroResist = 0;
    public int psychResist = 0;

    private bool isEqppd = false; // used to display an icon if the item is currently equipped

    //Constructors
    public Equipable() {
        itemType = "weapon";
    }

    public void setStats(int sped, int con, int intel, int str, int evd, int poiRes, int necRes, int psyRes) {
        speedStat = sped;
        constStat = con;
        intelStat = intel;
        strengthStat = str;
        evadeStat = evd;
        poisonResist = poiRes;
        necroResist = necRes;
        psychResist = psyRes;
    }
    public void randomizeEquipStats(int rarity) {
        //randomize the new equip with a certain ammount of stats based on rarity (good and bad stats)
    }

    public void unequip() { // called by Character.equip() if this equipable is equipped
        if (hasSkill())
            Knowledge.inventory.unlearnSkill(grantSkill);
        isEqppd = false;
    }
    public void equip() {
        if (hasSkill())
            Knowledge.inventory.learnSkill(grantSkill);
        isEqppd = true;
    }

    public Texture2D getTexture() { // get the player model texture(if there is one)
        return tex;
    }
    public bool updateTexture() { // returns if the texture is found
        if (texture == "") return false;
        tex = Knowledge.getEquipTexture(texture);
        return tex != null;
    }

    public void updateSkill() {
        grantSkill = Knowledge.getSkill(skillName);
        grantSkill.baseDamage = damage;
    }
    public void updateDamage() {
        grantSkill.setDamage(damage);
    }
    public int getSpeedStat() {
        return speedStat;
    }
    public int getConstStat() {
        return constStat;
    }
    public int getIntelStat() {
        return intelStat;
    }
    public int getStrengthStat() {
        return strengthStat;
    }
    public int getEvadeStat() {
        return evadeStat;
    }
    public Skill getGrantSkill() {
        return grantSkill;
    }

    public int getWeaponDamage() {
        return damage;
    }
    public bool isEquipped() {
        return isEqppd;
    }

    public bool hasTexture() { // might not neccessarly find the texture but it does have a request for one
        return texture != "";
    }
    public bool hasSkill() {
        return skillName != "";
    }

    public override string ToString() {
        return itemName + " [" + itemType + "] for " + damage + " base attack";
    }
}
