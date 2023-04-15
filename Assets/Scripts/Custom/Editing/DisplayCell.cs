// Holden Ernest - 4/15/2023
// An object that holds the basic data for any cells

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCell : MonoBehaviour {
    
    private string title; // the item name or whatever
    private bool showTitle = true; // whether or not the title is displayed under the texture
    private Sprite texture; // the texture displayed (optional)

    public TMP_Text cellText;
    public Image cellIcon;

    //public int location;

    void Start() {
        //cellText = gameObject.GetComponent<TMP_Text>();
        //cellIcon = this.gameObject.GetComponent<Image>();
    }

    public void setCell(string name, bool showT, Sprite tex) {
        title = name;
        showTitle = showT;
        texture = tex;
        updateCell();
    }
    public void setCell(string name) {
        setCell(name, true, null);
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
}
