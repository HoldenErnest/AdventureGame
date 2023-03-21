// Holden Ernest - 3/19/2023
// Convert world Vector2 to a Pathode in which the A* algorithm can compute on
// Checked by taking the collidable image in a 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public PathNode[] getNeighbors(PathNode p) {
        PathNode[] neighbors = new PathNode[8];
        for (int i = 0; i < 9; i++) {
            if (i != 4) {
                int nX = (i%3) - 1 + p.position.x; // -1, 0, 1
                int nY = Mathf.FloorToInt(i/3) - 1 + p.position.y; // -1, 0, 1
                Vector2Int v = new Vector2Int(nX, nY);
                Debug.Log("NGBRPOS: " + (v.x - p.position.x) + ", " + (v.y - p.position.y));
                PathNode neighborPath = getFromGrid(new PathNode(v, objectMap.activeTile(v)));
                Debug.Log("MY NEIGHBOR " + i + " IS: " + neighborPath);
                if (i < 4)
                    neighbors[i] = neighborPath;
                else
                    neighbors[i-1] = neighborPath;
                
            }
        }
        return neighbors;
    }

    // takes in a node and retrieves the node in that place(so the old ones values arent replaced), otherwise it places it in the grid based off its position
    private PathNode getFromGrid(PathNode p) {
        Vector2Int v = p.position;
        int x = (v.x - middlePos.x) + (int)(range/2);
        int y = (v.y - middlePos.y) + (int)(range/2);

        if (x >= range || x < 0 || y >= range || y < 0) {
            Debug.Log("POSITION [" + x + "," + y + "] NOT WITHIN GRID!");
            x = 0;
            y = 0;
            p = new PathNode(new Vector2Int(0,0), false); // <<<<<<!! SET NODE TO CLOSEST STILL IN RANGE
            
        }
        // replace the node only if there isnt already one in there
        if (!nodes[x,y]) {
            Debug.Log(x + "," + y + "node null, making new Node");
            nodes[x,y] = p;
        }
        return nodes[x,y];
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
}
