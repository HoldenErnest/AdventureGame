// Holden Ernest - 10/14/2023
// A simple script for handling which menu the player is in for the Editor Creator

using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class ChooseMenu : MonoBehaviour
{
    public GameObject[] menus;
    public int currentMenu = 0;

    private TMP_Dropdown m_Dropdown;

    void Start() {
        m_Dropdown = GetComponent<TMP_Dropdown>();
        m_Dropdown.onValueChanged.AddListener(delegate {
            setMenu(m_Dropdown);
        });
    }

    private void setMenu(TMP_Dropdown changed) { // forwarded event specified in Start()
        menus[currentMenu].SetActive(false);
        menus[changed.value].SetActive(true);
        currentMenu = changed.value;
    }
}
