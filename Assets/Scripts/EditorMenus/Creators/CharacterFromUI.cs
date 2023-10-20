// Holden Ernest - 10/20/2023
// one of the many -FromUI scripts, take in ui inputs and convert save it to a file of certain type

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFromUI : MonoBehaviour {

    public TMP_InputField nameUI;
    public TMP_InputField titleUI;
    public TMP_InputField descUI;
    public TMP_InputField hpUI;
    public TMP_InputField teamUI;
    public MultiSelectItems invUI;
    public MultiSelectItems equipsUI;
    public MultiSelectItems skillsUI;
    public MultiSelectItems questsUI;
    public TMP_Dropdown bodyTexUI;
    public TMP_InputField idUI;
    public Toggle importantUI;


    private string createJson() {
        CharacterCreator cc = new CharacterCreator
        {
            name = nameUI.text,
            title = titleUI.text,
            description = descUI.text,
            baseHealth = Int32.Parse(hpUI.text),
            team = Int32.Parse(teamUI.text),
            id = Int32.Parse(idUI.text),
            items = invUI.getAllItemSaves(),
            equipment = equipsUI.getAllStrings(),
            startingSkills = skillsUI.getAllStrings(),
            questsToGive = questsUI.getAllStrings(),
            bodyTexture = bodyTexUI.options[bodyTexUI.value].text,
            important = importantUI.isOn
            // TODO : STATS
        };
        return JsonUtility.ToJson(cc);
    }

    //on button press update everything and save it to whatever filename and whereever it needs to go
    public void onCreate() {
        string json = createJson();
        Debug.Log("save the bro" + json);
        // TODO: save it to streaming assets
    }

}
