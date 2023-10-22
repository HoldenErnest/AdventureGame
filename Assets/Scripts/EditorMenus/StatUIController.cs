// Holden Ernest - 10/21/2023
// Controls a bunch of SingleStatUI gameObjects

using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class StatUIController : MonoBehaviour {

    private Stats theStats;

    public TMP_InputField levelUI;
    public TMP_Text pointsText;

    void Start() {
        theStats = new Stats();
        levelUI.onValueChanged.AddListener(delegate {levelChange(levelUI.text); });
    }

    public void decreaseStat(int index) {
        ref int n = ref getStat(index);
        if (n > 0)
            n -= 1;
        updatePoints();
    }
    public void increaseStat(int index) {
        if (theStats.maxedStats()) return;
        ref int n = ref getStat(index);
        n += 1;
        updatePoints();
    }
    public ref int getStat(int index) {
        return ref theStats.getStatFromIndex(index);
    }
    public Stats getFullStats() {
        UnityEngine.Debug.Log(theStats + " are the stats");
        return theStats;
    }

    private void updatePoints() {
        pointsText.text = theStats.getTotalAttrUsed() + "/" + theStats.getTotalAttr();
    }

    public void levelChange(string takenString) { // Event - level UI changed
        if (takenString == "") {
            takenString = "1";
        }
        int newLevel = Int32.Parse(takenString);
        theStats.setLevel(newLevel);
        updatePoints();
    }

}
