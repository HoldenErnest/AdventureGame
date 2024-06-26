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
        Quest startingQuest = Knowledge.player.inventory.addQuest(q);
        DialogueManager.startNewDialogue(startingQuest.getNextDialoguePath());
    }

    private void startRandomDialogue() {
        string allText = Knowledge.getTextList("greeting").getRandom();
        DialogueManager.startNewDialogue(gameObject.GetComponent<Character>().name,allText);// THIS WONT WORK WITH TALKING TO OBJECTS!!!!!!
    }

    private string getAvailableQuest() { // if the speaker can give a quest, emmit that quests starting dialogue instead
        if (quests == null) return "";
        foreach (string s in quests) {
            for (int i = 0; i < Knowledge.player.inventory.availableQuests.Length; i++) { // assuming these quests are sorted by priority
                if (s.Equals(Knowledge.player.inventory.availableQuests[i])) {
                    return s;
                }
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
