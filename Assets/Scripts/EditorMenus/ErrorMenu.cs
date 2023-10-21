// Holden Ernest - 10/21/2023
// For the bad UI, show gameobject when you mightve made a mistake, like overwrite a file

using System;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ErrorMenu : MonoBehaviour {

    public TMP_Text messageText;
    public UnityEngine.UI.Button acceptButton; // accept whatever problem mightve happened ( right now it has to be an overwrite problem)

    private string path, file, content;

    public void throwError(string message) {
        setText(message);
        setButton(false);
    }
    public void throwError(string message, string p, string f, string c) {
        setText(message);
        setButton(true);
        path = p;
        file = f;
        content = c;
    }

    private void setText(string m) {
        messageText.text = m;
    }
    private void setButton(bool b) {
        acceptButton.gameObject.SetActive(b);
    }

    public void onAccept() { // event - when the accept button is pressed
        saveAnyway(path, file, content);
        gameObject.SetActive(false);
    }
    public void onClose() {
        gameObject.SetActive(false);
    }

    private void saveAnyway(string path, string file, string content) {
        Debug.Log("overwriting " + path + file);
        File.WriteAllText(path + file, content);
    }



}
