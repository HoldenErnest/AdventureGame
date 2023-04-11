using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemSave {
    public string path; // this is the item name only;
    public int amount;
    

    public Item toItem() {
        Item i = Knowledge.getItem(path);
        i.amount = amount;
        return i;
    }
    public override string ToString()
    {
        return path + " am: " + amount;
    }
}
