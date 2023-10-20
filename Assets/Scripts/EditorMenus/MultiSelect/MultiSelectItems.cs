// Holden Ernest - 10/15/2023
// The script to add Items to a multiSelect UI thing

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MultiSelectItems : MonoBehaviour {

    public string title;

    [SerializeField]
    private GameObject genericItemBP; // default gameobject to instantiate with each new item
    [SerializeField]
    private GameObject content; // default gameobject to instantiate with each new item
    [SerializeField]
    private TMP_Text titleText;

    [HideInInspector]
    public bool transferItems; // everytime you click an item it is transfered to the other list

    [HideInInspector]
    public MultiSelectItems otherList;

    [HideInInspector]
    public bool loadItems = false;
    [HideInInspector]
    public bool loadEquips = false;
    [HideInInspector]
    public bool loadSkills = false;
    [HideInInspector]
    public bool loadQuests = false;

    private List<GameObject> items = new List<GameObject>(); // the base list of items

    void Start() {
        updateTitle();
        setItems();
    }


    private void setItems() {
        if (loadItems) {
            Item[] tempItems = Knowledge.getAllItems();
            foreach (Item itm in tempItems) {
                addItem(itm);
            }
        }
        if (loadEquips) {
            Equipable[] tempEquips = Knowledge.getAllEquips();
            foreach (Equipable eq in tempEquips) {
                addItem(eq);
            }
        }
        if (loadSkills) {
            Skill[] tempSkills = Knowledge.getAllSkills();
            foreach (Skill eq in tempSkills) {
                addItem(eq);
            }
        }
        if (loadQuests) {
            string[] tempQuests = Knowledge.getAllQuestFolderNames();
            foreach (string qst in tempQuests) {
                addItem(qst);
            }
        }
    }

    private void updateTitle() {
        titleText.text = title;
    }

    private void addItem(Item i) {
        GameObject g = Instantiate(genericItemBP, content.transform);
        items.Add(g);
        g.GetComponent<ItemForMulti>().set(this, i);
        g.GetComponent<ItemForMulti>().setButtonTransfer(transferItems);
    }
    private void addItem(Skill i) {
        GameObject g = Instantiate(genericItemBP, content.transform);
        items.Add(g);
        g.GetComponent<ItemForMulti>().set(this, i);
        g.GetComponent<ItemForMulti>().setButtonTransfer(transferItems);
    }
    private void addItem(string i) {
        GameObject g = Instantiate(genericItemBP, content.transform);
        items.Add(g);
        g.GetComponent<ItemForMulti>().set(this, i);
        g.GetComponent<ItemForMulti>().setButtonTransfer(transferItems);
    }
    public void transferAnItem(GameObject g) {
        if (transferItems) {
            if (loadSkills)
                otherList.addItem(g.GetComponent<ItemForMulti>().skillValue);
            else if (!loadQuests) { 
                otherList.addItem(g.GetComponent<ItemForMulti>().itemValue);
            } else { // load a generic string type instead (quests use these)
                otherList.addItem(g.GetComponent<ItemForMulti>().otherValue);
            }
        } else {
            items.Remove(g);
            Destroy(g);
        }
    }

    public string[] getAllStrings() { // returns all objects that are of type T
        string[] final = new string[items.Count];
        for (int i = 0; i < items.Count; i++) {
            ItemForMulti ifm = items[i].GetComponent<ItemForMulti>();
            if (!System.Object.ReferenceEquals(ifm.skillValue, null)) final[i] = ifm.skillValue.getPath();
            else if (!System.Object.ReferenceEquals(ifm.itemValue, null)) final[i] = ifm.itemValue.getPath();
            else final[i] = ifm.otherValue;
        }
        return final;
    }
    public ItemSave[] getAllItemSaves() { // returns all objects that are of type T
        ItemSave[] itemSaves = new ItemSave[items.Count];
        for (int i = 0; i < items.Count; i++) {
            ItemForMulti ifm = items[i].GetComponent<ItemForMulti>();
            if (!System.Object.ReferenceEquals(ifm.itemValue, null)) {
                itemSaves[i] = ifm.itemValue.toItemSave();
            }
        }
        return itemSaves;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MultiSelectItems))]
public class RandomScript_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector(); // for other non-HideInInspector fields

		MultiSelectItems script = (MultiSelectItems)target;

		// draw checkbox for the bool
		script.transferItems = EditorGUILayout.Toggle("Transfer Items", script.transferItems);
		if (script.transferItems) // if bool is true, show other fields
		{
			script.otherList = EditorGUILayout.ObjectField("Other Multi-Select", script.otherList, typeof(MultiSelectItems), true) as MultiSelectItems;
            if (!script.loadSkills && !script.loadQuests) {
                script.loadItems = EditorGUILayout.Toggle("Load Items", script.loadItems);
                script.loadEquips = EditorGUILayout.Toggle("Load Equips", script.loadEquips);
            } else {
                script.loadItems = false;
                script.loadEquips = false;
            }
            if (!script.loadQuests && !script.loadItems && !script.loadEquips) {
                script.loadSkills = EditorGUILayout.Toggle("Load Skills", script.loadSkills);
            } else {
                script.loadSkills = false;
            }
            if (!script.loadSkills && !script.loadItems && !script.loadEquips) {
                script.loadQuests = EditorGUILayout.Toggle("Load Quests", script.loadQuests);
            } else {
                script.loadQuests = false;
            }
		}
	}
}
#endif
