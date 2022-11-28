//Holden Ernest - August 30, 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour // shows UI of each item in the inventoryUI
{
    public TextMeshProUGUI textUI;
    public GameObject border;
    private Item item;
    private int location; //num/rowLength >> change when sorting items
    private readonly float cellOffsetX = 0.2f;
    private readonly float cellOffsetY = 0.5f;
    private readonly float tableOffsetX = -5.5f;
    private readonly float tableOffsetY = 2.3f;
    private readonly int itemsPerRow = 10;

    private Vector2 UIposition;

    public void setItem(Item i) {
        item = i;
        updateText();
        updateIcon();
    }

    private void updateText() {
        textUI.text = item.ammount + " - " + item.itemName;
    }
    private void updateIcon() {
        Texture2D tex = Knowledge.getItemIcon(item.sid);
        
        GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f)); // create a new sprite to put in from the knowledge
    }

    public void setLocation(int l) { // change the location when doing sorting or others in the 'InventoryUI' class
        location = l;
        updatePosition();
    }
    private void updatePosition() {//given an integer position
        int locInRow = (location%itemsPerRow);
        int locInCol = -(location/itemsPerRow);

        float x = locInRow + (locInRow*cellOffsetX) + UIposition.x + tableOffsetX;
        float y = locInCol + (locInCol*cellOffsetY) + UIposition.y + tableOffsetY;
        this.gameObject.transform.position = new Vector2(x, y);
    }

    public void setUIposition(Vector2 v) { // where the icon is positioned (fake "screen space")
        UIposition = v;
    }

    public void updateIsEquipped() {
        if (item.GetType() == typeof(Equipable)) {
            if (((Equipable)item).isEquipped()) {
                border.SetActive(true);
            } else {
                border.SetActive(false);
            }
        }
    }

    public Item getItem() {
        return item;
    }
}
