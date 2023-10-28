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
    //public string texture; //only used to initialize tex (TEXTURE FOR THE CHARACTER)
    public string itemType; // Tell which slot its able to be equiped in?
    public string skillName; // only used to initialize grantSkill v
    public int damage = 0; // base damage for this weapon attack, has nothing to do with skills
    public Stats stats;

    private Texture2D tex; // a spritesheet texture that animates with the player >> NOT to be confused with 'Item.icon'.
    private Skill grantSkill;

    private bool isEqppd = false; // used to display an icon if the item is currently equipped

    //Constructors
    public Equipable() {
        itemType = "weapon";
    }
    
    public void randomizeEquipStats(int rarity) {
        //randomize the new equip with a certain ammount of stats based on rarity (good and bad stats)
    }

    public void unequip(Character c) { // called by Character.equip() if this equipable is equipped
        if (hasSkill())
            c.inventory.unlearnSkill(grantSkill);
        isEqppd = false;
    }
    public void equip(Character c) {
        if (hasSkill())
            c.inventory.learnSkill(grantSkill);
        isEqppd = true;
    }

    public Texture2D getTexture() { // get the player model texture(if there is one)
        return tex;
    }
    public bool updateTexture() { // returns if the texture is found
        string texture = getFileName();
        if (texture == "") return false;
        tex = Knowledge.getEquipTexture(texture);
        return tex != null;
    }

    public void updateSkill() {
        grantSkill = Knowledge.getSkill(skillName);
        updateDamage();
    }
    public void updateDamage() {
        grantSkill.setDamage(damage);
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
        return updateTexture();
    }
    public bool hasSkill() {
        return skillName != "";
    }

    public bool isEqual(Equipable e) {
        bool a = getPath() == e.getPath();
        bool b = itemName.Equals(e.itemName);
        bool c = stats.isEqual(e.stats);
        return a && b && c;
    }
    public override bool isEquip() {
        return true;
    }
    public override string ToString() {
        return itemName + " [" + itemType + "] for " + damage + " base attack";
    }

    public EquipSave toEquipSave() {
        EquipSave e = new EquipSave();
        e.path = getFileName();
        e.amount = amount;
        e.customName = itemName;
        e.stats = stats;

        return e;
    }
}
