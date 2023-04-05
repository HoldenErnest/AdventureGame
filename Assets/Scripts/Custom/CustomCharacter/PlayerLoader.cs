// Holden Ernest 4/2/2023
// save and load for the players Inventory (becasuse Quests and learnedskills are only for player why not just save seperatly)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class PlayerLoader {

    // set the players inventory
    public static void loadInventory() {
        Knowledge.player.inventory = Knowledge.getInvSave();
        Knowledge.player.inventory.loadLists();
    }

    public static void overwrite() {
        string path = Knowledge.getSavePath() + Knowledge.playerPath; // something/Saves/save0/Player/player.json
        string content = Knowledge.player.inventory.overwriteLists();
        saveToFile(path, "inventory.json", content);
        string cc = Knowledge.characterToJson(Knowledge.player.toBlueprint());
        saveToFile(path, "player.json", cc);
        
        // save to someplace/PlayerSave/inventory.json
        // Player characterCreator is saved to someplace/PlayerSave/player.json
    }

    private static void saveToFile(string path, string file, string content) {
        if (!File.Exists(path + file)) { // make sure it exists before creating it
            Directory.CreateDirectory(path);
            var myFile = File.Create(path + file);
            myFile.Close();
        }

        File.WriteAllText(path + file, content);
    }
}
