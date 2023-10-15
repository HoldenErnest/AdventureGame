// Holden Ernest - 10/15/2023
// A small script to display a character icon based on an input index

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconViewer : MonoBehaviour
{
    public UnityEngine.UI.Image image;

    private TMP_InputField textInput;

    public void Start(){
        textInput = GetComponent<TMP_InputField>();
        textInput.onValueChanged.AddListener (setIcon);
    }
    public void setIcon(string theString) {
        if (theString.Equals("")) {
            image.sprite = Knowledge.getNoCharIcon();
            return;
        }
        int index = Int32.Parse(theString);
        image.sprite = Knowledge.getCharIcon(index);

    }
}
