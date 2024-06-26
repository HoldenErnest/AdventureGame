// Holden Ernest - 4/14/2023
// takes the selected tile type and converts it to an array that is then displayed to the scroll view

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System;

public class TilesDisplay : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler {

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

    private List<GameObject> cells = new List<GameObject>(); // the actual gameobjects for the cells
    private Sprite[] SObjects; // all cells info in the current group
    private GameObject[] OObjects; // all cells info in the current group
    private CharacterCreator[] CObjects;
    private List<DisplayInfo> foundObjects = new List<DisplayInfo>(); // all cells info in 'objects' that also have the key

    // event when the player changes the group
    public void updateGroup() {
        Camera.main.gameObject.GetComponent<EditPlacer>().updateGroup(getGroupNumber());
        loadCells(getGroupNumber());
    }
    public void updateKey() {
        generateCells();
    }

    // creates the cells into the scroll view based on what group it is
    private void loadCells(int id) {
        // load objects that want to be displayed into an array
        switch (id) {
            case 0:
                SObjects = Knowledge.getAllSpritesFromTexture("Ground"); // all background tiles
                break;
            case 1:
                SObjects = Knowledge.getAllSpritesFromTexture("Walls"); // collidable tiles (excluding objects)
                break;
            case 2:
                SObjects = Knowledge.getAllSpritesFromTexture("Detail"); // top layer tiles
                break;
            case 3:
                //SObjects = Knowledge.getAllSpritesFromTexture("Structures");
                break;
            case 4:
                OObjects = Knowledge.getAllObjectsInFolder("SavedObjects/Objects/");
                break;
            case 5:
                CObjects = Knowledge.getAllCharacters(); // These are used ONLY to set characters initital positions, 
                break;
            case 6:
                //OObjects = Knowledge.getAllObjectsInFolder("SavedObjects/Markers");
                break;
            default:
                break;
        }
        // if you reset the group, reset the key
        keyInput.text = "";
        generateCells();
        //resize the content y to fit the
        RectTransform rectTransform = ((RectTransform)contentTranform);
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -cellSize*(cells.Count/itemsPerRow));
        //contentTranform.localScale = new Vector3(contentTranform.localScale.x, (cells.Count/itemsPerRow), contentTranform.localScale.z);
    }

    private void generateCells() {
        clearCells();
        updateKeyObjects();
        for (int i = 0; i < foundObjects.Count; i++) {
            createCell(i);
        }
    }

    private int currentArrayType() {
        int n = getGroupNumber();
        if (n <= 2)
            return 0;
        if (n == 3 || n == 4 || n == 6)
            return 1;
        return 2;
    }

    // put the selected group into an array of data to be turned into displayCells
    private void updateKeyObjects() {

        foundObjects.Clear();
        string key = getKey(); // prep the array and set the key
        if (currentArrayType() == 0) { // fill the sprite array
            for (int i = 0; i< SObjects.Length; i++) {
                DisplayInfo d = new DisplayInfo(i,SObjects[i]);
                if (key.Equals("")){
                    foundObjects.Add(d);
                    continue;
                }
                if (d.getTitle().ToLower().Contains(key.ToLower()))
                    foundObjects.Add(d);
            }
        } else if (currentArrayType() == 1) { // otherwise fill the object array
            for (int i = 0; i< OObjects.Length; i++) {
                DisplayInfo d = new DisplayInfo(i,OObjects[i]);
                if (key.Equals("")){
                    foundObjects.Add(d);
                    continue;
                }
                if (d.getTitle().ToLower().Contains(key.ToLower()))
                    foundObjects.Add(d);
            }
        } else { // otherwise fill the character array
            for (int i = 0; i< CObjects.Length; i++) {
                DisplayInfo d = new DisplayInfo(i,CObjects[i]);
                if (key.Equals("")){
                    foundObjects.Add(d);
                    continue;
                }
                if (d.getTitle().ToLower().Contains(key.ToLower()))
                    foundObjects.Add(d);
            }
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
        cell.GetComponent<DisplayCell>().setCell(pos, foundObjects[pos].getTitle(), true, foundObjects[pos].getSprite(), this);
        //cell.transform.position = getLocationFromIndex(pos);
        Vector3 v = getLocationFromIndex(pos);
        cell.GetComponent<RectTransform>().offsetMin = new Vector2(v.x,0);//from left
        cell.GetComponent<RectTransform>().offsetMax = new Vector2(0, v.y); // from top
        cell.GetComponent<RectTransform>().sizeDelta = new Vector2 (cellSize, cellSize);
        cells.Add(cell);
    }
    // the position of the cell on the grid
    public Vector3 getLocationFromIndex(int l) { // change the location when doing sorting or others in the 'InventoryUI' class
        int locInRow = (l%itemsPerRow);
        int locInCol = -(l/itemsPerRow);

        float x = locInRow + (cellOffsetX * locInRow) + tableOffsetX;
        float y = locInCol + (cellOffsetY * locInCol) + tableOffsetY;
        return new Vector3(x, y, 0) * cellSize;
    }

    public int getGroupNumber() {
        return groupDropdown.value;
    }
    private string getKey() {
        return keyInput.text;
    }

    public void setSelected(int pos) { // set a selected tile called from DisplayCell. Used in EditPlacer
        if (currentArrayType() == 1)
            Camera.main.gameObject.GetComponent<EditPlacer>().setSelected(OObjects[foundObjects[pos].getPosition()]);
        else if (currentArrayType() == 0)
            Camera.main.gameObject.GetComponent<EditPlacer>().setSelected(SObjects[foundObjects[pos].getPosition()]);
        else // selected is character tiles
            Camera.main.gameObject.GetComponent<EditPlacer>().setSelected(CObjects[foundObjects[pos].getPosition()]);
    }
    
    //EVENTS
    public void OnPointerEnter(PointerEventData eventData){
        //Debug.Log("cant scroll");
        Camera.main.gameObject.GetComponent<ScrollZoom>().setCanScroll(false);
    }
    public void OnPointerExit(PointerEventData eventData){
        //Debug.Log("can now scroll");
        Camera.main.gameObject.GetComponent<ScrollZoom>().setCanScroll(true);
    }
    
}
