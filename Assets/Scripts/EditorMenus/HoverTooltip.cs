// Holden Ernest - 10/14/2023
// placed on the main camera, keeps track of and handles the tooltip gameobject for the editor menus

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class HoverTooltip : MonoBehaviour
{
    public GameObject theToolTip;

    public void setTooltip(string title, string desc, Vector2 pos) {
        theToolTip.transform.GetChild(0).GetComponent<TMP_Text>().text = $"_{title}_\n\n{desc}";
        //theToolTip.transform.position = pos; // set its position to near the mouse maybe
        showTooltip();
    }
    public void hideTooltip() {
        theToolTip.SetActive(false);
    }
    private void showTooltip() {
        theToolTip.SetActive(true);
    }
}
