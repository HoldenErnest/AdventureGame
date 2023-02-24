// Holden Ernest - 2/23/2023
// A script to parse JSON files in to create a new character.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterCreator : MonoBehaviour {

    public int id; // specified to tell unique characters
    public string name; // character name
    public int baseHealth; // base max health
    public Stats stats; // const, str, dex, int, evade, armor, spd, xp, poiRst, psyRst
    public int team; // character team
    public int[] homePos; // home position (depending, can be used to spawn here)

    public string[] equipment; // equipment to equip
    public string[] items; // (extra items or materials to drop on death)
    public string[] startingSkills; // currently/last equipped skills in hotbar
    public string bodyTexture; // Resources/Textures/Bodies/<bodyTexture>.png
    public string icon; // a character icon for dialogues (might not be seen ever so you this is optional)

    // IMPORTANT!!!!!!!!!
    // if anything is not specified make sure it has a default value so it wont break

    public GameObject createCharacterFrom(GameObject characterBase) { // Create a character from the base character GameObject / model
        GameObject character = Instantiate(characterBase);
        Character c = character.GetComponent<Character>();
        if (c.isPlayer()) {
            AIController control = character.GetComponent<AIController>();
            if (homePos == null || homePos.Length >= 2) { // set home position
                control.homePosition = new Vector2(homePos[0], homePos[1]);
            } else control.homePosition = new Vector2(0,0);
        }
        Team cTeam = character.GetComponent<Team>();

        c.setName(name);
        c.setBaseHp(baseHealth);
        c.setStats(stats);
        cTeam.setTeam(team);
        c.setEquips(equipment);
        c.setItems(items);
        c.setStartingSkills(startingSkills);
        c.setBodyTex(bodyTexture);
        c.setCharIcon(icon);

        return character;
    }
}
