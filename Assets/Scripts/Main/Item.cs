//Holden Ernest - June 25, 2022

// Represents the values of all types of 'Item's
// 'itemName' is used to act on that item, select it for editing ammounts or whatnot.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item : MonoBehaviour {
    public string sid; // real item name, used to refrence the items icon
    public string itemName; // custom name
    public int ammount;

    public virtual string ToString() {
        return "[" + sid + "]: " + itemName + ". amnt: " + ammount;
    }
}
