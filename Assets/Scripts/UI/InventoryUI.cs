//Holden Ernest - August 30, 2022

// Manage entire inventory UI, each item instantiates a new 'ItemCell' prefab
// this cannot affect or modify items, it is only for arranging(sorting) items

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    
    public GameObject rcItemMenu;

    public GameObject[] menuToggles; // used only to darken the color of the selected tab.
    public RectTransform scrollTransform;
    public int currentMenu; // 0 = inventory, 1 = quests
    public GameObject[] cellPrefabs;
    public List<GameObject> cells = new List<GameObject>();
    public Sprite[] invSprites; // the sprites for the inventory color changes.
    private Image invImage;

    void Start() {
        invImage = GetComponent<Image>();
    }

    public void updateCells() {
        updateHeight();
        repopulate(); // maybe do soemthing differetn here in the future(dont delete then repopulate all items each time inventory is opened)
    }


    private void repopulate() { // populate new cells for all items in Inventory into the grid
        removeAllCells();
        switch (currentMenu) { // depending on whatever menu is up change what the cells contain.
            case 0:
                foreach (Item items in Knowledge.inventory.getAllItems()) {
                    cells.Add(newCell(items));
                }
                break;
            case 1:
                foreach (Quest quests in Knowledge.inventory.getAllQuests()) {
                    cells.Add(newCell(quests));
                }
                break;
            default:
                Debug.Log("nothing selected");
                break;
        }
    }

    private GameObject newCell(Item item) { // runs for every new call for a cell
        GameObject g = Instantiate(cellPrefabs[0], new Vector3(0,0, 0), Quaternion.identity);
        g.transform.parent = scrollTransform;
        ItemCell cScript = g.GetComponent<ItemCell>();
        cScript.setUIposition(this.gameObject.transform.position);
        cScript.setItem(item);
        cScript.setOffsets();
        cScript.setLocation(cells.Count); // set the location of the item to its location in the array
        cScript.updateIsEquipped();

        return g;
    }
    private GameObject newCell(Quest quest) { // runs for every new call for a cell
        GameObject g = Instantiate(cellPrefabs[1], new Vector3(0,0, 0), Quaternion.identity);
        g.transform.parent = scrollTransform;
        QuestUI cScript = g.GetComponent<QuestUI>();
        cScript.setUIposition(this.gameObject.transform.position);
        cScript.setQuest(quest);
        cScript.setOffsets();
        cScript.setLocation(cells.Count); // set the location of the questUI to its location in the array

        return g;
    }

    private void removeAllCells() {
        for (int i = 0; i < cells.Count; i++) {
            Destroy(cells[i]);
        }
        cells = new List<GameObject>();
        //run garbage collection (thats a lotta memory potentially being unused on each itemCell deleted)
    }

    public void setMenuInventory() { // the onClick events for the buttons
        if (currentMenu != 0) {
            currentMenu = 0;
            updateInvSprite();
            updateCells();
        }
    }
    public void setMenuQuests() { // the onClick events for the buttons
        if (currentMenu != 1) {
            currentMenu = 1;
            updateInvSprite();
            updateCells();
        }
    }

    private void updateInvSprite() { // set the background sprite to current menu sprite
        invImage.sprite = invSprites[currentMenu];
    }

    private void updateHeight() { // update the content height so the scroll viewport can correctly translate when allowed
        scrollTransform.localPosition = Vector3.zero;
        scrollTransform.sizeDelta = new Vector2(0,800); // !!FIX, NEED HEIGHT OF ALL CONTENT -- maybe a simple fix like fitting to children !!
    }
}
