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

    public static readonly string savesPath = "/Saves/";

    public static readonly string skillsPath = "SavedObjects/Skills/";
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
    public static readonly string skillsPrefabPath = "Prefabs/Skills/";
    public static readonly string charsPrefabPath = "Prefabs/Characters/";
    public static readonly string questsPath = "Quests/";

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
    public static Inventory getInvSave(string path) {
        Inventory newInv = new Inventory();
        try {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(Application.persistentDataPath + savesPath + path + ".json"), newInv);
        } catch (Exception e){
            Debug.Log("Save \"" + path + ".json\" not found." + e);
        }
        return newInv;
    }
    public static CharacterCreator getPlayerSave(string path) {
        CharacterCreator cc = new CharacterCreator();
        try {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(Application.persistentDataPath + savesPath + path + ".json"), cc);
        } catch (Exception e){
            Debug.Log("Save \"" + path + ".json\" not found." + e);
        }
        return cc;
    }
    public static Skill getSkill(string skillName) {
        Skill newSkill = new Skill();
        try {
            string json = Resources.Load<TextAsset>(skillsPath + skillName).text;
            JsonUtility.FromJsonOverwrite(json, newSkill); // instead of rewriting a new Skill() try rewriting the skill currently in use
        } catch {
            Debug.Log("Skill \"" + skillName + ".json\" not found.");
        }
        newSkill.setPath(skillsPath + skillName);
        return newSkill;
    }
    public static Quest getQuest(string questName) {
        Quest newQuest = new Quest();
        try {
            string json = Resources.Load<TextAsset>(questsPath + questName).text;
            JsonUtility.FromJsonOverwrite(json, newQuest);
        } catch {
            Debug.Log("Quest \"" + questName + ".json\" not found.");
        }
        newQuest.setPath(questsPath + questName);
        return newQuest;
    }
    public static Effect getEffect(string effectName, int effectTier) {
        Effect newEffect = new Effect();
        try {
            string json = Resources.Load<TextAsset>(effectsPath + effectName + effectTier).text;
            JsonUtility.FromJsonOverwrite(json, newEffect);
        } catch {
            Debug.Log("Effect \"" + effectName + effectTier + ".json\" not found.");
        }
        return newEffect;
    }
    public static Equipable getEquipable(string equipName) {
        Equipable newEquip = new Equipable();
        try {
            string json = Resources.Load<TextAsset>(equipsPath + equipName).text;
            JsonUtility.FromJsonOverwrite(json, newEquip);
        } catch {
            Debug.Log("Equipable \"" + equipName + ".json\" not found.");
        }
        newEquip.setPath(equipsPath + equipName);
        return newEquip;
    }
    public static Item getItem(string itemName) {
        Item newItem = new Item();
        try {
            string json = Resources.Load<TextAsset>(itemsPath + itemName).text;
            JsonUtility.FromJsonOverwrite(json, newItem);
        } catch {
            Debug.Log("Item \"" + itemName + ".json\" not found.");
        }
        newItem.setPath(itemsPath + itemName);
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
        try {
            string json = Resources.Load<TextAsset>(charactersPath + charName).text;
            JsonUtility.FromJsonOverwrite(json, character);
        } catch {
            Debug.Log("Character \"" + charName + ".json\" not found.");
        }
        return character;
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

    public static Texture2D getSkillIcon(string s) {
        try {
            return Resources.Load<Texture2D>(skillsIconPath + s);
        } catch {
            Debug.Log("Skill icon \"" + s + ".png\" not found.");
        }
        return null;
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
}
