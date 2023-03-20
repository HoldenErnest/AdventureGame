// Holden Ernest - 3/19/2023
// A node(tile with costs) for the A* pathing algorithm

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {

    private bool moveable; // if the node is not an object
    private Vector2Int position; // in tiles, the x and y coords

    // Cost in units of (tiles * 10)
    private int startCost; // cost to the start node, traveling previous explored tiles
    private int endCost; //cost to the end node

    private PathNode[] neighbors;

    public PathNode(){}

    public PathNode(Vector2Int pos, bool isMoveable) {
        position = pos;
        moveable = isMoveable;
    }

    private int getTotalCost() {
        return startCost + endCost;
    }

    public int getCost() {
        return getTotalCost();
    }
    public int getEndCost() {
        return endCost;
    }

    // returns the better costing node
    public PathNode compareNodes(PathNode other) {
        int otherCost = other.getCost();
        int thisCost = getCost();
        if (otherCost < thisCost) {
            return other;
        }
        if (otherCost == thisCost) {
            if (other.getEndCost() < endCost)
                return other;
        }
        return this;
    }

    // Activates the node, allowing it to be a potential candidate to move on. and not allowing it to be opened again
    public void closeNode() {

    }

    public void updateNode() {

    }
}
