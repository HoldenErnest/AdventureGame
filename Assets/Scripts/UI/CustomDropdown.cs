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
    public string option = "bodies"; // what directory to access

    private string[] list;

    public List<string> theList = new List<string>();

    void Start() {
        dropdown = GetComponent<TMP_Dropdown>();
        updateOptions();
        if (multiSelect)
            dropdown.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "--";
    }
    private string bodyPath = "Assets/Resources/Textures/Bodies/";
    private string itempath = "Assets/Resources/SavedObjects/Items/";
    private string equipPath = "Assets/Resources/SavedObjects/Equips/";
    private string skillPath = "Assets/Resources/SavedObjects/Skills/";

    private string bodyPath2 = "Assets/Resources/Textures/Bodies/";
    private string itempath2 = "Assets/Resources/SavedObjects/Items/";
    private string equipPath2 = "Assets/Resources/SavedObjects/Equips/";
    private string skillPath2 = "Assets/Resources/SavedObjects/Skills/";
    private void updateOptions() {
        switch (option) {
            case "bodies":
                try {
                    list = Directory.GetFiles(bodyPath);
                    //list.Concat(Directory.GetFiles(bodyPath2)).ToArray();
                } catch (Exception e) {
                    Debug.Log(e);
                }
                break;
            case "items":
                try {
                    list = Directory.GetFiles(itempath);
                    //list.Concat(Directory.GetFiles(itempath2)).ToArray();
                } catch (Exception e) {
                    Debug.Log(e);
                }
                break;
            case "equips":
                try {
                    list = Directory.GetFiles(equipPath);
                    //list.Concat(Directory.GetFiles(equipPath2)).ToArray();
                } catch (Exception e) {
                    Debug.Log(e);
                }
                break;
            case "skills":
                try {
                    list = Directory.GetFiles(skillPath);
                    //list.Concat(Directory.GetFiles(skillPath2)).ToArray();
                } catch (Exception e) {
                    Debug.Log(e);
                }
                break;
                
        }
        populate();
    }

    private void populate() {
        dropdown.options.Clear ();
        foreach (string t in list) {
            if (!t.EndsWith(".meta")) {
                string[] a = t.Split('/');
                string newText = a[a.Length - 1];
                if (newText.EndsWith(".json"))
                    newText = newText.Substring(0,newText.Length-5);
                if (newText.EndsWith(".png"))
                    newText = newText.Substring(0,newText.Length-4);
                dropdown.options.Add (new TMP_Dropdown.OptionData() {text=newText});
            }
        }
    }
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
