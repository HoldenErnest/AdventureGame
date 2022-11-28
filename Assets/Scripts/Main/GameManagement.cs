//Holden Ernest - Manage save / load system + game exit ect.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour{

    public void exitGame() {
        //saveGame();
        Application.Quit();
    }
    public void saveGame() {
        // create more than one save? be able to load from operable saves?
    }
    public void openSettings() {

    }

}
