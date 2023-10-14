// Holden Ernest - 2/23/2023
// A script to parse JSON files in to create a new character.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterCreator : MonoBehaviour {

    private string path;
    public int id; // specified to tell unique characters
    new public string name; // character name
    public string title; // character name
    public string description;
    public bool important;
    public int baseHealth; // base max health
    public Stats stats; // const, str, dex, int, evade, armor, spd, xp, poiRst, psyRst
    public int team; // character team
    public float[] homePos; // home position (depending, can be used to spawn here)
    public string lastMap; // what map were you previously in (usually not changed or even refrenced)(keep "" to spawn with original map)

    public string[] equipment; // equipment to equip
    public ItemSave[] items; // (extra items or materials to drop on death)
    public string[] startingSkills; // currently/last equipped skills in hotbar
    public string bodyTexture; // Resources/Textures/Bodies/<bodyTexture>.png
    public string icon; // a character icon for dialogues (might not be seen ever so you this is optional) LEGACY -- all icons are gotten from the id

    public string[] questsToGive; // a list of the quests they will give to the player on interaction

    // IMPORTANT!!!!!!!!!
    // if anything is not specified make sure it has a default value so it wont break
    public CharacterCreator() {
        
    }
    public Character createCharacter() { // Create a character
        return createCharacterFrom(Knowledge.getCharacterPrefab("genericCharacter"));
    }
    public Character createCharacterFrom(GameObject characterBase) { // Create a character from the base character GameObject / model
        GameObject character = Instantiate(characterBase);
        Character c = character.GetComponent<Character>();
        if (!c.isPlayer()) {
            AIController control = character.GetComponent<AIController>();
            if (homePos != null && homePos.Length == 2) { // set home position
                control.homePosition = new Vector2(homePos[0], homePos[1]);
                character.transform.position = new Vector3(homePos[0], homePos[1], 0);
            } else control.homePosition = new Vector2(0,0);
            Speaker sp = character.GetComponent<Speaker>();
            sp.setQuestsToGive(questsToGive);
        }
        Team cTeam = character.GetComponent<Team>();
        //Debug.Log(c.name + " is important: " + important);
        c.setImportant(important);
        c.setPath(path);
        c.setName(name);
        c.setTitle(title);
        c.setDescription(description);
        c.setBaseHp(baseHealth);
        c.setStats(stats);
        cTeam.setTeam(team);
        c.setBodyTex(bodyTexture);
        c.setEquips(equipment);
        c.setItems(items);  // << need a new inventory for this character
        c.setStartingSkills(startingSkills);
        c.setCharIcon(icon);

        c.updateAll();

        return c;
    }
    public GameObject createPlayerFrom(GameObject characterBase, GameObject escUI, GameObject invUI, GameObject statsUI, Hotbar[] hotbars) { // Create a PLAYER from the base character GameObject / model
        GameObject character = Instantiate(characterBase);
        Character c = character.GetComponent<Character>();
        Team cTeam = character.GetComponent<Team>();
        try {
            c.setName(name);
            c.setTitle(title);
            c.setDescription(description);
            c.setImportant(true);
            c.setBaseHp(baseHealth);
            c.setStats(stats);
            cTeam.setTeam(team);
            c.setBodyTex(bodyTexture);
            c.setEquips(equipment);
            c.setStartingSkills(startingSkills);
            if (hotbars != null)
                c.setHotbar(hotbars);
            c.setCharIcon(icon);
            Controller charCont = character.GetComponent<Controller>();
            charCont.invUI = invUI;
            charCont.escUI = escUI;
            charCont.gameObject.transform.position = new Vector2(homePos[0], homePos[1]); // sets the players transform to the home position
            c.setStatsUI(statsUI);
            c.setPlayer(); // setplayer inherintly does updateAll()
        } catch (Exception e) {
            Debug.Log("PLAYER CANT BE CREATED: " + e);
        }

        return character;
    }

    public void setRandomName(string file) {
        TextList names = Knowledge.getTextList(file ?? "npcNames");
        name = names.getRandom();
    }

    // Common to call before spawning in, will instantiate the character here
    public void setSpawn(Vector2 v) {
        homePos[0] = v.x;
        homePos[1] = v.y;
    }

    public void setLevel(int l, string[] attrs) { // not only picks level but the attributes to spec into (attribute points are spread evenly to everything in [attrs])
        stats.resetAllAttr();
        stats.setLevel(l);
        float multiplier = (1.0f / attrs.Length);
        foreach (string attr in attrs) {
            setAttribute(attr, multiplier);
        }
    }
    private void setAttribute(string type, float multiplier) { // not only picks level but attributes to spec into
        int points = (int)((stats.getLevel() * 2) * multiplier);
        switch (type) {
            case "con":
                stats.constitution = points;
                break;
            case "str":
                stats.strength = points;
                break;
            case "dex":
                stats.dexterity = points;
                break;
            case "int":
                stats.intelligence = points;
                break;
            case "evd":
                stats.evasion = points;
                break;
            case "spd":
                stats.speed = points;
                break;
        }
    }

    public string getPath() {
        return path;
    }
    public void setPath(string p) {
        path = p;
    }

    // in case youre just working with the creators, not actual characters
    public Sprite getIcon() {
        return Knowledge.getCharIcon(id);
    }
}
