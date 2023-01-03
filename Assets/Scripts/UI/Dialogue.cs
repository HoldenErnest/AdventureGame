using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;
using System.IO;
using System;

public class Dialogue : MonoBehaviour
{
    public static TextMeshProUGUI textUI;
    public static DialogueXML dialogue;
    public static int dialogueItemNum = 0;
    public static Character player;

    void Start() {
        textUI = gameObject.GetComponent<TextMeshProUGUI>();
        startNewDialogue("intro1");
        player = Knowledge.player;
    }

    public static void startNewDialogue (string name) { // open a new dialogue from file
        dialogue = ImportXml<DialogueXML>(name);
        dialogueItemNum = 0;
        setDialogueBox(0);
    }
    private static void setDialogueBox(int num) { // sets the actual text from one of the dialogue items
        
        textUI.text = $"{dialogue.dialogueItems[num].speaker} \n  {escapeVars(dialogue.dialogueItems[num].text)}";
        
    }
    private static string escapeVars(string txt) { // escape variables
        string l;
        l = txt.Replace("{player.name}", "Guy");
        l = l.Replace("{player.title}", "the brainless");
        return l;
    }

    public static void next() { // updates text to the next dialogue item in the file
        if (dialogueItemNum < dialogue.dialogueItems.Length-1) {
            dialogueItemNum++;
            setDialogueBox(dialogueItemNum);
        } else {
            textUI.text = "";
        }
    }
    
    private static T ImportXml<T>(string path) {
        try {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream("Assets/Resources/Dialogue/" + path + ".xml", FileMode.Open)) {
                return (T)serializer.Deserialize(stream);
            }
        } catch (Exception e) {
            Debug.LogError("Exception importing xml file: " + e);
            return default;
        }
    }

}

