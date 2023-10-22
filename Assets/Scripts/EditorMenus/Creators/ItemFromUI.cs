// Holden Ernest - 10/22/2023
// The base script to make an Item from a bunch of UI items.

using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemFromUI : MonoBehaviour {

    public TMP_InputField nameUI;
    public TMP_InputField descUI;
    public TMP_InputField altIconUI;
    public TMP_InputField consumeUI;
    public Slider dropChanceUI;

    public ErrorMenu error; // error object that will show when you have problems with object creation

    private Item createItem() {
        Item itm = new Item{
            itemName = nameUI.text,
            description = descUI.text,
            altIcon = altIconUI.text,
            consumeEffect = consumeUI.text,
            dropChance = (float)dropChanceUI.value// (float)Math.Round(dropChanceUI.value, 2) // this doesnt work?
        };

        return itm;
    }

    public void onCreate() { // Event, the save button
        try {
            Item itm = createItem();
            saveToFile(Path.Join(Application.streamingAssetsPath, "CustomItems\\"), itm.itemName + ".json", JsonUtility.ToJson(itm));
        } catch {
            error.gameObject.SetActive(true);
            error.throwError($"There was a problem making an Item, You might need to fill in more boxes");
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
