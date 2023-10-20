// Holden Ernest - converting strings to objects from xml objects
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D.Animation;
using System.IO;
using System;
using System.Diagnostics;
using UnityEngine.UI;
using Unity.VisualScripting;

//A class strictly for remembering the combinations for skills, Effects, ect.
// Refrence this class when equipping any skill to a character
public static class Knowledge {
    
    public static Character player;
    public static Tools tools;
    public static FolderStructure folderStruct;

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
        UnityEngine.Debug.Log(json);
    }
    public static void questToJson(Quest s) {
        string json = JsonUtility.ToJson(s);
        UnityEngine.Debug.Log(json);
    }
    public static string specialObjectToJson(SpecialObject s) {
        return s.toJson();
    }
    public static void effectToJson(Effect e) {
        string json = JsonUtility.ToJson(e);
        UnityEngine.Debug.Log(json);
    }
    public static void equipToJson(Equipable e) {
        string json = JsonUtility.ToJson(e);
        UnityEngine.Debug.Log(json);
    }
    public static void itemToJson(Item i) {
        string json = JsonUtility.ToJson(i);
        UnityEngine.Debug.Log(json);
    }
    public static string characterToJson(CharacterCreator c) {
        string json = JsonUtility.ToJson(c);
        return json;
    }
    public static string inventoryToJson(Inventory i) {
        string json = JsonUtility.ToJson(i);
        return json;//UnityEngine.Debug.Log(json);
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
            UnityEngine.Debug.Log("Save \"inventory.json\" not found." + e);
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
            UnityEngine.Debug.Log("Save \"player.json\" not found." + e);
        }
        return cc;
    }
    public static Skill getSkill(string skillName) {
        Skill newSkill = new Skill();
        string theFile = skillsPath + skillName;
        if (skillName != null && skillName != "")
        try {
            if (File.Exists(savesPath + theFile + ".json")) { // see if it exists within saved files. if not load the default one
                JsonUtility.FromJsonOverwrite(File.ReadAllText(savesPath + theFile + ".json"), newSkill);
            } else {
                string json = Resources.Load<TextAsset>(theFile).text;
                JsonUtility.FromJsonOverwrite(json, newSkill); // instead of rewriting a new Skill() try rewriting the skill currently in use
            }
        } catch {
            UnityEngine.Debug.Log("Skill \"" + skillName + ".json\" not found.");
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
            UnityEngine.Debug.Log("Quest \"" + questName + "/quest.json\" not found.");
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
            UnityEngine.Debug.Log("Effect \"" + effectName + effectTier + ".json\" not found.");
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
            UnityEngine.Debug.Log("Equipable \"" + equipName + ".json\" not found.");
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
            UnityEngine.Debug.Log("Item \"" + itemName + ".json\" not found.");
        }
        newItem.setPath(theFile);
        return newItem;
    }
    public static Item[] getAllItems() {
        Item[] allItems = null;
        try {
            // resources.loadall doesnt work directly to json files so we have to
            // get all filenames then overwrite each file
            TextAsset[] fileArray = Resources.LoadAll<TextAsset>(itemsPath);
            allItems = new Item[fileArray.Length];
            for (int i = 0; i < fileArray.Length; i++) {
                string json = fileArray[i].text;
                Item itm = new Item();
                JsonUtility.FromJsonOverwrite(json, itm);
                allItems[i] = itm;
            }
        } catch {
            UnityEngine.Debug.Log("Folder at \"" + itemsPath + "\" does not contain any Items.");
        }
        return allItems;
    }
    public static Equipable[] getAllEquips() {
        Equipable[] allEquips = null;
        try {
            // resources.loadall doesnt work directly to json files so we have to
            // get all filenames then overwrite each file
            TextAsset[] fileArray = Resources.LoadAll<TextAsset>(equipsPath);
            allEquips = new Equipable[fileArray.Length];
            for (int i = 0; i < fileArray.Length; i++) {
                string json = fileArray[i].text;
                Equipable eq = new Equipable();
                JsonUtility.FromJsonOverwrite(json, eq);
                allEquips[i] = eq;
            }
        } catch {
            UnityEngine.Debug.Log("Folder at \"" + equipsPath + "\" does not contain any Equips.");
        }
        return allEquips;
    }
    public static Skill[] getAllSkills() {
        Skill[] allSkills = null;
        try {
            // resources.loadall doesnt work directly to json files so we have to
            // get all filenames then overwrite each file
            TextAsset[] fileArray = Resources.LoadAll<TextAsset>(skillsPath);
            allSkills = new Skill[fileArray.Length];
            for (int i = 0; i < fileArray.Length; i++) {
                string json = fileArray[i].text;
                Skill skl = new Skill();
                JsonUtility.FromJsonOverwrite(json, skl);
                allSkills[i] = skl;
            }
        } catch {
            UnityEngine.Debug.Log("Folder at \"" + skillsPath + "\" does not contain any Skills.");
        }
        return allSkills;
    }
    public static string[] getAllQuestFolderNames() {
        return folderStruct.allQuests;
    }
    // Returns a generic list of strings with methods
    public static TextList getTextList(string textName) {
        TextList list = new TextList();
        try {
            string json = Resources.Load<TextAsset>(textPath + textName).text;
            JsonUtility.FromJsonOverwrite(json, list);
        } catch {
            UnityEngine.Debug.Log("Text \"" + textName + ".json\" not found.");
        }
        return list;
    }
    public static TextAsset getMapSave(string file) {
        TextAsset theText = null;
        try {
            if (File.Exists(savesPath + file + ".txt")) { // see if it exists within saved files. if not load the default one
                theText = new TextAsset(File.ReadAllText(savesPath + file + ".txt"));
            } else {
                string theString = Resources.Load<TextAsset>(file).text;
                theText = new TextAsset(theString);
            }
        } catch(Exception e) {
            UnityEngine.Debug.Log("Map location \"" + file + ".txt\" not found. ERROR: " + e);
        }
        return theText;
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
            UnityEngine.Debug.Log("Character \"" + charName + ".json\" not found.");
        }
        character.setPath(charName);
        return character;
    }
    public static CharacterCreator getCharBlueprint(int charId) {
        return getCharBlueprint(""+charId);
    }
    public static Texture2D getEquipTexture(string s) {
        try {
            return Resources.Load<Texture2D>(equipsTexturePath + s);
        } catch {
            UnityEngine.Debug.Log("Equip texture \"" + s + ".png\" not found.");
        }
        return null;
    }
    public static Texture2D getBodyTexture(string s) {
        try {
            return Resources.Load<Texture2D>(bodyTexturePath + s);
        } catch {
            UnityEngine.Debug.Log("Body texture \"" + s + ".png\" not found.");
        }
        return null;
    }
    public static Sprite getCharIcon(string s) { // DEPRECIATED
        if (s != "")
        try {
            return Resources.Load<Sprite>(charsIconPath + s);
        } catch {
            UnityEngine.Debug.Log("Character icon \"" + s + ".png\" not found.");
        }
        return getNoCharIcon();
    }
    public static Sprite getNoCharIcon() {
        return Resources.Load<Sprite>(charsIconPath + "noicon");
    }
    public static Sprite getCharIcon(int i) {
        Sprite[] theSprites = getAllCharSprites();
        if (i < 0 || i >= theSprites.Length) return getNoCharIcon();
        return theSprites[i];
    }
    public static Texture2D getSkillIcon(string s) {
        if (s != "")
        try {
            return Resources.Load<Texture2D>(skillsIconPath + s);
        } catch {
            UnityEngine.Debug.Log("Skill icon \"" + s + ".png\" not found.");
        }
        return Resources.Load<Texture2D>(skillsIconPath + "noSkill");
    }
    public static Sprite[] getAllSpritesFromTexture(string s) {
        try {
            //Texture2D theImg = Resources.Load<Texture2D>(tilesPath + s);
            return Resources.LoadAll<Sprite>(tilesPath + s);
        } catch {
            UnityEngine.Debug.Log("Multi-Texture \"" + s + ".png\" not found.");
        }
        return null;
    }
    public static Sprite[] getAllCharSprites() {
        try {
            return Resources.LoadAll<Sprite>(charsIconPath + "allCharacters");
        } catch {
            UnityEngine.Debug.Log("Multi-Texture \"" + ".png\" not found.");
        }
        return null;
    }
    public static string[] getAllBodyTextureNames() { // load any gameOBjects in a certain folder
        string[] theNames = {};
        try {
            Texture2D[] tex = Resources.LoadAll<Texture2D>(bodyTexturePath);
            theNames = new string[tex.Length];
            for (int i = 0; i < tex.Length; i++) {
                theNames[i] = tex[i].name;
            }
        } catch {
            UnityEngine.Debug.Log("Folder at \"" + bodyTexturePath + "\" does not contain any body textures.");
        }
        return theNames;
    }
    public static GameObject[] getAllObjectsInFolder(string path) { // load any gameOBjects in a certain folder
        try {
            return Resources.LoadAll<GameObject>(path);
        } catch {
            UnityEngine.Debug.Log("Folder at \"" + path + "\" does not contain any objects.");
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
            UnityEngine.Debug.Log("Folder at \"" + charsPath + "\" does not contain any characters.");
        }
        return allChars;
    }
    public static Texture2D getItemIcon(string s) { // Equips and Items both use this for Icons only
        Texture2D temp = Resources.Load<Texture2D>(itemsIconPath + s);
        if (temp != null) {
            return temp;
        } else {
            UnityEngine.Debug.Log("Item icon \"" + s + ".png\" not found.");
        }
        return Resources.Load<Texture2D>(itemsIconPath + "no_texture");
    }

    public static GameObject getObject(string fileName) { // load any gameOBjects in a certain folder
        try {
            return Resources.Load<GameObject>("SavedObjects/Objects/" + fileName);
        } catch {
            UnityEngine.Debug.Log("At \"" + fileName + "\" no object.");
        }
        return null;
    }
    public static GameObject getSkillPrefab(string s) {
        try {
            return Resources.Load<GameObject>(skillsPrefabPath + s);
        } catch {
            UnityEngine.Debug.Log("Gameobject \"" + s + ".prefab\" not found.");
        }
        return null;
    }
    public static GameObject getCharacterPrefab(string s) {
        try {
            return Resources.Load<GameObject>(charsPrefabPath + s);
        } catch {
            UnityEngine.Debug.Log("Gameobject \"" + s + ".prefab\" not found.");
        }
        return null;
    }
    public static GameObject getHpUI() {
        try {
            return Resources.Load<GameObject>("Prefabs/UI/HealthbarUI");
        } catch {
            UnityEngine.Debug.Log("Gameobject \"Prefabs/UI/HealthbarUI.prefab\" not found.");
        }
        return null;
        
    }

    public static void loadFolderStruct() {
        folderStruct = new FolderStructure();
        #if UNITY_EDITOR
        folderStruct.save(); // if its the unity editor go into the folder system and update/save all the paths to this structure object
        #endif
        if (File.Exists(Application.persistentDataPath + "/folderStructure.json")) { // see if it exists within saved files. if not load the default one
            JsonUtility.FromJsonOverwrite(File.ReadAllText(Application.persistentDataPath + "/folderStructure.json"), folderStruct);
            UnityEngine.Debug.Log(Application.persistentDataPath + "/folderStructure.json loaded");
        } else {
            UnityEngine.Debug.Log(Application.persistentDataPath + "/folderStructure.json does not exist");
        }
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
