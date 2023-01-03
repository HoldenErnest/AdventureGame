//unused object. consolidate code between QuestUI and ItemCell which are essencially the same thing, just parsing different objects into them.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    private int location; //num/rowLength >> change when sorting items
    public float cellOffsetX;
    public float cellOffsetY;
    public float tableOffsetX;
    public float tableOffsetY;
    public int itemsPerRow;

    private Vector2 UIposition;

    public void setOffsets() {
        cellOffsetX = 0;
        cellOffsetY = 0;
        tableOffsetX = 0;
        tableOffsetY = 0;
        itemsPerRow = 1;
    }
    private void updateText() {
        textUI.text = "bruh";
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
}
