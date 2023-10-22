// Holden Ernest - 10/20/2023
// one of the many -FromUI scripts, take in ui inputs and convert save it to a file of certain type

using System;
using System.IO;
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
    public StatUIController statsUI;

    public ErrorMenu error;

    private CharacterCreator createCharacter() {
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
            important = importantUI.isOn,
            stats = statsUI.getFullStats()
            // TODO : STATS
        };
        return cc;
    }

    //on button press update everything and save it to whatever filename and whereever it needs to go
    public void onCreate() {
        try {
            CharacterCreator cc = createCharacter();
            saveToFile(Path.Join(Application.streamingAssetsPath, "CustomCharacters\\"), cc.id + ".json", JsonUtility.ToJson(cc));
        } catch {
            error.gameObject.SetActive(true);
            error.throwError($"There was a problem making a Character, most likely you need to fill in more input boxes (ID especially)");
        }
    }

    private void saveToFile(string path, string file, string content) {
        Debug.Log("saving: " + path + file);
        if (!File.Exists(path + file)) { // make sure it exists before creating it
            Directory.CreateDirectory(path);
            var myFile = File.Create(path + file);
            myFile.Close();
            File.WriteAllText(path + file, content);
        } else {
            error.gameObject.SetActive(true);
            error.throwError($"You are overwriting the file \"{file}\", do you want to continue?", path, file, content);
            Debug.Log("It seems like youre overwriting this Character");
        }
    }

}
