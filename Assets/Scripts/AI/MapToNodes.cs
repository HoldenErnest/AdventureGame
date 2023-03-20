// Holden Ernest - 3/19/2023
// Convert world Vector2 to a Pathode in which the A* algorithm can compute on
// Checked by taking the collidable image in a 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToNodes : MonoBehaviour {
    
    public GameObject player; // TEMP!!!!!!!!!!!! for testing coords
    public MapFromImage objectMap;

    void Start() {
        Pathing.player = player;
        Pathing.objectMap = objectMap;
    }

}
