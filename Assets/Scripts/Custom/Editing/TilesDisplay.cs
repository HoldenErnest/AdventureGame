// Holden Ernest - 4/14/2023
// takes the selected tile type and converts it to an array that is then displayed to the scroll view

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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

    private DisplayInfo[] objects; // all cells info in the current group

    private List<DisplayInfo> foundObjects = new List<DisplayInfo>(); // all cells info in 'objects' that also have the key

    void Start() {
        //contentTranform.localScale = new Vector3(cellSize, cellSize, 1);
    }

    // event when the player changes the group
    public void updateGroup() {
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
                objects = getInfoFromArray(Knowledge.getAllSpritesFromTexture("Ground"));
                break;
            case 1:
                objects = getInfoFromArray(Knowledge.getAllSpritesFromTexture("Walls"));
                break;
            case 2:
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

    private DisplayInfo[] getInfoFromArray(Sprite[] s) {
        DisplayInfo[] dis = new DisplayInfo[s.Length];
        for (int i = 0; i < s.Length; i++) {
            dis[i] = new DisplayInfo(s[i]);
        }
        return dis;
    }

    private void generateCells() {
        clearCells();
        updateKeyObjects();
        for (int i = 0; i < foundObjects.Count; i++) {
            createCell(i);
        }
    }

    private void updateKeyObjects() {
        if (objects == null) {
            loadCells(0);
            return;
        }
        foundObjects.Clear();
        string key = getKey();
        foreach (DisplayInfo di in objects) {
            
            if (key.Equals("")){
                foundObjects.Add(di);
                continue;
            }
            if (di.getTitle().ToLower().Contains(key.ToLower()))
                foundObjects.Add(di);
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
        cell.GetComponent<DisplayCell>().setCell(foundObjects[pos].getTitle(), true, foundObjects[pos].getSprite());
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
