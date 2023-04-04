//Holden Ernest - June 25, 2022

// Contains *everything* the player has currently collected in the game
// on game saves these lists will be parsed into a file to be stored and read on load.
// items in these lists can be added and removed at will, updating any inventory UI or even *quests* (on item pickups)

// Quests and LearnedSkills dont need to be loaded for AI characters.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory : MonoBehaviour {
    // saved arrays (DO NO REF, only used for PlayerLoader) >> paths for the savedObjects
    public string[] items;
    public string[] skills;
    public string[] quests;

    // these lists are actually called apon.
    private List<Item> inventory = new List<Item>(); // list of items
    private List<Skill> learnedSkills = new List<Skill>(); // list of skills
    private List<Quest> activeQuests = new List<Quest>(); // list of quests

    void Start() {
        
    }

    public string overwriteLists() {
        items = listToArrayI(inventory);
        skills = listToArrayS(learnedSkills);
        quests = listToArrayQ(activeQuests);

        string temp = Knowledge.inventoryToJson(this); // convert this inventory to a json string

        // empty these temp arrays after they overwrote the files
        delInitArrs();

        return temp; // return it to be saved to a json file
    }
    // loading a new player will load with new items[] skills[] quests[] and then invoke this.
    public void loadLists() {
        setInventory(items);
        setLearnedSkills(skills);
        setQuests(quests);
        delInitArrs();
    }
    private void delInitArrs() {
        items = null;
        skills = null;
        quests = null;
    }
    public List<Item> getAllItems() {
        return inventory;
    }
    public List<Quest> getAllQuests() {
        return activeQuests;
    }
    public List<Skill> getAllSkills() {
        return learnedSkills;
    }

    //reset inventory from saved arrays
    public void setInventory(string[] i) {
        if (i == null) return;
        try {
            inventory.Clear();
        } catch (Exception e) {
            inventory = new List<Item>();
        }
        foreach(string itm in i) {
            int delimit = itm.LastIndexOf('/') + 1;
            string file = itm.Substring(delimit, itm.Length - delimit);
            if (itm.Substring(0,delimit).Equals("SavedObjects/Items/")) // if the path says its in the Items/ folder
                inventory.Add(Knowledge.getItem(file));
            else // its in the Equipables/ folder
                inventory.Add(Knowledge.getEquipable(file));
        }
    }

    // for player only--------
    public void setQuests(string[] q) {
        if (q == null) return;
        activeQuests.Clear();
        foreach(string qst in q) {
            int delimit = qst.LastIndexOf('/')+1;
            string file = qst.Substring(delimit, qst.Length - delimit);
            activeQuests.Add(Knowledge.getQuest(file));
        }
    }
    public void setLearnedSkills(string[] s) {
        if (s == null) return;
        learnedSkills.Clear();
        foreach(string skl in s) {
            int delimit = skl.LastIndexOf('/')+1;
            string file = skl.Substring(delimit, skl.Length - delimit);
            learnedSkills.Add(Knowledge.getSkill(file));
        }
    }

    // OVERWRITE STRING ARRAYS
    private string[] listToArrayI(List<Item> b) {
        string[] a = new string[b.Count];
        for (int i = 0; i < b.Count; i++) {
            a[i] = b[i].getPath();
        }
        return a;
    }
    private string[] listToArrayQ(List<Quest> b) {
        string[] a = new string[b.Count];
        for (int i = 0; i < b.Count; i++) {
            a[i] = b[i].getPath();
        }
        return a;
    }
    private string[] listToArrayS(List<Skill> b) {
        string[] a = new string[b.Count];
        for (int i = 0; i < b.Count; i++) {
            a[i] = b[i].getPath();
        }
        return a;
    }

    //items
    public void addItems (string itemName, int ammount) {
        for (int i = 0; i < ammount; i++) {
            addItem(itemName);
        }
    }
    public void addItem(string itemName) {
        inventory.Add(Knowledge.getItem(itemName));
    }
    public void loseItems (string itemName, int ammount) { // fix to work with the ammount variable on items instead
        for (int i = 0; i < ammount; i++) {
            loseItem(itemName);
        }
    }
    public void loseItem(string itemName) {
        inventory.Remove(Knowledge.getItem(itemName));
    }

    public void addEquips (string itemName, int ammount) {
        for (int i = 0; i < ammount; i++) {
            addEquip(itemName);
        }
    }
    public void addEquip(string itemName) {
        inventory.Add(Knowledge.getEquipable(itemName));
    }
    public void loseEquips (string itemName, int ammount) { // unequip if equipped.  <<<<<<<< ALSO inventory.remove() might not work like this??
        for (int i = 0; i < ammount; i++) {
            loseEquip(itemName);
        }
    }
    public void loseEquip(string itemName) {
        inventory.Remove(Knowledge.getEquipable(itemName));
    }

    //skills
    public void learnSkill(string s) {
        Skill newSkill = Knowledge.getSkill(s);
        learnSkill(newSkill);
    }
    public void learnSkill(Skill s) {
        learnedSkills.Add(s);
    }
    public void unlearnSkill(Skill s) {
        learnedSkills.Remove(s);
        for (int i = 0; i < Knowledge.player.usingSkills.Length; i++) {
            if (Knowledge.player.usingSkills[i] == s) Knowledge.player.usingSkills[i] = null;
        }

    }

    //quests
    public void addQuest(string q) {
        Quest newQuest = Knowledge.getQuest(q);
        // if (!newQuest.activeQuest()) // might need a check so quests cant get activated more than once. << including if its completed.
        activeQuests.Add(newQuest);
    }
    public void updateAllQuests(string type, string objective) {
        foreach (Quest q in activeQuests) {
            q.update(type, objective);
        }
    }

}
