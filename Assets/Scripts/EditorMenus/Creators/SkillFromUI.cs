// Holden Ernest - 10/28/2023
// one of the many -FromUI scripts, take in ui inputs and convert save it to a file of certain type

using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillFromUI : MonoBehaviour {



    public TMP_InputField nameUI;
    public TMP_Dropdown typeUI;
    public TMP_InputField damageUI;
    public TMP_InputField ManaUI;
    public TMP_InputField reloadUI;
    public TMP_InputField maxRUI;
    public TMP_InputField minRUI;
    public TMP_InputField aoeUI;
    public TMP_InputField pierceUI;
    public TMP_InputField speedUI;
    public TMP_InputField lvlReqUI;
    public MultiSelectItems effectsUI;
    public TMP_Dropdown PrefabUI;

    public ErrorMenu error;

    private Skill createSkill() {
        Skill cc = new Skill
        {
            skillName = nameUI.text,
            damageType = typeUI.options[typeUI.value].text,
            baseDamage = Int32.Parse(damageUI.text),
            manaCost = Int32.Parse(ManaUI.text),
            reloadTime = float.Parse(reloadUI.text),
            maxRange = float.Parse(maxRUI.text),
            minRange = float.Parse(minRUI.text),
            attackArea = float.Parse(aoeUI.text),
            pierce = Int32.Parse(pierceUI.text),
            projectileSpeed = float.Parse(speedUI.text),
            levelReq = Int32.Parse(lvlReqUI.text),
            setEffects = effectsUI.getAllStrings(),
            prefabName = PrefabUI.options[PrefabUI.value].text
        };
        return cc;
    }

    //on button press update everything and save it to whatever filename and whereever it needs to go
    public void onCreate() {
        try {
            Skill cc = createSkill();
            saveToFile(Path.Join(Application.streamingAssetsPath, "CustomSkills\\"), cc.skillName + ".json", JsonUtility.ToJson(cc));
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
