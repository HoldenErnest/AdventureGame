using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUI : InventoryCell {

    private Quest quest;
    public TextMeshProUGUI levelUI;
    public GameObject star; // toggle on and off depending on main quest

    public void setOffsets() {
        cellOffsetX = 0f;
        cellOffsetY = 0.2f;
        tableOffsetX = 1.95f;
        tableOffsetY = 2.2f;
        itemsPerRow = 1;
    }

    public void setQuest(Quest q) {
        quest = q;
        updateInfo();
    }

    private void updateInfo() {
        textUI.text = quest.title;
        levelUI.text = $"({quest.reccomendedLevel})";
        if (quest.isMainQuest) {
            star.SetActive(true);
        } else {
            star.SetActive(false);
        }
    }

    public Quest getQuest() {
        return quest;
    }
}
