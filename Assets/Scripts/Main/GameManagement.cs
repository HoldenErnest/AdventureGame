// Holden Ernest - 10/23/2022
// Manage save / load system + game exit ect.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour{

    private int currentSave = 0; // CHANGE WITH EACH SAVE

    public void exitGame() {
        //saveGame();
        Application.Quit();
    }
    public void saveGame() {
        PlayerLoader.overwrite();
        // create more than one save? be able to load from operable saves?
    }
    public void loadGame() { // load one of the 3 saves!
        PlayerLoader.loadInventory();
    }
    public void openSettings() {
        loadGame(); // TEMP REPLACEMENT
    }
    public int getCurrentSaveIndex() {
        return currentSave;
    }
    public void setCurrentSave(int n) {
        currentSave = n;
    }

}
