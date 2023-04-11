//Holden Ernest - June 25, 2022

// Represents the values of all types of 'Item's
// 'itemName' is used to act on that item, select it for editing ammounts or whatnot.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item : MonoBehaviour {
    private string path; // path the object came from (put in as soon as it is taken from Knowledge.getItem())
    
    public string altIcon; // DONT INIT in json to set icon to default filename
    public string description;
    public string itemName; // name of the item for display purposes (BOTH item and equip have a custom name, but only equips save it)
    public string consumeEffect; // the effect to activate if you are to consume it.
    public int amount = 1;
    public float dropChance = 1f;

    public virtual string ToString() {
        return "[" + getFileName() + "]: " + itemName + ". amnt: " + amount;
    }
    public virtual bool isEquip() {
        return false;
    }
    public string getPath() {
        return path;
    }
    public void setPath(string p) {
        path = p;
    }
    public bool isConsumable() {
        return consumeEffect != null && consumeEffect != "";
    }

    // returns the string uid for the file
    public string getFileName() {
        int startFile = path.LastIndexOf('/') + 1;
        string sid = path.Substring(startFile, path.Length - startFile);
        return sid;
    }
    public string getIconName() {
        Debug.Log(altIcon ?? getFileName());
        return altIcon ?? getFileName();
    }

    public ItemSave toItemSave() {
        ItemSave i = new ItemSave();
        i.path = getFileName();
        i.amount = amount;
        return i;
    }
}
