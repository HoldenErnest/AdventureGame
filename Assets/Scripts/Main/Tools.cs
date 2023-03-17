// Holden Ernest - 2/25/2023
// UI for any initially hidden stats / help
// takes arguments from ToolTips.cs gameObject events

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour {

    public bool allowHoverUI;

    public CharacterUI charHoverUI;

    private Camera cam;
    private Transform pos;
    private Coroutine hovering; 

    void Start() { // everytime this script is enabled
        cam = this.gameObject.GetComponent<Camera>();
    }
    // update the char health object
    public void updateHealth() {
        if (charHoverUI != null && allowHoverUI)
            charHoverUI.updateHealth();
    }
    // Events --------
    public void setHoverUI(Character c) {
        if (hovering != null)
            StopCoroutine(hovering);
        charHoverUI.gameObject.SetActive(true);
        pos = c.gameObject.transform;
        charHoverUI.setCharacter(c);
    }
    public void closeHoverUI() {
        hovering = StartCoroutine(close());
    }
    private IEnumerator close() {
        yield return new WaitForSeconds(0.4f);
        charHoverUI.gameObject.SetActive(false);
    }
}
