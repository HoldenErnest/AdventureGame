// Holden Ernest - 10/29/2023
// your standard really bad UI to object script :(

using System;
using System.IO;
using TMPro;
using UnityEngine;

public class EffectFromUI : MonoBehaviour {

    public TMP_InputField typeUI;
    public TMP_InputField tierUI;
    public TMP_InputField damageUI;
    public TMP_InputField durrationUI;

    public ErrorMenu error; // error object that will show when you have problems with object creation

    private Effect createItem() {
        Effect itm = new Effect{
            effectType = typeUI.text,
            damage = Int32.Parse(damageUI.text),
            effectTier = Int32.Parse(tierUI.text),
            duration = Int32.Parse(durrationUI.text)
        };

        return itm;
    }

    public void onCreate() { // Event, the save button
        try {
            Effect itm = createItem();
            saveToFile(Path.Join(Application.streamingAssetsPath, "CustomEffects\\"), itm.effectType + itm.effectTier + ".json", JsonUtility.ToJson(itm));
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
