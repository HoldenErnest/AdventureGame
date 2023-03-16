// Holden Ernest - 2/25/2023
// Activates tooltips from events for Tools.cs.
// All tooltips gameobjects pass information to Tools.cs to process

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTips : MonoBehaviour {

    private Tools toolScript;

    public bool allowHoverUI;

    void Start() {
        toolScript = Knowledge.tools;
    }

    void OnMouseEnter() {
        if (allowHoverUI && hoverActive()) {
            toolScript.setHoverUI(this.gameObject.GetComponent<Character>());
        }
    }
    void OnMouseExit() {
        if (allowHoverUI && hoverActive()) {
            toolScript.closeHoverUI();
        }

    }

    private bool tooltipsActive() {
        if (!toolScript) return false;

        return toolScript.enabled;

    }
    private bool hoverActive() {
        if (!tooltipsActive()) return false;

        return toolScript.allowHoverUI;
    }
}
