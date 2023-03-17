using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class CustomDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    public bool multiSelect;
    public string path = "Assets/Resources/Textures/Bodies/"; // what directory to access

    private string[] list;

    public List<string> theList = new List<string>();

    void Start() {
        dropdown = GetComponent<TMP_Dropdown>();
        updateOptions();
        if (multiSelect)
            dropdown.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "--";
    }
    //bodyPath = "Assets/Resources/Textures/Bodies/";
    //itempath = "Assets/Resources/SavedObjects/Items/";
    //equipPath = "Assets/Resources/SavedObjects/Equips/";
    //skillPath = "Assets/Resources/SavedObjects/Skills/";

    private void updateOptions() {
        if (path == null || path == "") return;
        try {
            list = Directory.GetFiles(path);
            //list.Concat(Directory.GetFiles(skillPath2)).ToArray();
        } catch (Exception e) {
            Debug.Log(e);
        }
        populate();
    }

    private void populate() {
        dropdown.options.Clear ();
        foreach (string t in list) {
            if (!t.EndsWith(".meta")) {
                string[] a = t.Split('/');
                string newText = a[a.Length - 1];
                int dot = newText.LastIndexOf('.');
                if (dot != -1)
                    newText = newText.Substring(0,dot);
                dropdown.options.Add (new TMP_Dropdown.OptionData() {text=newText});
            }
        }
    }

    // multi-select onChange event
    public void onChange() {

        if (!multiSelect) return;

        string theText = dropdown.options[(dropdown.value)].text;
        if (!theText.EndsWith(" *")) {
            theList.Add(theText);
            dropdown.options[dropdown.value].text = theText + " *";
        } else {
            dropdown.options[dropdown.value].text = theText.Substring(0, theText.Length-2);
            theList.Remove(theText);
            
        }
        dropdown.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "--";
    }
}
