// Holden Ernest - 3/19/2023
// The A* pathing algorithm to allow AI to move around

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathing {

    // nodes to keep track of neighbors
    private static PathNode[,] nodes;

    private static List<PathNode> openNodes = new List<PathNode>();
    private static List<PathNode> closedNodes = new List<PathNode>();

    private static int range = 10; // x tiles to check around the player
    private static Vector2Int startPosition;

    public static GameObject player; // TEMP!!!!!!!!!!!! for testing coords
    public static MapFromImage objectMap;

    // PUBLIC METHODS!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // the method called by the AI, passing through the start and end nodes gotten from an image translation
    public static void getPath(Vector2Int startPos, Vector2Int endPos) {
        startPosition = startPos;
        PathNode startNode = worldPosToNode(startPos);
        PathNode endNode = worldPosToNode(endPos);
    }
    public static void setRadius(int rad) {
        range = rad;
        initArray();
    }
    // END PUBLIC METHODS!!!!!!!!!!!!!!!!!!!!!!!!!

    private static void initArray() {
        nodes = new PathNode[range, range];
    }

    // adds a node to the array
    public static PathNode worldPosToNode(Vector2 pos) { // ENTER THE POSITION OF THE FOOT HITBOX
        Vector2Int nodePos = new Vector2Int((int)Mathf.FloorToInt(pos.x), (int)Mathf.FloorToInt(pos.y));
        bool isWalkable = objectMap.activeTile(nodePos);
        Vector2Int arrayPos = worldPointToArray(nodePos);
        // try to add the node to the 2Darray
        
        return new PathNode(nodePos, isWalkable);
    }

    /* updates all Nodes around x,y in the array
    0 1 2
    3 4 5
    6 7 8
    */
    private static void setNeighbors(int x, int y) {
        for (int i = 0; i < 9; i++) {
            if (i != 4) {
                int nX = (i%3) - 1 + x; // -1, 0, 1
                int nY = Mathf.FloorToInt(i/3) - 1 + y; // -1, 0, 1
                if (nodes[nX,nY] == null) {
                    // node needs to be opened
                    Vector2Int v = new Vector2Int(nX, nY);
                    nodes[nX,nY] = new PathNode(v, objectMap.activeTile(v));
                }
            }
        }
    }

    // Returns the index for the position in the grid
    private static Vector2Int worldPointToArray(Vector2Int v) {
        int tx = v.x + (int)(getWidth()/2);
        int ty = v.y + (int)(getHeight()/2);
        tx = startPosition.x - tx;
        ty = startPosition.y - ty;
        //outside of array limits for some reason
        if (tx < 0 || tx >= getWidth()) return new Vector2Int(0,0);
        if (ty < 0 || ty >= getHeight()) return new Vector2Int(0,0);

        return new Vector2Int(tx,ty);
    }
    public static int getHeight() {
        return range;
    }
    public static int getWidth() {
        return range;
    }

    // returns the lowest costing node from a list
    private static PathNode lowestCost(PathNode[] theNodes) {
        PathNode lowestCostNode = theNodes[0];
        foreach (PathNode n in theNodes) {
            lowestCostNode = lowestCostNode.compareNodes(n);
        }
        return lowestCostNode;
    }

}
