using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterTile : Tile {
    
    public CharacterCreator charBase;

    public CharacterTile (CharacterCreator c) {
        charBase = c;
    }

}
