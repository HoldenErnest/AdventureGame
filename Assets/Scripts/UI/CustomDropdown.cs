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

    private List<string> theList = new List<string>();
    void Start() {
        dropdown = GetComponent<TMP_Dropdown>();
        updateOptions();
        if (multiSelect)
            dropdown.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "--";
    }

    private void updateOptions() {
        switch (option) {
            case "bodies":
                try {
                    list = Directory.GetFiles("Assets/Resources/" + "Textures/Bodies/");
                } catch (Exception e) {
                    Debug.Log(e);
                }
                break;
            case "items":
                try {
                    list = Directory.GetFiles("Assets/Resources/" + "SavedObjects/Items/");
                } catch (Exception e) {
                    Debug.Log(e);
                }
                break;
            case "equips":
                try {
                    list = Directory.GetFiles("Assets/Resources/" + "SavedObjects/Equips/");
                } catch (Exception e) {
                    Debug.Log(e);
                }
                break;
            case "skills":
                try {
                    list = Directory.GetFiles("Assets/Resources/" + "SavedObjects/Skills/");
                } catch (Exception e) {
                    Debug.Log(e);
                }
                break;
                
        }
        dropdown.options.Clear ();
        foreach (string t in list) {
            if (!t.EndsWith(".meta")) {
                string[] a = t.Split('/');
                dropdown.options.Add (new TMP_Dropdown.OptionData() {text=a[a.Length - 1]});
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
