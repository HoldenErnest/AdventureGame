// Holden Ernest - 3/19/2023
// A node(tile with costs) for the A* pathing algorithm
// NOTE:!!: alot of built in methods dont work for comparing 2 nodes, (list.contains, node == null) use .isEqual(node) instead

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {

    public bool unwalkable; // if the node is not an object
    public Vector2Int position; // in tiles, the x and y coords (NOT POSITION IN A GRID)

    // Cost in units of (tiles * 10)
    public int startCost; // cost to the start node, traveling previous explored tiles // GCOST
    public int endCost; // cost to the end node                                        // HCOST
    private PathNode parent; // where the node came from

    private bool closed = false;
    public PathNode(){}

    public PathNode(Vector2Int pos, bool isunwalkable) {
        position = pos;
        unwalkable = isunwalkable;
        Debug.DrawLine(new Vector2(position.x, position.y), new Vector2(position.x + 1.0f, position.y + 1.0f), Color.green, 0.3f);
        Debug.DrawLine(new Vector2(position.x, position.y + 1.0f), new Vector2(position.x + 1.0f, position.y), Color.green, 0.3f);
    }

    private int getTotalCost() {
        return startCost + endCost;
    }
    public void setClosed() {
        closed = true;
        Debug.DrawLine(new Vector2(position.x + 0.5f, position.y), new Vector2(position.x + 0.5f, position.y + 1.0f), Color.red, 0.3f);
        Debug.DrawLine(new Vector2(position.x, position.y + 0.5f), new Vector2(position.x + 1.0f, position.y + 0.5f), Color.red, 0.3f);
    }
    public bool isClosed() {
        return closed;
    }
    public bool isEqual(PathNode other) {
        return (other.position == position);
    }
    public void setParent(PathNode p) {
        parent = p;
    }
    public PathNode getParent() {
        return parent;
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
    // returns the distance (int tiles * 10) to the other tile
    public int getDistanceTo(PathNode other) {
		int dX = Mathf.Abs(position.x - other.position.x);
		int dY = Mathf.Abs(position.y - other.position.y);

        // to get even numbers *10 or corners *14
		if (dX > dY)
			return 14*dY + 10* (dX-dY);
		return 14*dX + 10 * (dY-dX);
	}

    public override string ToString() {
        return $"Node at [{position}]: closed:{closed}, {getTotalCost()}";
    }
}
