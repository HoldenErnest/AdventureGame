//Holden Ernest - June 25, 2022

// Contains *everything* the player has currently collected in the game
// on game saves these lists will be parsed into a file to be stored and read on load.
// items in these lists can be added and removed at will, updating any inventory UI or even *quests* (on item pickups)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory : MonoBehaviour {

    public List<Item> inventory = new List<Item>(); // list of items
    public List<Skill> learnedSkills = new List<Skill>(); // list of skills
    public List<Quest> activeQuests = new List<Quest>(); // list of quests

    void Start() {
        inventory = new List<Item>();
    }


    public List<Item> getAllItems() {
        return inventory;
    }
    public List<Quest> getAllQuests() {
        return activeQuests;
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
