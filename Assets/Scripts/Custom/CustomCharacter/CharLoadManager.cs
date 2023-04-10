using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharLoadManager : MonoBehaviour
{
    public CharacterUI[] ui = new CharacterUI[3];
    private Character[] chars = new Character[3];

    void Start() {
        loadPlayers();
        setUI();
    }

    // load the players into the chars array
    private void loadPlayers() {
        for (int n = 0; n < 3; n++) {
            Knowledge.setSaveNumber(n);
            CharacterCreator playerBP = Knowledge.getPlayerSave(); // << playercharacter instead (get playerCharacter save path)
            chars[n] = playerBP.createCharacter().GetComponent<Character>();
            chars[n].gameObject.SetActive(false);
        }
    }

    private void setUI() {
        for (int i = 0; i < 3; i++) {
            ui[i].setCharacter(chars[i]);
        }
    }
}
