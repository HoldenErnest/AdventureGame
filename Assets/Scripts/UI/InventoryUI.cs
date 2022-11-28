//Holden Ernest - August 30, 2022

// Manage entire inventory UI, each item instantiates a new 'ItemCell' prefab
// this cannot affect or modify items, it is only for arranging(sorting) items

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    public GameObject cellPrefab;
    public GameObject rcItemMenu;
    public List<GameObject> cells = new List<GameObject>();

    public void updateCells() {
        repopulate(); // maybe do soemthing differetn here in the future(dont delete then repopulate all items each time inventory is opened)
    }


    private void repopulate() { // populate new cells for all items in Inventory into the grid
        removeAllCells();
        foreach (Item items in Knowledge.inventory.getAllItems()) {
            cells.Add(newCell(items));
        }
    }

    private GameObject newCell(Item item) { // runs for every new call for a cell
        GameObject g = Instantiate(cellPrefab, new Vector3(0,0, 0), Quaternion.identity);
        g.transform.parent = gameObject.transform;
        ItemCell cScript = g.GetComponent<ItemCell>();
        cScript.setUIposition(this.gameObject.transform.position);
        cScript.setItem(item);
        cScript.setLocation(cells.Count); // set the location of the item to its location in the array
        cScript.updateIsEquipped();

        return g;
    }

    

    private void removeAllCells() {
        for (int i = 0; i < cells.Count; i++) {
            Destroy(cells[i]);
        }
        cells = new List<GameObject>();
        //run garbage collection (thats a lotta memory potentially being unused on each itemCell deleted)
    }
}
