// Holden Ernest - 3/19/2023
// The A* pathing algorithm to allow AI to move around

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathing : MonoBehaviour {

    private PathGrid grid;

    // decrease for less lag but less hits on target
    public int allowedItterations = 100;
    public int gridDiameter = 21;

    void Start() {
        grid = GetComponent<PathGrid>();
        grid.setRange(gridDiameter);
    }
    void Update() {
        
        if (Input.GetMouseButtonDown(2)) {
            Vector3 pos = Input.mousePosition;
            pos.z = 10;
            pos = Camera.main.ScreenToWorldPoint(pos);
            Vector3 pos2 = grid.player.transform.position;
            //Debug.Log("mouse at: " + pos.x + "," +  pos.y);
            traceBack(getPath(new Vector2(pos2.x, pos2.y - 0.5f), new Vector2(pos.x,pos.y)));
        }
    }

    // the method called by the AI, passing through the start and end nodes gotten from an image translation
    public PathNode getPath(Vector2 startPos, Vector2 endPos) {

        grid.createGrid(startPos);
        PathNode startNode = grid.worldPosToNode(startPos);
        PathNode endNode = grid.worldPosToNode(endPos);
        if (endNode.unwalkable) {
            Debug.Log("Target point for path is an object!");
            return startNode;
        }
        List<PathNode> openNodes = new List<PathNode>(); // make these binary tree heaps instead for better performance (keep sorted by lowest cost)
        openNodes.Add(startNode);
        int itterations = 0;
        while (openNodes.Count > 0) {
            if (++itterations > allowedItterations) break;
            Debug.Log(grid.printList(openNodes));
            PathNode current = grid.lowestCost(openNodes);
            grid.removeFromList(openNodes,current);
            current.setClosed();
            if (current.isEqual(endNode)) {
                return current;
            }
            
            List<PathNode> neighbors = grid.getNeighbors(current);
            foreach (PathNode node in neighbors) {
                //if (node.isNull) return null; // TEMP variable, if any node goes out of the array, return
                if (node.unwalkable || node.isClosed()) // skip walls and any nodes that have already been picked
                    continue;

                // new total cost for the node
                int newCostToStart = current.startCost + current.getDistanceTo(node);
				if (newCostToStart < node.startCost || !grid.listContains(openNodes,node)) {
                    // if this new neighbor is a better path towards the end
					node.startCost = newCostToStart;
					node.endCost = node.getDistanceTo(endNode);
					node.setParent(current);

					if (!grid.listContains(openNodes,node)) { // only add
						openNodes.Add(node);
                    }
				}
            }
        }
        return endNode;
    }

    public void traceBack(PathNode endNode) {

        try {
            PathNode p = endNode.getParent();
            Debug.DrawLine(new Vector2(endNode.position.x + 0.5f, endNode.position.y + 0.5f), new Vector2(p.position.x + 0.5f, p.position.y + 0.5f), Color.red, 0.8f);
            traceBack(p);
        } catch (Exception e) {
            return;
        }
    }

}
