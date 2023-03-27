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

    private PathNode startNode;
    private PathNode endNode;

    public List<PathNode> thePath = new List<PathNode>();
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
            getPath(new Vector2(pos2.x, pos2.y - 0.5f), new Vector2(pos.x,pos.y));
            formatPathList(startNode, endNode);
            printList(thePath);
        }
    }

    // sets the parent of each node resulting in a reversed path, passing through the start and end nodes gotten from an image translation
    public PathNode getPath(Vector2 startPos, Vector2 endPos) {
        thePath.Clear();
        grid.createGrid(startPos);
        startNode = grid.worldPosToNode(startPos);
        endNode = grid.worldPosToNode(endPos);
        if (endNode.unwalkable) {
            Debug.Log("Target point for path is an object!");
            return startNode;
        }
        List<PathNode> openNodes = new List<PathNode>(); // make these binary tree heaps instead for better performance (keep sorted by lowest cost)
        openNodes.Add(startNode);
        int itterations = 0;
        while (openNodes.Count > 0) {
            if (++itterations > allowedItterations) break;
            //Debug.Log(grid.printList(openNodes));
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

    //trace the path back to the start node(only works if the end nodes been found)
    //adds nodes in order to thePath list.
    private void formatPathList(PathNode nextNode, PathNode backNode) {
        PathNode current = getFarthestNode(nextNode, backNode); // the node closest to nextNode(startNode)
        if (!current.isEqual(backNode)) {
            formatPathList(current, backNode); // if you didnt format the last node, format again
            return;
        }
        Debug.Log("complete");
    }
    /*
        
    */
    // returns the farthest node thats seeable by the passed in node
    private PathNode getFarthestNode(PathNode nextNode, PathNode backNode) {
        PathNode parent = backNode.getParent();
        if (parent.isEqual(nextNode)) {
            Debug.Log("hit the back node");
            thePath.Add(backNode);
            return backNode;
        }
        Debug.Log( "distance is:" + ((float)parent.getDistanceTo(nextNode) / 10));
        Vector2 nextWorld = new Vector2(nextNode.position.x + 0.5f, nextNode.position.y + 0.5f);
        Vector2 parentWorld = new Vector2(parent.position.x + 0.5f, parent.position.y + 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(nextWorld, -(nextWorld - parentWorld).normalized, parent.getDistanceTo(nextNode) / 10, LayerMask.GetMask("Collision")); // raycast between the start node to this next node

        if ((hit.collider != null && hit.collider.tag == "collidable")) {
            Debug.Log("collsiion between next and parent");
            return getFarthestNode(nextNode, parent);
        } else { // add the furthest away node that you can see
            thePath.Add(parent);
            Debug.Log("new node in list");
            //Debug.DrawLine(new Vector2(nextNode.position.x + 0.5f, nextNode.position.y + 0.5f), new Vector2(parent.position.x + 0.5f, parent.position.y + 0.5f), Color.red, 0.8f);
            return parent;
        }
        return parent;
    }
    public void printList(List<PathNode> list) {
        if (list.Count == 0) {Debug.Log("nsoangao");}
        foreach (PathNode node in list) {
            node.drawNode();
        }
    }

}