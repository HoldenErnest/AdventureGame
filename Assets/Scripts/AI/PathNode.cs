// Holden Ernest - 3/19/2023
// A node(tile with costs) for the A* pathing algorithm

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {

    public bool moveable; // if the node is not an object
    public Vector2Int position; // in tiles, the x and y coords (NOT POSITION IN A GRID)

    // Cost in units of (tiles * 10)
    public int startCost; // cost to the start node, traveling previous explored tiles
    public int endCost; // cost to the end node
    public PathNode parent; // where the node came from

    public PathNode(){}

    public PathNode(Vector2Int pos, bool isMoveable) {
        position = pos;
        moveable = isMoveable;
        Debug.DrawLine(new Vector2(position.x, position.y), new Vector2(position.x + 1.0f, position.y + 1.0f), Color.green);
        Debug.DrawLine(new Vector2(position.x, position.y + 1.0f), new Vector2(position.x + 1.0f, position.y), Color.green);
    }

    private int getTotalCost() {
        return startCost + endCost;
    }

    // returns the better costing node
    public PathNode compareNodes(PathNode other) {
        int otherCost = other.getTotalCost();
        int thisCost = getTotalCost();
        if (otherCost < thisCost) {
            return other;
        }
        if (otherCost == thisCost) {
            if (other.endCost < endCost)
                return other;
        }
        return this;
    }
}
