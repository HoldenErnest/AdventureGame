// Holden Ernest - 3/19/2023
// Convert world Vector2 to a Pathode in which the A* algorithm can compute on
// Checked by taking the collidable image in a 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid : MonoBehaviour {
    
    // nodes to keep track of neighbors
    private PathNode[,] nodes;

    private int range = 9; // x tiles to check around the player
    private Vector2Int middlePos;

    public GameObject player; // TEMP!!!!!!!!!!!! for testing coords
    private MapFromImage objectMap; // the image

    void Start() {
        objectMap = GetComponent<MapFromImage>();
    }
    public void createGrid(PathNode center) {
        //range = newRange;
        nodes = new PathNode[range, range];
        
        middlePos = center.position;
    }

    // adds a node to the array
    public PathNode worldPosToNode(Vector2 pos) { // ENTER THE POSITION OF THE FOOT HITBOX
        Vector2Int nodePos = new Vector2Int((int)Mathf.FloorToInt(pos.x), (int)Mathf.FloorToInt(pos.y));
        bool isWalkable = objectMap.activeTile(nodePos);
        PathNode p = new PathNode(nodePos, isWalkable);
        setInGrid(p);
        return p;
    }

    /* updates all Nodes around x,y in the array
    0 1 2
    3 4 5
    6 7 8
    */
    private void setNeighbors(int x, int y) {
        for (int i = 0; i < 9; i++) {
            if (i != 4) {
                int nX = (i%3) - 1 + x; // -1, 0, 1
                int nY = Mathf.FloorToInt(i/3) - 1 + y; // -1, 0, 1
                Vector2Int v = new Vector2Int(nX, nY);
                setInGrid(new PathNode(v, objectMap.activeTile(v)));
            }
        }
    }

    // takes in a node and places it in the grid based off its position
    private bool setInGrid(PathNode p) {
        Vector2Int v = p.position;
        int x = (v.x - middlePos.x) + (int)(range/2);
        int y = (v.y - middlePos.y) + (int)(range/2);

        if (x >= range || x < 0 || y >= range || y < 0) {
            Debug.Log("POSITION [" + x + "," + y + "] NOT WITHIN GRID!");
            return false;
        }

        nodes[x,y] = p;
        return true;
    }

    // Returns the index for the position in the grid
    public int getHeight() {
        return range;
    }
    public int getWidth() {
        return range;
    }

    // returns the lowest costing node from a list
    private PathNode lowestCost(List<PathNode> theNodes) {
        PathNode lowestCostNode = theNodes[0];
        foreach (PathNode n in theNodes) {
            lowestCostNode = lowestCostNode.compareNodes(n);
        }
        return lowestCostNode;
    }
}
