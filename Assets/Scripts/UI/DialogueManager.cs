using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;
using System.IO;
using System;

public class DialogueManager : MonoBehaviour
{
    public static TextMeshProUGUI textUI;
    public static DialogueXML dialogue;
    public static int dialogueItemNum = 0;

    private static bool active = false;

    void Start() {
        textUI = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public static void startNewDialogue (string name) { // open a new dialogue from file
        if (active) return;
        active = true;
        dialogue = ImportXml<DialogueXML>(name);
        if (dialogue == null) {
            active = false;
            return;
        }
        dialogueItemNum = 0;
        setDialogueBox(0);
    }
    private static void setDialogueBox(int num) { // sets the actual text from one of the dialogue items
        
        textUI.text = $"{dialogue.dialogueItems[num].speaker} \n  {escapeVars(dialogue.dialogueItems[num].text)}";
        
    }
    private static string escapeVars(string txt) { // escape variables
        string l;
        l = txt.Replace("{player.name}", Knowledge.player.name);
        l = l.Replace("{player.title}", Knowledge.player.title);
        return l;
    }

    // once the dialogue is in, you cant get any more dialogue until you finish this one.
    public static void next() { // updates text to the next dialogue item in the file
        if (dialogueItemNum < dialogue.dialogueItems.Length-1) {
            dialogueItemNum++;
            setDialogueBox(dialogueItemNum);
        } else {
            textUI.text = "";
            active = false;
        }
    }
    
    private static T ImportXml<T>(string path) {
        try {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream("Assets/Resources/" + path + ".xml", FileMode.Open)) {
                return (T)serializer.Deserialize(stream);
            }
        } catch (Exception e) {
            Debug.Log("Exception importing xml file: (File might not have been found)" + path);
            return default;
        }
    }

    public static bool isActive() {
        return active;
    }

}

