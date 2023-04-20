// Holden Ernest - 4/20/2023
// Controller for the user to place objects on the editing grids

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPlacer : MonoBehaviour {

    [SerializeField]
    private MapToImage[] grids;
    
    private GameObject selectedObject; // the object to place
    private Sprite selectedTile; // the tile to place  <^ these are interchangable

    [SerializeField]
    private TilesDisplay sectionTable;

    void Update() {
        if (!canPlace()) return;
        if (Input.GetMouseButton(0)) {
            place();
        }
    }

    private void place() {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (selectedTile != null) {
            grids[sectionTable.getGroupNumber()].setTile(wp.x, wp.y, selectedTile);
        }
        if (selectedObject != null) {
            Debug.Log("is an object");
            //grids[sectionTable.getGroupNumber()].setTile(wp.x, wp.y, selectedObject);
        }
    }

    public void setSelected(Sprite s) {
        selectedObject = null;
        selectedTile = s;
    }
    public void setSelected(GameObject g) {
        selectedTile = null;
        selectedObject = g;
    }

    private bool canPlace() {
        return true;
    }
}
