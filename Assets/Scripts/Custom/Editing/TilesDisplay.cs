// Holden Ernest - 4/14/2023
// takes the selected tile type and converts it to an array that is then displayed to the scroll view

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TilesDisplay : MonoBehaviour {

    public GameObject cellPrefab;
    public Sprite defaultImg;

    public TMP_Dropdown groupDropdown;
    public TMP_InputField keyInput;

    public int cellSize = 10;

    // offsets for each tile
    public float cellOffsetX = 0;
    public float cellOffsetY = 0;
    public float tableOffsetX = 0;
    public float tableOffsetY = 0;
    public int itemsPerRow = 2;

    public Transform contentTranform;

    private List<GameObject> cells = new List<GameObject>();

    private Sprite[] objects;

    private List<Sprite> foundObjects = new List<Sprite>();

    void Start() {
        //contentTranform.localScale = new Vector3(cellSize, cellSize, 1);
    }

    // event when the player changes the group
    public void updateGroup() {
        
        loadCells(getGroupNumber());
    }

    // creates the cells into the scroll view based on what group it is
    private void loadCells(int id) {
        // load objects that want to be displayed into an array
        Debug.Log("ID is: " + id);
        switch (id) {
            case 0:
                objects = Knowledge.getAllSpritesFromTexture("Ground");
                break;
            case 1:
                objects = Knowledge.getAllSpritesFromTexture("Walls");
                break;
            case 2:
                break;
            default:
                break;
        }
        // create the display cells based off this array of random objects
        generateCells();
    }

    private void generateCells() {
        clearCells();
        updateKeyObjects();
        for (int i = 0; i < foundObjects.Count; i++) {
            createCell(i);
        }
    }

    private void updateKeyObjects() {
        foundObjects.Clear();
        string key = getKey();
        Debug.Log("kjey is " + key);
        foreach (Sprite s in objects) {
            
            if (key.Equals("")){
                foundObjects.Add(s);
                continue;
            }
            if (s.name.ToLower().Contains(key.ToLower()))
                foundObjects.Add(s);
        }
    }
    private void clearCells() {
        foreach (GameObject cell in cells) {
            Destroy(cell);
        }
        cells.Clear();
    }

    private void createCell(int pos) {
        GameObject cell = Instantiate(cellPrefab, contentTranform);
        cell.GetComponent<DisplayCell>().setCell(foundObjects[pos].name, true, foundObjects[pos]);
        //cell.transform.position = getLocationFromIndex(pos);
        Vector3 v = getLocationFromIndex(pos);
        cell.GetComponent<RectTransform>().offsetMin = new Vector2(v.x,0);
        cell.GetComponent<RectTransform>().offsetMax = new Vector2(0, v.y);
        cell.GetComponent<RectTransform>().sizeDelta = new Vector2 (cellSize, cellSize);
        cells.Add(cell);
    }
    // the position of the cell on the grid
    public Vector3 getLocationFromIndex(int l) { // change the location when doing sorting or others in the 'InventoryUI' class
        int locInRow = (l%itemsPerRow);
        int locInCol = -(l/itemsPerRow);

        float x = locInRow + (cellOffsetX * locInRow) /*+ contentTranform.position.x*/ + tableOffsetX;
        float y = locInCol + (cellOffsetY * locInCol) /*+ contentTranform.position.y*/ + tableOffsetY;
        return new Vector3(x, y, 0) * cellSize;
    }

    private int getGroupNumber() {
        return groupDropdown.value;
    }
    private string getKey() {
        return keyInput.text;
    }

}
