// Holden Ernest - 10/11/2023
// all the saved info for the chest gameobject

//TODO: load and unload onto the maps object file when you open and close rather than keeping all chests items in Memory
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;

[Serializable]
public class Chest : MonoBehaviour {
    public Item[] savedItems;

    [SerializeField]
    private List<Item> items = new List<Item>();

    void Start() {
        loadItems();

        // TEMP
        addItem("orange");
        addItem("apple");
        addItem("orange");
    }
    private void loadItems() { // transfer all savedItems to items
        foreach (Item i in savedItems) {
            items.Add(i);
        }
    }
    private void updateItems() { // updates all items that can be saved (savedItems = items)
        savedItems = new Item[items.Count];
        for (int i = 0; i < savedItems.Length; i++) {
            savedItems[i] = items[i];
        }
        items.Clear(); // remove the info so it doesnt hog memory unnessicarily
    }

    public void addItem(string itemName) {
        Debug.Log("adding item " + itemName + " to chest");
        items.Add( Knowledge.getItem(itemName) );
    }
}
