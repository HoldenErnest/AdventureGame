// Holden Ernest - 10/23/2022
// Manage save / load system + game exit ect.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour{

    public void exitGame() {
        //saveGame();
        Application.Quit();
    }
    public void saveGame() {
        GameSaver.overwritePlayer();
        GameSaver.overwriteNPCS();
        //GameSaver.overwriteSomething();
        // create more than one save? be able to load from operable saves?
    }
    public void loadGame() { // load one of the 3 saves!

    }
    public void openSettings() {

    }

}
