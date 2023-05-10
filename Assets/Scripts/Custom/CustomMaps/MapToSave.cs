using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System;

public class MapToSave : MonoBehaviour
{
    //public Texture2D img;
    public pixelMap[] tiles;
    public int layer;

    private Tilemap map;

    private Dictionary<int, string> tableToTiles = new Dictionary<int, string>();

    void Start() {

        map = this.GetComponent<Tilemap>();
        //GenerateMap();

    }
    public void saveMap(string file) { // saves the map and the metadata to the map
        file += layer;
        string metafile = file + ".meta.txt";
        file += ".txt";
        string filePath = Path.Combine(Application.streamingAssetsPath,"Maps");
        metafile = Path.Combine(filePath,metafile);
        filePath = Path.Combine(filePath,file);
        
        Dictionary<string, int> tilesTable = new Dictionary<string, int>();
        StreamWriter writer = new StreamWriter(filePath);
        
        BoundsInt bounds = map.cellBounds;
        int uniqueTiles = 0; // records the number of unique tiles to save in the metadata

        for (int x = bounds.x; x < bounds.size.x; x++) { // loops through every tile in the map array
            for (int y = bounds.y; y < bounds.size.y; y++) {
                Tile tile = map.GetTile<Tile>(new Vector3Int(x,y,0));
                if (tile != null) {
                    string tileKey = tile.sprite.name;
                    if (tilesTable.ContainsKey(tileKey)) // maps the specified tile to a number for easier storage (the metadata will contain the remap values)
                        writer.WriteLine((x)+","+(y)+","+tilesTable[tileKey]);
                    else {
                        writer.WriteLine((x)+","+(y)+","+uniqueTiles);
                        tilesTable.Add(tileKey, uniqueTiles);
                        uniqueTiles++;
                    }
                }
            }
        }
        writer.Close();
        StreamWriter metaWriter = new StreamWriter(metafile);
        foreach( KeyValuePair<string, int> kvp in tilesTable ) {
            metaWriter.WriteLine(kvp.Key+","+kvp.Value);
        }
        metaWriter.Close();
        
    }

    public void setTile(float x, float y, Sprite s) {
        Vector2Int v = worldToGridPoint(x,y);
        map.SetTile(new Vector3Int(v.x,v.y,0), spriteToTile(s));
    }
    public void floodPlace(float x, float y, Sprite s) {
        Vector2Int v = worldToGridPoint(x,y);
        map.FloodFill(new Vector3Int(v.x,v.y,0),spriteToTile(s));
    }

    //LOAD MAP
    public void loadMap(string file) {
        file = "Maps/"+file+layer;
        loadMetadata(file);
        generateMap(file);
    }
    private void loadMetadata(string file) {
        TextAsset txt = Resources.Load<TextAsset>(file+".meta");
        Debug.Log("loadig " + file+".meta");
        string[] allTiles = txt.text.Split("\n");
        for (int i = 0; i < allTiles.Length; i++) {
            string[] line = allTiles[i].Split(",");
            try {
                tableToTiles.Add(Int32.Parse(line[1]),line[0]);
            } catch {
                Debug.Log("not a number");
            }
        }
    }
    private void generateMap(string file) {
        TextAsset txt = Resources.Load(file) as TextAsset;
        string[] allTiles = txt.text.Split("\n");
        for (int i = 0; i < allTiles.Length; i++) {
            string[] line = allTiles[i].Split(",");
            try {
                generateTile(Int32.Parse(line[2]),Int32.Parse(line[0]),Int32.Parse(line[1]));
            } catch {
                Debug.Log("not a number");
            }
        }
    }
    private void generateTile(int i, int x, int y) {
        string s = tableToTiles[i];
        if (s.Equals("")) return;
        
        map.SetTile(new Vector3Int(x,y,0), retrieveTile(x,y,s));
    }

    // retrieves the tile needed based on the tile type
    public TileBase retrieveTile(int x, int y, string s) {
        Sprite[] tileOptions = getSprites(); // go through all the different sprites it could be
        for (int i = 0; i < tileOptions.Length; i++) {
            if (tileOptions[i].name.Equals(s))
                return spriteToTile(tileOptions[i]);
        }
        return null;
    }
    private Sprite[] getSprites() {
        // load the sprites that should to be displayed into an array
        switch (layer) {
            case 0:
                return Knowledge.getAllSpritesFromTexture("Ground");
                break;
            case 1:
                return Knowledge.getAllSpritesFromTexture("Walls");
                break;
            case 2:
                break;
            default:
                break;
        }
        return null;
    }

    private Tile spriteToTile (Sprite s) {
        if (System.Object.ReferenceEquals(s, null)) return null;
        Tile theTile = new Tile();
        theTile.sprite = s;
        //theTile.transform.localScale = new Vector3(2,2,1);
        //theTile.RefreshTile();
        return theTile;
    }

    private Vector2Int worldToGridPoint(float x, float y) {
        int nx = Mathf.RoundToInt(x-0.5f);// - ((int)(width/2))*-1;
        int ny = Mathf.RoundToInt(y-0.5f);// - ((int)(height/2))*-1;
        //Debug.Log(x + ", " + y + ". is mouse");
        return new Vector2Int(nx,ny);
    }
    public bool activeTile(float x, float y) {
        return activeTile(worldToGridPoint(x,y));
    }

    // Returns if the tile at x,y is active
    public bool activeTile(Vector2Int v) {
        return map.HasTile(new Vector3Int(v.x,v.y,0));
    }
}
