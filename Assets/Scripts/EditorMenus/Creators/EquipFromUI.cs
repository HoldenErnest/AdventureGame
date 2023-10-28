// Holden Ernest - 10/28/2023
// one of the many -FromUI scripts, take in ui inputs and convert save it to a file of certain type

using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipFromUI : MonoBehaviour {

    public TMP_InputField nameUI;
    public TMP_InputField typeUI;
    public TMP_InputField descUI;
    public TMP_InputField altIconUI;
    public Slider dropChanceUI;
    public TMP_Dropdown skillUI;
    public TMP_InputField damageUI;

    public ErrorMenu error; // error object that will show when you have problems with object creation

    private Equipable createItem() {
        Equipable itm = new Equipable{
            itemName = nameUI.text,
            itemType = typeUI.text,
            description = descUI.text,
            altIcon = altIconUI.text,
            dropChance = (float)dropChanceUI.value, // (float)Math.Round(dropChanceUI.value, 2) // this doesnt work?
            skillName = skillUI.options[skillUI.value].text,
            damage = Int32.Parse(damageUI.text)
        };

        return itm;
    }

    public void onCreate() { // Event, the save button
        try {
            Equipable itm = createItem();
            saveToFile(Path.Join(Application.streamingAssetsPath, "CustomEquips\\"), itm.itemName + ".json", JsonUtility.ToJson(itm));
        } catch {
            error.gameObject.SetActive(true);
            error.throwError($"There was a problem making an Equip, You might need to fill in more boxes");
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
            Debug.Log("It seems like youre overwriting this Item");
        }
    }
}
