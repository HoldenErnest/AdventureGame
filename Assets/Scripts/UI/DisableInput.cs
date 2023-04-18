using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInput : MonoBehaviour {
    // for the text input box
    public void editingText() {
        Debug.Log("disable input");
        Knowledge.player.GetController().setEditingText(true);
    }
    public void stopEditingText() {
        Debug.Log("enable input");
        Knowledge.player.GetController().setEditingText(false);
    }
}
