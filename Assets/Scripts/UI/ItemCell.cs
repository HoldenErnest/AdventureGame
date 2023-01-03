//Holden Ernest - August 30, 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemCell : InventoryCell // shows UI of each item in the inventoryUI
{
    private Item item;
    public GameObject border;
    
    public void setOffsets() {
        cellOffsetX = 0.2f;
        cellOffsetY = 0.5f;
        tableOffsetX = -5.5f;
        tableOffsetY = 2.3f;
        itemsPerRow = 10;
    }

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
