// Holden Ernest - 10/21/2023
// Keep track of a single stat on a bunch of UI, controlled by a main StatUI

using System.Data;
using TMPro;
using UnityEngine;

public class SingleStatUI : MonoBehaviour {

    public int statIndex = -1;
    public StatUIController control;

    public TMP_Text theNumber;
    
    public void increaseStat() {
        control.increaseStat(statIndex);
    }
    public void decreaseStat() {
        control.decreaseStat(statIndex);
    }

    void FixedUpdate() {
        updateStatUI();
    }

    public void updateStatUI() {
        ref int n = ref control.getStat(statIndex);
        theNumber.text = n + "";
    }

}
