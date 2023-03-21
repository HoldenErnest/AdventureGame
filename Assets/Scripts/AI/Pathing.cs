// Holden Ernest - 3/19/2023
// The A* pathing algorithm to allow AI to move around

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour {

    private PathGrid grid;

    

    void Start() {
        grid = GetComponent<PathGrid>();
        Vector3 pos = Input.mousePosition;
        pos.z = 10;
        pos = Camera.main.ScreenToWorldPoint(pos);
        Vector3 pos2 = grid.player.transform.position;
        getPath(new Vector2(pos2.x, pos2.y - 0.5f), new Vector2(-8.0f,4.0f));
    }
    void Update() {
        
    }

    // the method called by the AI, passing through the start and end nodes gotten from an image translation
    public PathNode getPath(Vector2 startPos, Vector2 endPos) {

        grid.createGrid(startPos);
        PathNode startNode = grid.worldPosToNode(startPos);
        PathNode endNode = grid.worldPosToNode(endPos);
        List<PathNode> openNodes = new List<PathNode>(); // make these binary tree heaps instead for better performance (keep sorted by lowest cost)
        openNodes.Add(startNode);

        while (openNodes.Count > 0 && openNodes.Count < 20) {
            PathNode current = grid.lowestCost(openNodes);
            openNodes.Remove(current);
            current.setClosed();
            if (current.isEqual(endNode)) {
                return current;
            }
            
            PathNode[] neighbors = grid.getNeighbors(current);
            foreach (PathNode node in neighbors) {
                if (node.moveable || node.isClosed())
                    continue;

                // new total cost for the node
                int newCostToNeighbour = current.startCost + current.getDistanceTo(node);
				if (newCostToNeighbour < node.startCost || !openNodes.Contains(node)) {
					node.startCost = newCostToNeighbour;
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
