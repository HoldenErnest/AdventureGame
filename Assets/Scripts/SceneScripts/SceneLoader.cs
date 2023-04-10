// Holden Ernest 3/17/2023
// Load entire scene from main menu, including NPC home positions and whatnot. (parse a save file and load that as a scene)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public string saveFile;

    private readonly string savesPath = "Resources/saves/";

    void Start() {

    }

    public void loadGame() {
        SceneManager.LoadScene("GameScene");
    }
    public void loadSceneLoad() {
        SceneManager.LoadScene("LoadScene");
    }
    public void newScene() {
        
    }

    public void setGame0() {
        Knowledge.setSaveNumber(0);
        loadGame();
    }
    public void setGame1() {
        Knowledge.setSaveNumber(1);
        loadGame();
    }
    public void setGame2() {
        Knowledge.setSaveNumber(2);
        loadGame();
    }
}
