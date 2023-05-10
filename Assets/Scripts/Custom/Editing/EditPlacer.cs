// Holden Ernest - 4/20/2023
// Controller for the user to place objects on the editing grids

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPlacer : MonoBehaviour {

    [SerializeField]
    private MapToSave[] grids;
    
    private GameObject selectedObject; // the object to place
    private Sprite selectedTile; // the tile to place  <^ these are interchangable

    [SerializeField]
    private TilesDisplay sectionTable;

    void Update() {
        if (!canPlace()) return;
        if (Input.GetMouseButton(0)) {
            place();
        }
        if (Input.GetMouseButton(1)) {
            remove();
        }
        if (Input.GetMouseButton(2)) {
            floodPlace();
        }
    }

    private void place() {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (selectedTile != null) {
            grids[sectionTable.getGroupNumber()].setTile(wp.x, wp.y, selectedTile);
        } else if (selectedObject != null) {
            Debug.Log("is an object");
            //grids[sectionTable.getGroupNumber()].setTile(wp.x, wp.y, selectedObject);
        }
    }
    private void floodPlace() {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (selectedTile != null) {
            grids[sectionTable.getGroupNumber()].floodPlace(wp.x, wp.y, selectedTile);
        } else if (selectedObject != null) {
            Debug.Log("is an object");
            //grids[sectionTable.getGroupNumber()].setTile(wp.x, wp.y, selectedObject);
        }
    }
    private void remove() {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (grids[sectionTable.getGroupNumber()].activeTile(wp.x,wp.y))
            grids[sectionTable.getGroupNumber()].setTile(wp.x, wp.y, null);
        else Debug.Log("not active");
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
    //saves all tilemaps to a WORLD file (a csv lol)
    public void saveMap() {
        Debug.Log("saving map!");
        for (int i = 0; i < grids.Length; i++) {
            grids[i].saveMap("test");
        }
    }
    public void loadMap() {
        Debug.Log("loading map!");
        for (int i = 0; i < grids.Length; i++) {
            grids[i].loadMap("test");
        }
    }

    public void updateGroup(int g) {
        for (int i = 0; i < grids.Length; i++) {
            if (grids[i].layer != g) {
                grids[i].fadeMap();
            } else
                grids[i].unfadeMap();
        }
    }
}
