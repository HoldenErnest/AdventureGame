// Holden Ernest - 5/17/2023
// A component to handle talking to characters and objects -- more specifically send a request to DialogueManager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Speaker : MonoBehaviour, IPointerClickHandler {
    
    private string[] quests;

    public void setQuestsToGive(string[] q) {
        quests = q;
    }

    public void talk() { // interact with this speaker character.
        if (DialogueManager.isActive()) return;
        string aq = getAvailableQuest();
        if (aq != "") startQuest(aq);
        else startRandomDialogue();
    }

    private void startQuest(string q) {
        Quest startingQuest = Knowledge.getQuest(q);
        DialogueManager.startNewDialogue(startingQuest.getNextDialoguePath());
        Knowledge.player.inventory.addQuest(startingQuest);
    }

    private void startRandomDialogue() {
        DialogueManager.startNewDialogue("Dialogue/greeting");
    }

    private string getAvailableQuest() { // if the speaker can give a quest, emmit that quests starting dialogue instead
        foreach (string s in quests) {
            foreach(string s2 in Knowledge.player.inventory.availableQuests) { // assuming these quests are sorted by priority
                if (s.Equals(s2)) return s;
            }
        }
        return "";
    }
    // onclicks
    public void OnPointerClick (PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            talk();
        }
    }
}
