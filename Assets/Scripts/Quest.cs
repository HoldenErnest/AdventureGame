using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Quest {

    public bool isComplete;
    public bool rewardsCollected;
    public string title;
    public string desc;
    public QuestReward[] rewards;
    public QuestItem[] items;

    public Quest () {
        isComplete = false;
        rewardsCollected = false;
        title = "Unknown Quest";
        desc = "This quest is unknown";
        rewards = new QuestReward[] {new QuestReward()};
        items = new QuestItem[] {new QuestItem(), new QuestItem(), new QuestItem()};
    }

    public bool itemsHasType(string t) { // if this quest has an item of a certain type (used to turn on or off testing for certain interactions)
        foreach (QuestItem item in items) {
            if (item.type.Equals(t) && !item.isComplete) return true;
        }
        return false;
    }

    public void updateCompletion() {
        foreach (QuestItem item in items) {
            if (!item.isComplete)  {
                isComplete = false;
                return;
            }
        }
        isComplete = true;
    }

    public void collectAllRewards() {
        if (rewardsCollected) return;

        foreach (QuestReward reward in rewards) {
            reward.redeem();
        }
        rewardsCollected = true;
    }
}

[Serializable]
public class QuestItem {

    public string desc; // literal description of quest item
    public string type; // the action needed
    public string objective; // the name of the noun the type of quest is acting on
    public int total;
    private int current; // the current quest item the player is on.
    public bool isComplete;

    public QuestItem () {
        desc = "Exterminate 1 bat with the newly aquired power";
        type = "kill"; // (kill objectives), (collect objectives), (talk to objectives), (remove objectives)
        objective = "bat"; // name of enemy, item, or talkable character
        current = 1;
        total = 1;
        isComplete = false;
    }
}

[Serializable]
public class QuestReward {

    public string rewardName;
    public string rewardType;
    public int ammount;

    public QuestReward () {
        rewardName = "";
        rewardType = "xp"; // (skill), (xp), (item), (equip), (remove)
        ammount = 50;
    }

    public void redeem() {
        if (rewardType.Equals("xp")) {
            //Inventory.player.addXp(ammount);
        } else if (rewardType.Equals("item")) {
            //Inventory.addItems(rewardName, ammount);
        } else if (rewardType.Equals("equip")) {
            //Inventory.addEquips(rewardName, ammount);
        } else if (rewardType.Equals("remove")) {
            //Inventory.loseItem(rewardName, ammount);
        } else if (rewardType.Equals("skill")) {
            //Inventory.learnSkill(rewardName);
        }
    }
}