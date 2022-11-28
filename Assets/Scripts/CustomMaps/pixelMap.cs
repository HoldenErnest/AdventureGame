using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[System.Serializable]
public class pixelMap {
    public Color color;
    public string type = "single"; // single, random, connected
    public TileBase[] tiles;

}