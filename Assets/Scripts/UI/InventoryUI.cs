//Holden Ernest - August 30, 2022

// Manage entire inventory UI, each item instantiates a new 'ItemCell' prefab
// this cannot affect or modify items, it is only for arranging(sorting) items

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    public GameObject itemCellPrefab;
    public GameObject questCellPrefab;
    public GameObject rcItemMenu;

    public GameObject[] menuToggles; // used only to darken the color of the selected tab.
    public int currentMenu; // 0 = inventory, 1 = quests

    public List<GameObject> cells = new List<GameObject>();

    public void updateCells() {
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
        GameObject g = Instantiate(itemCellPrefab, new Vector3(0,0, 0), Quaternion.identity);
        g.transform.parent = gameObject.transform;
        ItemCell cScript = g.GetComponent<ItemCell>();
        cScript.setUIposition(this.gameObject.transform.position);
        cScript.setItem(item);
        cScript.setOffsets();
        cScript.setLocation(cells.Count); // set the location of the item to its location in the array
        cScript.updateIsEquipped();

        return g;
    }
    private GameObject newCell(Quest quest) { // runs for every new call for a cell
        GameObject g = Instantiate(questCellPrefab, new Vector3(0,0, 0), Quaternion.identity);
        g.transform.parent = gameObject.transform;
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
            updateCells();
        }
    }
    public void setMenuQuests() { // the onClick events for the buttons
        if (currentMenu != 1) {
            currentMenu = 1;
            updateCells();
        }
    }
}
