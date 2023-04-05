// Holden Ernest - 2/24/2023
// Manage what happens on scene startup

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSceneManager : MonoBehaviour {

    public GameObject genericCharacter;
    public GameObject playerCharacter;
    public GameObject statsUI; // empty gameobject encompasing the personal stats overlay (health and xp bars ect) FOR PLAYER ONLY!!!!!
    public GameObject escUI;
    public GameObject invUI;
    public Hotbar[] hotbars;

    void Start() {
        loadInit();
        loadPlayer();
        loadCharacters();
    }
    private void loadInit() {
        Knowledge.setSaveNumber(0);
    }
    public void loadCharacters() {
        CharacterCreator a = Knowledge.getCharBlueprint("genericCharacter");
        a.createCharacter();
        a.team = 1;
    }
    private void loadPlayer() {
        int n = gameObject.GetComponent<GameManagement>().getCurrentSaveIndex();
        
        CharacterCreator playerBP = Knowledge.getPlayerSave(); // << playercharacter instead (get playerCharacter save path)
        Knowledge.player = playerBP.createPlayerFrom(playerCharacter, escUI, invUI, statsUI, hotbars).GetComponent<Character>();
        gameObject.GetComponent<CameraFollow>().playerPos = Knowledge.player.gameObject.transform;
        Knowledge.tools = gameObject.GetComponent<Tools>();
        
        PlayerLoader.loadInventory();
    }
}
