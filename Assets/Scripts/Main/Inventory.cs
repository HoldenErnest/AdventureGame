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

    public List<Item> inventory = new List<Item>();
    public List<Skill> learnedSkills = new List<Skill>();
    public List<Quest> activeQuests = new List<Quest>();

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
    public void addEquips (string itemName, int ammount) {
        for (int i = 0; i < ammount; i++) {
            addEquip(itemName);
        }
    }
    public void addEquip(string itemName) {
        inventory.Add(Knowledge.getEquipable(itemName));
    }

    //skills
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

}
