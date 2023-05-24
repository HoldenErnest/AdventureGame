using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Quest {

    public string file; // file this was saved under
    public bool isComplete;
    public bool rewardsCollected;
    public string title;
    public string desc;
    public int reccomendedLevel; // reccomended level to take on the quest (to get a gague on difficulty)
    public bool isMainQuest;
    private int itemsDone; // the ammount of quest items the player has completed.
    public QuestReward[] rewards;
    public QuestItem[] items;

    public Quest () {
        isComplete = false;
        rewardsCollected = false;
        title = "Unknown Quest";
        desc = "This quest is unknown";
        reccomendedLevel = 0;
        isMainQuest = false;
        itemsDone = 0;
        rewards = new QuestReward[] {new QuestReward()};
        items = new QuestItem[] {new QuestItem(), new QuestItem(), new QuestItem()};
    }

    public string getNextDialoguePath() { // returns the next dialogue piece depending on how many items youve completed
        return Knowledge.questsPath + file + "/item" + itemsDone;
    }

    public bool itemsHasType(string t) { // if this quest has an item of a certain type (used to turn on or off testing for certain interactions)
        foreach (QuestItem item in items) {
            if (item.type.Equals(t) && !item.isComplete) return true;
        }
        return false;
    }
    public bool updateCompletion() {
        itemsDone = 0;
        foreach (QuestItem item in items) {
            if (!item.isComplete)  {
                isComplete = false;
                Debug.Log($"Overall Quest quota ({title}): {itemsDone} / {items.Length}");
                return false;
            }
            itemsDone++;
        }
        completeQuest();
        return true;
        
    }
    public void completeQuest() {
        isComplete = true;
        Debug.Log($"Completed Quest ({title}): {itemsDone} / {items.Length}");
        collectAllRewards(); // rewards given as soon as quest is completed.. maybe change to redeem rewards in quest menu.

    }
    public void collectAllRewards() {
        if (rewardsCollected) return;

        foreach (QuestReward reward in rewards) {
            reward.redeem();
        }
        rewardsCollected = true;
    }
    public int getProgress() { // quest progress, how many quest items are completed
        return itemsDone;
    }
    public bool update(string type, string objective) { // type of questitem to see if its even applicable --> after talking to a guy >> update("talk", "bilbo");
        Debug.Log("testing whether there is any quests to update");
        if (itemsDone >= items.Length)  { return updateCompletion(); } // << returns whether the quest is complete
        items[itemsDone].updateCompletion(this, type, objective); // update the current questItem 
        return updateCompletion();
    }

    // save path object came from
    public string getFile() {
        return file;
    }
    public void setFile(string f) {
        file = f;
    }

    public QuestSave toSave() {
        QuestSave qs = new QuestSave();
        qs.file = file;
        qs.gotReward = rewardsCollected;
        qs.itemsDone = getCompleted();
        return qs;
    }

    // returns how many quest Items are complete
    private int getCompleted() {
        int total = 0;
        foreach (QuestItem qi in items) {
            if (!qi.isComplete) break;
            total++;
        }
        return total;
    }

}

[Serializable]
public class QuestItem {

    public string desc; // literal description of quest item
    public string type; // the action needed
    public string objective; // the name of the noun the type of quest is acting on
    public int total;
    public bool isComplete;
    public int current; // current objective completion

    public QuestItem () {
        desc = "Exterminate 1 bat with the newly aquired power";
        type = "kill"; // (kill objectives), (collect objectives), (talk to objectives), (remove objectives)
        objective = "bat"; // name of enemy, item, or talkable character
        total = 1;
        current = 0;
        isComplete = false;
    }

    //the "events" to run after every of these t types of interactions.
    public void updateCompletion(Quest q, string t, string o) {
        Debug.Log("testing whether this is applicable kill");
        if (!type.Equals(t) || !objective.Equals(o)) return; // make sure the current quest item is this type anyway.
        current++;
        Debug.Log($"Item quota for Quest ({q.title}): {current} / {total} : by {t}ing a {o}");
        if (current >= total) { // quest item quota met.
            isComplete = true;
            q.updateCompletion();
            DialogueManager.startNewDialogue(q.getNextDialoguePath()); // try to play the next dialogue for this quest if there is one
        }
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
        Debug.Log(rewardType + " being claimed");
        switch (rewardType) {
            case "xp":
                Knowledge.player.addXp(ammount);
                break;
            case "quest":
                Knowledge.player.inventory.addAvailableQuest(rewardName);
                break;
            case "item":
                Knowledge.player.inventory.addItems(rewardName, ammount);
                break;
            case "equip":
                Knowledge.player.inventory.addEquips(rewardName, ammount);
                break;
            case "remove":
                Knowledge.player.inventory.loseItems(rewardName, ammount);
                break;
            case "skill":
                Knowledge.player.inventory.learnSkill(rewardName);
                break;
        }
        
    }
}