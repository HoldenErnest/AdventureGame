//Holden Ernest - August 30, 2022

// Manage entire inventory UI, each item instantiates a new 'ItemCell' prefab
// this cannot affect or modify items, it is only for arranging(sorting) items

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    
    public GameObject rcItemMenu;

    public GameObject[] menuToggles; // used only to darken the color of the selected tab.
    public GameObject scroller;
    public int currentMenu; // 0 = inventory, 1 = quests
    public GameObject[] cellPrefabs;
    public List<GameObject> cells = new List<GameObject>();
    public Sprite[] invSprites; // the sprites for the inventory color changes.
    private Image invImage;
    private RectTransform contentTransform; // child of scroller

    private GameObject selectedCell;

    //selected cell INFORMATION...
    public GameObject questInfo;
    private TextMeshProUGUI questDesc;
    private TextMeshProUGUI questTitle;

    void Awake() {
        invImage = GetComponent<Image>();
        questTitle = questInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questDesc = questInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        contentTransform = scroller.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void updateCells() {
        updateHeight();
        repopulate();
    }


    private void repopulate() { // populate new cells for all items in Inventory into the grid
        removeAllCells();
        switch (currentMenu) { // depending on whatever menu is up change what the cells contain.
            case 0:
                foreach (Item items in Knowledge.player.inventory.getAllItems()) {
                    GameObject gm = newCell(items);
                    cells.Add(gm);
                    //gm.GetComponent<Button>().onClick.AddListener(() => setSelectedCell(gm));
                }
                break;
            case 1:
                foreach (Quest quests in Knowledge.player.inventory.getAllQuests()) {
                    GameObject gm = newCell(quests);
                    cells.Add(gm);

                    gm.GetComponent<Button>().onClick.AddListener(() => setSelectedCell(gm));
                }
                break;
            default:
                Debug.Log("nothing selected");
                break;
        }
    }
    private int getCellCount() { // populate new cells for all items in Inventory into the grid
        switch (currentMenu) { // depending on whatever menu is up change what the cells contain.
            case 0:
                return Knowledge.player.inventory.getAllItems().Count;
                break;
            case 1:
                return Knowledge.player.inventory.getAllQuests().Count;
                break;
            default:
                break;
        }
        return 0;
    }

    private GameObject newCell(Item item) { // runs for every new call for a cell
        GameObject g = Instantiate(cellPrefabs[0], new Vector3(0,0, 0), Quaternion.identity);
        g.transform.parent = contentTransform;
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
        g.transform.parent = contentTransform;
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
            questInfo.SetActive(false);
            updateInvSprite();
            updateCells();
        }
    }
    public void setMenuQuests() { // the onClick events for the buttons
        if (currentMenu != 1) {
            questInfo.SetActive(true);
            currentMenu = 1;
            updateInvSprite();
            updateCells();
        }
    }

    private void updateInvSprite() { // set the background sprite to current menu sprite
        invImage.sprite = invSprites[currentMenu];
        updateScrollTrans();
    }

    private void updateScrollTrans() { // updates the transform based on selected menu
        RectTransform tt = scroller.GetComponent<RectTransform>();
        switch (currentMenu) {
            case 0:
                tt.localPosition = new Vector3(1f,-18f,0f);
                tt.sizeDelta = new Vector3(385f,253f,0);
                break;
            case 1:
                tt.localPosition = new Vector3(71f,-18f,0f);
                tt.sizeDelta = new Vector3(243f,253f,0);
                break;
            default:
                break;
        }
    }
    private void updateHeight() { // update the content height so the scroll viewport can correctly translate when allowed
        contentTransform.localPosition = Vector3.zero;
        float height = 45;
        contentTransform.sizeDelta = new Vector2(0,height*getCellCount()); // !!FIX, NEED HEIGHT OF ALL CONTENT -- maybe a simple fix like fitting to children !!
    }
    private void updateSelectedInfo(Item i) { // update whatever info needed for the currently selected cell
        //the item 
    }
    private void updateSelectedInfo(Quest q) { // update whatever info needed for the currently selected cell
        questTitle.text = q.title;
        questDesc.text = q.desc + "\n\n";
        questDesc.text += q.getProgressionInfo();
    }
    public void updateCurrentSelected() {
        setSelectedCell(selectedCell);
    }
    private void updateCellOrder() { // update the cells if theyre moved in the array.
        
        for (int i = 0; i < cells.Count; i++) {
            cells[i].GetComponent<QuestUI>().setLocation(i);
        }
    }

    //OnClick()
    public void setSelectedCell(GameObject g) { // The OnClick event for the cells
        selectedCell = g;
        if (GameObject.ReferenceEquals(g,null)) return;
        switch (currentMenu) {
            case 0:
                Item item = g.GetComponent<ItemCell>().getItem();
                updateSelectedInfo(item);
                break;
            case 1:
                Quest quest = g.GetComponent<QuestUI>().getQuest();
                updateSelectedInfo(quest);
                break;
            default:
                break;
        }
    }

    public void moveCellUp() { // Onclick, move the cell up in the array
        if (GameObject.ReferenceEquals(selectedCell, null)) return;
        if (selectedCell.GetComponent<QuestUI>().getLocation() <= 0) {
            return;
        }
        int thisLocation = selectedCell.GetComponent<QuestUI>().getLocation();
        GameObject temp = cells[thisLocation - 1];
        cells[thisLocation - 1] = cells[thisLocation]; // prev = selected;
        cells[thisLocation] = temp;

        updateCellOrder();
    }
    public void moveCellDown() { // Onclick, move the cell down in the array
        if (GameObject.ReferenceEquals(selectedCell, null)) return;
        if (selectedCell.GetComponent<QuestUI>().getLocation() >= cells.Count - 1) {
            return;
        }
        int thisLocation = selectedCell.GetComponent<QuestUI>().getLocation();
        GameObject temp = cells[thisLocation + 1];
        cells[thisLocation + 1] = cells[thisLocation]; // next = selected;
        cells[thisLocation] = temp; // selected = oldNext

        updateCellOrder();
    }
}
