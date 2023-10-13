// Holden Ernest - 2/24/2023
// Manage what happens on scene startup

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneManager : MonoBehaviour {

    public GameObject genericCharacter;
    public GameObject playerCharacter;
    public GameObject statsUI; // empty gameobject encompasing the personal stats overlay (health and xp bars ect) FOR PLAYER ONLY!!!!!
    public GameObject escUI;
    public GameObject invUI;
    public Hotbar[] hotbars;
    public string currentMap;

    [SerializeField]
    private MapToSave[] grids;

    void Start() {
        loadInit();
        loadPlayer();
        loadCharacters(); // LOAD ALL NPCS
        loadMap();
    }
    private void loadInit() {
        Knowledge.tools = gameObject.GetComponent<Tools>();
    }
    public void loadCharacters() {
        if (SceneManager.GetActiveScene().name.Equals("EditorScene")) return;
        CharacterCreator a = Knowledge.getCharBlueprint("2");
        CharacterCreator b = Knowledge.getCharBlueprint("0");
        //GameSaver.npcs.Add(a.createCharacter());
        a.team = 1;
        //GameSaver.npcs.Add(a.createCharacter());
    }
    private void loadPlayer() {
        
        CharacterCreator playerBP = Knowledge.getPlayerSave(); // << playercharacter instead (get playerCharacter save path)
        Knowledge.player = playerBP.createPlayerFrom(playerCharacter, escUI, invUI, statsUI, hotbars).GetComponent<Character>();
        gameObject.GetComponent<CameraFollow>().playerPos = Knowledge.player.gameObject.transform;
        gameObject.transform.position = new Vector3(Knowledge.player.gameObject.transform.position.x,Knowledge.player.gameObject.transform.position.y, -10);
        
        
        GameSaver.loadInventory();
    }
    private void loadMap () {
        foreach (MapToSave m in grids) {
            m.loadMap("test");
        }
    }
}
