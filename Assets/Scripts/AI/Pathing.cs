// Holden Ernest - 3/19/2023
// The A* pathing algorithm to allow AI to move around

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour {

    private PathGrid grid;

    private List<PathNode> openNodes = new List<PathNode>(); // make these binary tree heaps instead for better performance (keep sorted by lowest cost)
    private List<PathNode> closedNodes = new List<PathNode>();

    void Start() {
        grid = GetComponent<PathGrid>();
    }
    void Update() {
        Vector3 pos = Input.mousePosition;
        pos.z = 10;
        pos = Camera.main.ScreenToWorldPoint(pos);
        Vector3 pos2 = grid.player.transform.position;
        getPath(new Vector2(pos2.x, pos2.y - 0.5f), pos);
    }

    // the method called by the AI, passing through the start and end nodes gotten from an image translation
    public void getPath(Vector2 startPos, Vector2 endPos) {
        PathNode startNode = grid.worldPosToNode(startPos);
        PathNode endNode = grid.worldPosToNode(endPos);
        grid.createGrid(grid.worldPosToNode(startPos));
    }

}
