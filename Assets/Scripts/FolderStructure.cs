// Holden Ernest - 10/20/2023
// A dumb script needed to keep track of file paths in the resrouces folder
// update/save everytime you build or add a new thing

using UnityEngine;
using System;
using System.IO;

[Serializable]
public class FolderStructure {

    public string[] allQuests;

    private void updateQuests() {
        try {
            allQuests = Directory.GetDirectories(Application.dataPath + "\\Resources\\Quests");
            for (int i = 0; i < allQuests.Length; i++) {
                allQuests[i] = allQuests[i].Substring(allQuests[i].LastIndexOf("\\")+1);
            }
        } catch {
            Debug.Log("Quests not loaded!");
        }
    }

    public void save() {
        updateQuests();

        string json = JsonUtility.ToJson(this);
        saveToFile(Application.persistentDataPath + "\\","folderStructure.json", json);
    }

    private static void saveToFile(string path, string file, string content) {
        Debug.Log("saving to file " + path + file);
        if (!File.Exists(path + file)) { // make sure it exists before creating it
            Directory.CreateDirectory(path);
            var myFile = File.Create(path + file);
            myFile.Close();
        }

        File.WriteAllText(path + file, content);
    }

}
