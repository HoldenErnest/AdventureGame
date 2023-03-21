// Holden Ernest - 3/19/2023
// The A* pathing algorithm to allow AI to move around

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour {

    private PathGrid grid;

    void Start() {
        grid = GetComponent<PathGrid>();
        
    }
    void FixedUpdate() {
        Vector3 pos = Input.mousePosition;
        pos.z = 10;
        pos = Camera.main.ScreenToWorldPoint(pos);
        Vector3 pos2 = grid.player.transform.position;
        Debug.Log("mouse at: " + pos.x + "," +  pos.y);
        getPath(new Vector2(pos2.x, pos2.y - 0.5f), new Vector2(pos.x,pos.y));
    }

    // the method called by the AI, passing through the start and end nodes gotten from an image translation
    public PathNode getPath(Vector2 startPos, Vector2 endPos) {

        grid.createGrid(startPos);
        PathNode startNode = grid.worldPosToNode(startPos);
        PathNode endNode = grid.worldPosToNode(endPos);
        List<PathNode> openNodes = new List<PathNode>(); // make these binary tree heaps instead for better performance (keep sorted by lowest cost)
        openNodes.Add(startNode);
        int itterations = 0;
        while (openNodes.Count > 0) {
            itterations++;
            if (itterations > 30) break;
            PathNode current = grid.lowestCost(openNodes);
            openNodes.Remove(current);
            current.setClosed();
            if (current.isEqual(endNode)) {
                return current;
            }
            
            List<PathNode> neighbors = grid.getNeighbors(current);
            foreach (PathNode node in neighbors) {
                if (node.isNull) return null; // TEMP variable, if any node goes out of the array, return
                if (node.moveable || node.isClosed())
                    continue;

                // new total cost for the node
                int newCostToStart = current.startCost + current.getDistanceTo(node);
				if (newCostToStart < node.startCost || !openNodes.Contains(node)) {
                    // if this new neighbor is a better path towards the end
                    
					node.startCost = newCostToStart;
					node.endCost = node.getDistanceTo(endNode);
					node.parent = current;

					if (!openNodes.Contains(node)) {
						openNodes.Add(node);
                    }
				}
            }
        }
        return endNode;
    }

}
