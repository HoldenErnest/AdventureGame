// Holden Ernest - 4/15/2023
// An object that holds the basic data for any cells

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCell : MonoBehaviour {
    
    private int arrayLocation; // for getting the actual object back out of the arary it was initially in.
    private string title; // the item name or whatever
    private bool showTitle = true; // whether or not the title is displayed under the texture
    private Sprite texture; // the texture displayed (optional)
    private TilesDisplay table;

    public TMP_Text cellText;
    public Image cellIcon;

    //public int location;

    void Start() {
        //cellText = gameObject.GetComponent<TMP_Text>();
        //cellIcon = this.gameObject.GetComponent<Image>();
    }

    public void setCell(int loc, string name, bool showT, Sprite tex, TilesDisplay t) {
        arrayLocation = loc;
        title = name;
        showTitle = showT;
        texture = tex;
        table = t;
        updateCell();
    }

    // Display the cell
    private void updateCell() {
        updateText();
        updateTexture();
    }
    private void updateText() {
        if (!showTitle) {
            cellText.text = "";
            return;
        }
        cellText.text = title;
    }
    private void updateTexture() {
        cellIcon.sprite = texture;
    }

    public void onClick() {
        Debug.Log(title);
        table.setSelected(arrayLocation);
    }
}
