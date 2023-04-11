using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EquipSave {
    public string path; // the filename (located within SO/Equips/ )
    public int amount;
    public string customName;
    public Stats stats;

    public Equipable toEquip() {
        Equipable e = Knowledge.getEquipable(path);
        e.amount = amount;
        e.itemName = customName;
        e.stats = stats;
        return e;
    }
    public override string ToString()
    {
        return path + " CN! " + customName + " am: " + amount;
    }
}
