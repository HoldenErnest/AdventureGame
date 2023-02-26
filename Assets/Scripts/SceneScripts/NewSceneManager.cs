// Holden Ernest - 2/24/2023
// Manage what happens on scene startup

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSceneManager : MonoBehaviour {

    public GameObject genericCharacter;

    void Start() {
        Knowledge.inventory = new Inventory();
        loadCharacters();
    }

    public void loadCharacters() {
        CharacterCreator a = Knowledge.getCharBlueprint("genericCharacter");
        a.createCharacterFrom(genericCharacter);
        //a.team = 1;
        //a.createCharacterFrom(genericCharacter);
    }
}
