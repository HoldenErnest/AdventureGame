using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class QuestSave {
    public string file; // file to the actual quest object. Something like: (Resources/Quests/) hunterQuest (.json)
    public int itemsDone = 0; // number of items completed for that quest
    public bool gotReward = false; // if the player already claimed the reward. (the quest is done!)


    public Quest toQuest() {
        Quest q = Knowledge.getQuest(file);
        completeQuests(q);
        q.rewardsCollected = gotReward;
        return q;
    }

    // complete the quest items
    private void completeQuests(Quest q) {
        if (itemsDone >= q.items.Length) {
            itemsDone = q.items.Length;
            q.isComplete = true;
        }

        for (int i = 0; i < itemsDone; i++) {
            q.items[i].isComplete = true;
        }
    }

}
