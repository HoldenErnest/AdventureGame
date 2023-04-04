// Holden Ernest 4/2/2023
// save and load for the players Inventory (becasuse Quests and learnedskills are only for player why not just save seperatly)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class PlayerLoader {

    private static readonly string savePath = "/Saves/";

    // set the players inventory
    public static void loadInventory(int n) {
        Knowledge.player.inventory = Knowledge.getSave("save" + n);
        Knowledge.player.inventory.loadLists();
    }

    public static void overwrite(int n) {
        string file = @$"{getPath()}save{n}.json"; // something/Saves/save0.json
        string content = Knowledge.player.inventory.overwriteLists();
        File.WriteAllText(file, content);
        
        // save to someplace/PlayerSave/inventory.json
        // Player characterCreator is saved to someplace/PlayerSave/player.json
    }

    private static string getPath() {
        return Application.persistentDataPath + savePath;
    }
}
