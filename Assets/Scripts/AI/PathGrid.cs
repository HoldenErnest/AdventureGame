// Holden Ernest - 3/19/2023
// Convert world Vector2 to a Pathode in which the A* algorithm can compute on
// Checked by taking the collidable image in a 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathGrid : MonoBehaviour {
    
    // nodes to keep track of neighbors
    private PathNode[,] nodes;

    private int range = 21; // x tiles to check around the player
    private Vector2Int middlePos;

    public GameObject player; // TEMP!!!!!!!!!!!! for testing coords
    private MapFromImage objectMap; // the image

    void Start() {
        objectMap = GetComponent<MapFromImage>();
    }
    public void createGrid(Vector2 center) {
        //range = newRange;
        nodes = new PathNode[range, range];
        Vector2Int centerInt = new Vector2Int((int)Mathf.FloorToInt(center.x), (int)Mathf.FloorToInt(center.y));
        middlePos = centerInt;
    }

    // adds a node to the array
    public PathNode worldPosToNode(Vector2 pos) { // ENTER THE POSITION OF THE FOOT HITBOX
        Vector2Int nodePos = new Vector2Int((int)Mathf.FloorToInt(pos.x), (int)Mathf.FloorToInt(pos.y));
        bool isWalkable = objectMap.activeTile(nodePos);
        // return the new node, or a node that is already in its place
        return getFromGrid(new PathNode(nodePos, isWalkable));
    }

    /* returns all Nodes around x,y in the array
    0 1 2
    3 4 5
    6 7 8
    */
    public List<PathNode> getNeighbors(PathNode p) {
        List<PathNode> neighbors = new List<PathNode>();
        for (int i = 0; i < 9; i++) {
            if (i != 4) {
                int nX = (i%3) - 1 + p.position.x; // -1, 0, 1
                int nY = Mathf.FloorToInt(i/3) - 1 + p.position.y; // -1, 0, 1
                Vector2Int v = new Vector2Int(nX, nY);
                if (isOnGrid(v)) {
                    PathNode neighborPath = getFromGrid(new PathNode(v, objectMap.activeTile(v)));
                    if (!neighborPath.isClosed())
                        neighbors.Add(neighborPath);
                }
                
            }
        }
        return neighbors;
    }

    // takes in a node and retrieves the node in that place(so the old ones values arent replaced), otherwise it places it in the grid based off its position
    private bool isOnGrid(Vector2Int v) { // takes world pos
        Vector2Int gridV = gridCoords(v);

        if (gridV.x == -1 && gridV.y == -1) {
            return false;
        }
        return true;
    }
    private Vector2Int gridCoords(Vector2Int v) { // takes world pos and converts to grid pos
        int x = (v.x - middlePos.x) + Mathf.CeilToInt(range/2);
        int y = (v.y - middlePos.y) + Mathf.CeilToInt(range/2);

        if (x >= range || x < 0 || y >= range || y < 0) {
            Debug.Log("POSITION [" + x + "," + y + "] NOT WITHIN GRID! " + v.x +","+ v.y);
            return new Vector2Int(-1,-1);
        }
        return new Vector2Int(x,y);
    }
    private PathNode getFromGrid(PathNode p) { // returns a path node from world pos
        if (!isOnGrid(p.position)) {
            p = new PathNode(new Vector2Int(range-1,range-1), false); // <<<<<<!! SET NODE TO CLOSEST STILL IN RANGE
            return p;
        }
        Vector2Int v = gridCoords(p.position);

        // replace the node only if there isnt already one in there
        /*if (nodes[v.x,v.y].Equals(null)) {
            Debug.Log("null, create new node at: " + v.x + ", " + v.y);
            nodes[v.x,v.y] = p;
        }*/

        // HELP!!!!!!!!!!! DONT KNOW WHY I cant tell if nodes[x,y] is null!!!!!!!!!!!!
        try {
            int no = nodes[v.x,v.y].position.x;
        } catch (Exception e) {
            nodes[v.x,v.y] = p;
        }
        return nodes[v.x,v.y];
    }

    // Returns the index for the position in the grid
    public int getHeight() {
        return range;
    }
    public int getWidth() {
        return range;
    }

    // returns the lowest costing node from a list
    public PathNode lowestCost(List<PathNode> theNodes) {
        PathNode lowestCostNode = theNodes[0];
        for (int i = 1; i < theNodes.Count; i++) {
            lowestCostNode = lowestCostNode.compareNodes(theNodes[i]);
        }
        return lowestCostNode;
    }
    public bool listContains(List<PathNode> theNodes, PathNode node) {
        foreach (PathNode n in theNodes) {
            if (n.isEqual(node)) {
                return true;
            }
        }
        return false;
    }
    public bool removeFromList(List<PathNode> theNodes, PathNode node) {
        for (int i = 0; i < theNodes.Count; i++) {
            if (theNodes[i].isEqual(node)) {
                theNodes.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}
