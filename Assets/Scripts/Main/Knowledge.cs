// Holden Ernest - converting strings to objects from xml objects
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D.Animation;
using System.IO;
using System;

//A class strictly for remembering the combinations for skills, Effects, ect.
// Refrence this class when equipping any skill to a character
public static class Knowledge {
    
    public static Character player;
    public static Tools tools;

    // Any files from Assets/Resources/{path} are only used for new instance files
    // new files are saved to Application.persistentDataPath/{path}
    private static string savesPath = $"{Application.persistentDataPath}/Saves/save0/";
    public static readonly string playerPath = "Player/";
    public static readonly string skillsPath = "SavedObjects/Skills/";
    public static readonly string charsPath = "SavedObjects/Characters/";
    public static readonly string effectsPath = "SavedObjects/Effects/";
    public static readonly string itemsPath = "SavedObjects/Items/";
    public static readonly string equipsPath = "SavedObjects/Equips/";
    public static readonly string textPath = "Texts/";
    public static readonly string charactersPath = "SavedObjects/Characters/";
    public static readonly string equipsTexturePath = "Textures/Equips/";
    public static readonly string bodyTexturePath = "Textures/Bodies/";
    public static readonly string effectsIconPath = "Icons/Effects/";
    public static readonly string itemsIconPath = "Icons/Items/";
    public static readonly string skillsIconPath = "Icons/Skills/";
    public static readonly string charsIconPath = "Icons/Characters/";
    public static readonly string skillsPrefabPath = "Prefabs/Skills/";
    public static readonly string charsPrefabPath = "Prefabs/Characters/";
    public static readonly string questsPath = "Quests/";
    public static readonly string tilesPath = "Tiles/";

    private static int currentSave = 0;

    //any __ToJson method will take an object and print it in json format
    public static void skillToJson(Skill s) {
        string json = JsonUtility.ToJson(s);
        Debug.Log(json);
    }
    public static void questToJson(Quest s) {
        string json = JsonUtility.ToJson(s);
        Debug.Log(json);
    }
    public static void effectToJson(Effect e) {
        string json = JsonUtility.ToJson(e);
        Debug.Log(json);
    }
    public static void equipToJson(Equipable e) {
        string json = JsonUtility.ToJson(e);
        Debug.Log(json);
    }
    public static void itemToJson(Item i) {
        string json = JsonUtility.ToJson(i);
        Debug.Log(json);
    }
    public static string characterToJson(CharacterCreator c) {
        string json = JsonUtility.ToJson(c);
        return json;
    }
    public static string inventoryToJson(Inventory i) {
        string json = JsonUtility.ToJson(i);
        return json;//Debug.Log(json);
    }

    // all get___ methods take a file based on a specified path and name and generates it as an object of that type
    public static Inventory getInvSave() {
        Inventory newInv = new Inventory();
        string theFile = playerPath + "inventory";
        try {
            if (File.Exists(savesPath + theFile + ".json")) { // see if it exists within saved files. if not load the default one
                JsonUtility.FromJsonOverwrite(File.ReadAllText(savesPath + theFile + ".json"), newInv);
            } else {
                string json = Resources.Load<TextAsset>(theFile).text;
                JsonUtility.FromJsonOverwrite(json, newInv);
            }
        } catch (Exception e){
            Debug.Log("Save \"inventory.json\" not found." + e);
        }
        return newInv;
    }
    public static CharacterCreator getPlayerSave() {
        CharacterCreator cc = new CharacterCreator();
        string theFile = playerPath + "player";
        try {
            if (File.Exists(savesPath + theFile + ".json")) { // see if it exists within saved files. if not load the default one
                JsonUtility.FromJsonOverwrite(File.ReadAllText(savesPath + theFile + ".json"), cc);
            } else {
                string json = Resources.Load<TextAsset>(theFile).text;
                JsonUtility.FromJsonOverwrite(json, cc);
            }
        } catch (Exception e){
            Debug.Log("Save \"player.json\" not found." + e);
        }
        return cc;
    }
    public static Skill getSkill(string skillName) {
        Skill newSkill = new Skill();
        string theFile = skillsPath + skillName;
        try {
            if (File.Exists(savesPath + theFile + ".json")) { // see if it exists within saved files. if not load the default one
                JsonUtility.FromJsonOverwrite(File.ReadAllText(savesPath + theFile + ".json"), newSkill);
            } else {
                string json = Resources.Load<TextAsset>(theFile).text;
                JsonUtility.FromJsonOverwrite(json, newSkill); // instead of rewriting a new Skill() try rewriting the skill currently in use
            }
        } catch {
            Debug.Log("Skill \"" + skillName + ".json\" not found.");
        }
        newSkill.setPath(theFile);
        return newSkill;
    }
    public static Quest getQuest(string questName) {
        Quest newQuest = new Quest();
        string theFile = questsPath + questName + "/quest";
        try {
            if (File.Exists(savesPath + theFile + ".json")) { // see if it exists within saved files. if not load the default one
                JsonUtility.FromJsonOverwrite(File.ReadAllText(savesPath + theFile + ".json"), newQuest);
            } else {
                string json = Resources.Load<TextAsset>(theFile).text;
                JsonUtility.FromJsonOverwrite(json, newQuest); // instead of rewriting a new Skill() try rewriting the skill currently in use
            }
        } catch {
            Debug.Log("Quest \"" + questName + "/quest.json\" not found.");
        }
        newQuest.setFile(questName);
        return newQuest;
    }
    public static Effect getEffect(string effectName, int effectTier) {
        Effect newEffect = new Effect();
        string theFile = effectsPath + effectName + effectTier;
        try {
            if (File.Exists(savesPath + theFile + ".json")) { // see if it exists within saved files. if not load the default one
                JsonUtility.FromJsonOverwrite(File.ReadAllText(savesPath + theFile + ".json"), newEffect);
            } else {
                string json = Resources.Load<TextAsset>(theFile).text;
                JsonUtility.FromJsonOverwrite(json, newEffect); // instead of rewriting a new Skill() try rewriting the skill currently in use
            }
        } catch {
            Debug.Log("Effect \"" + effectName + effectTier + ".json\" not found.");
        }
        return newEffect;
    }
    public static Equipable getEquipable(string equipName) {
        Equipable newEquip = new Equipable();
        string theFile = equipsPath + equipName;
        try {
            string json = Resources.Load<TextAsset>(theFile).text;
            JsonUtility.FromJsonOverwrite(json, newEquip); // instead of rewriting a new Skill() try rewriting the skill currently in use
        } catch {
            Debug.Log("Equipable \"" + equipName + ".json\" not found.");
        }
        newEquip.setPath(theFile);
        return newEquip;
    }
    public static Item getItem(string itemName) {
        Item newItem = new Item();
        string theFile = itemsPath + itemName;
        try {
            string json = Resources.Load<TextAsset>(theFile).text;
            JsonUtility.FromJsonOverwrite(json, newItem); // instead of rewriting a new Skill() try rewriting the skill currently in use
        } catch {
            Debug.Log("Item \"" + itemName + ".json\" not found.");
        }
        newItem.setPath(theFile);
        return newItem;
    }
    // Returns a generic list of strings with methods
    public static TextList getTextList(string textName) {
        TextList list = new TextList();
        try {
            string json = Resources.Load<TextAsset>(textPath + textName).text;
            JsonUtility.FromJsonOverwrite(json, list);
        } catch {
            Debug.Log("Text \"" + textName + ".json\" not found.");
        }
        return list;
    }
    // Returns the blueprint for a new character, call createCharacterFrom() to make a new character.
    public static CharacterCreator getCharBlueprint(string charName) {
        CharacterCreator character = new CharacterCreator();
        string theFile = charactersPath + charName;
        try {
            if (File.Exists(savesPath + theFile + ".json")) { // see if it exists within saved files. if not load the default one
                JsonUtility.FromJsonOverwrite(File.ReadAllText(savesPath + theFile + ".json"), character);
            } else {
                string json = Resources.Load<TextAsset>(theFile).text;
                JsonUtility.FromJsonOverwrite(json, character); // instead of rewriting a new Skill() try rewriting the skill currently in use
            }
        } catch {
            Debug.Log("Character \"" + charName + ".json\" not found.");
        }
        character.setPath(charName);
        return character;
    }
    public static CharacterCreator getCharBlueprint(int charId) {
        return getCharBlueprint(""+charId);
    }
    public static CharacterCreator[] getAllCharBP() {
        CharacterCreator[] cca = new CharacterCreator[5];
        return cca;
    }
    public static Texture2D getEquipTexture(string s) {
        try {
            return Resources.Load<Texture2D>(equipsTexturePath + s);
        } catch {
            Debug.Log("Equip texture \"" + s + ".png\" not found.");
        }
        return null;
    }
    public static Texture2D getBodyTexture(string s) {
        try {
            return Resources.Load<Texture2D>(bodyTexturePath + s);
        } catch {
            Debug.Log("Body texture \"" + s + ".png\" not found.");
        }
        return null;
    }
    public static Sprite getCharIcon(string s) {
        if (s != "")
        try {
            return Resources.Load<Sprite>(charsIconPath + s);
        } catch {
            Debug.Log("Character icon \"" + s + ".png\" not found.");
        }
        return Resources.Load<Sprite>(charsIconPath + "noicon");
    }
    public static Sprite getCharIcon(int i) {
        return getAllCharSprites()[i];
    }
    public static Texture2D getSkillIcon(string s) {
        if (s != "")
        try {
            return Resources.Load<Texture2D>(skillsIconPath + s);
        } catch {
            Debug.Log("Skill icon \"" + s + ".png\" not found.");
        }
        return Resources.Load<Texture2D>(skillsIconPath + "noSkill");
    }
    public static Sprite[] getAllSpritesFromTexture(string s) {
        try {
            //Texture2D theImg = Resources.Load<Texture2D>(tilesPath + s);
            return Resources.LoadAll<Sprite>(tilesPath + s);
        } catch {
            Debug.Log("Multi-Texture \"" + s + ".png\" not found.");
        }
        return null;
    }
    public static Sprite[] getAllCharSprites() {
        try {
            return Resources.LoadAll<Sprite>(charsIconPath + "allCharacters");
        } catch {
            Debug.Log("Multi-Texture \"" + ".png\" not found.");
        }
        return null;
    }
    public static GameObject[] getAllObjectsInFolder(string path) {
        try {
            return Resources.LoadAll<GameObject>(charsPath);
        } catch {
            Debug.Log("Folder at \"" + path + "\" does not contain any characters.");
        }
        return null;
    }
    public static CharacterCreator[] getAllCharacters() {
        CharacterCreator[] allChars = null;
        try {
            TextAsset[] fileArray = Resources.LoadAll<TextAsset>(charsPath);
            allChars = new CharacterCreator[fileArray.Length];
            for (int i = 0; i < fileArray.Length; i++) {
                string json = fileArray[i].text;
                CharacterCreator cc = new CharacterCreator();
                JsonUtility.FromJsonOverwrite(json, cc);
                allChars[i] = cc;
            }
            return allChars;
        } catch {
            Debug.Log("Folder at \"" + charsPath + "\" does not contain any characters.");
        }
        return allChars;
    }
    public static Texture2D getItemIcon(string s) { // Equips and Items both use this for Icons only
        Texture2D temp = Resources.Load<Texture2D>(itemsIconPath + s);
        if (temp != null) {
            return temp;
        } else {
            Debug.Log("Item icon \"" + s + ".png\" not found.");
        }
        return Resources.Load<Texture2D>(itemsIconPath + "no_texture");
    }

    public static GameObject getSkillPrefab(string s) {
        try {
            return Resources.Load<GameObject>(skillsPrefabPath + s);
        } catch {
            Debug.Log("Gameobject \"" + s + ".prefab\" not found.");
        }
        return null;
    }
    public static GameObject getCharacterPrefab(string s) {
        try {
            return Resources.Load<GameObject>(charsPrefabPath + s);
        } catch {
            Debug.Log("Gameobject \"" + s + ".prefab\" not found.");
        }
        return null;
    }
    public static GameObject getHpUI() {
        try {
            return Resources.Load<GameObject>("Prefabs/UI/HealthbarUI");
        } catch {
            Debug.Log("Gameobject \"Prefabs/UI/HealthbarUI.prefab\" not found.");
        }
        return null;
        
    }


    // Functions to change Knowledge variables
    public static void setSaveNumber (int saveNum) {
        currentSave = saveNum;
        savesPath = $"{Application.persistentDataPath}/Saves/save{saveNum}/";
    }
    public static string getSavePath () {
        return savesPath;
    }

}
