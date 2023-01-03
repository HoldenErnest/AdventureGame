using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUI : InventoryCell {

    private Quest quest;
    public TextMeshProUGUI descUI;

    public void setOffsets() {
        cellOffsetX = 0f;
        cellOffsetY = 0.2f;
        tableOffsetX = -5.5f;
        tableOffsetY = 2.3f;
        itemsPerRow = 1;
    }

    public void setQuest(Quest q) {
        quest = q;
        updateText();
    }

    private void updateText() {
        textUI.text = quest.title;
        descUI.text = quest.desc;
    }

    public Quest getQuest() {
        return quest;
    }
}
