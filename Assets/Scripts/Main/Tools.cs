// Holden Ernest - 2/25/2023
// UI for any initially hidden stats / help
// takes arguments from ToolTips.cs gameObject events

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour {

    public bool allowHoverUI;

    public CharacterUI charHoverUI;

    private Transform pos;
    private bool hovering;

    void Start() { // everytime this script is enabled
        
    }
    void Update() {
        if (allowHoverUI && pos)
            charHoverUI.gameObject.transform.position = pos.position; // update position
    }

    public void setHoverUI(Character c) {
        hovering = true;
        charHoverUI.gameObject.SetActive(true);
        pos = c.gameObject.transform;
        charHoverUI.setCharacter(c);
    }
    public void closeHoverUI() {
        StartCoroutine(close());
    }
    public IEnumerator close() {
        hovering = false;
        yield return new WaitForSeconds(0.5f);
        if (!hovering)
            charHoverUI.gameObject.SetActive(false);
    }
}
