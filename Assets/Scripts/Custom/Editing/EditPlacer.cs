// Holden Ernest - 4/20/2023
// Controller for the user to place objects on the editing grids

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditPlacer : MonoBehaviour {

    [SerializeField]
    private MapToSave[] grids;
    
    private Tile selectedSpecialTile; // the object to place
    private Sprite selectedTile; // the tile to place  <^ these are interchangable

    [SerializeField]
    private TilesDisplay sectionTable;

    [SerializeField]
    private GameObject visual;

    //bools for not allowing placement
    public bool hoveringDisplay = false;

    private bool editMode = true;

    void Update() {
        if (!canPlace()) return;
        if (!editMode) return;
        moveVisual();
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
        if (selectedSpecialTile == null) {
            grids[sectionTable.getGroupNumber()].setTile(wp.x, wp.y, selectedTile);
        } else {
            Debug.Log("is an object");
            grids[sectionTable.getGroupNumber()].setTile(wp.x, wp.y, selectedSpecialTile);
        }
    }
    private void floodPlace() {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (selectedSpecialTile == null) {
            grids[sectionTable.getGroupNumber()].floodPlace(wp.x, wp.y, selectedTile);
        } else {
            Debug.Log("is an object");
            // COMMENT BACK IN TO FLOOD PLACE SPECIAL TILES! (I removed it because it seemed unnecessary)
            //grids[sectionTable.getGroupNumber()].floodPlace(wp.x, wp.y, selectedSpecialTile);
        }
    }
    private void remove() {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (grids[sectionTable.getGroupNumber()].activeTile(wp.x,wp.y))
            grids[sectionTable.getGroupNumber()].removeTile(wp.x, wp.y);
        // otherwise the tile isnt active
    }

    public void setSelected(Sprite s) {
        selectedSpecialTile = null;
        selectedTile = s;
        updateVisual();
    }
    public void setSelected(Tile t) {
        selectedSpecialTile = t;
        selectedTile = t.sprite;
        updateVisual();
    }
    public void setSelected(CharacterCreator c) {
        CharacterTile t = new CharacterTile(c);
        t.sprite = c.getIcon();
        selectedSpecialTile = t;
        selectedTile = t.sprite;
        updateVisual();
    }
    private void updateVisual() {
        visual.GetComponent<SpriteRenderer>().sprite = selectedTile;
    }
    private void moveVisual() {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2Int v = grids[0].worldToGridPoint(wp.x,wp.y);
        visual.transform.position = new Vector3(Mathf.RoundToInt(wp.x+0.5f)-0.5f,Mathf.RoundToInt(wp.y+0.5f)-0.5f,0);
    }

    public void changeMode() { // swaps between edit mode and view mode
        editMode = !editMode;
        if (!editMode) {
            unfadeAll();
            visual.gameObject.SetActive(false);
        } else {
            visual.gameObject.SetActive(true);
            updateGroup(sectionTable.getGroupNumber());
        }
    }

    private bool canPlace() {
        return !hoveringDisplay;

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
        selectedTile = null;
        selectedSpecialTile = null;
        updateVisual();
        for (int i = 0; i < grids.Length; i++) {
            if (grids[i].layer != g) {
                grids[i].fadeMap();
            } else
                grids[i].unfadeMap();
        }
    }
    private void unfadeAll() {
        for (int i = 0; i < grids.Length; i++) {
            grids[i].unfadeMap();
        }
    }

    //key the groups into tile types !! there is also one of these in TilesDisplay -- make sure to edit that as well !!
    private int currentArrayType() {
        int n = sectionTable.getGroupNumber();
        if (n <= 2)
            return 0;
        if (n == 3 || n == 4 || n == 6)
            return 1;
        return 2;
    }
}
