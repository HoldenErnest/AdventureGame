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

    Sprite[] tileOptions; // all the different possible sprites for this map

    void Start() {

        map = this.GetComponent<Tilemap>();
        //GenerateMap();

    }
    public void saveMap(string file) { // saves the map and the metadata to the map
        file += layer+".txt";
        string filePath = Path.Combine(Application.streamingAssetsPath,"Maps");
        filePath = Path.Combine(filePath,file);

        StreamWriter writer = new StreamWriter(filePath);
        
        BoundsInt bounds = map.cellBounds;

        tileOptions = getSprites();

        for (int x = bounds.x; x < bounds.size.x; x++) { // loops through every tile in the map array
            for (int y = bounds.y; y < bounds.size.y; y++) {
                Tile tile = map.GetTile<Tile>(new Vector3Int(x,y,0));
                if (tile != null) {
                    string tileKey = tile.sprite.name;
                    for (int i = 0; i < tileOptions.Length; i++) { // go through all sprites it could be, find the index where this sprite is
                        if (tileOptions[i].name.Equals(tileKey)) {
                            writer.WriteLine((x)+","+(y)+","+(i));
                            break;
                        }
                    }
                }
            }
        }
        writer.Close();
    }


    public void setTile(float x, float y, Tile t) {
        Vector2Int v = worldToGridPoint(x,y);
        map.SetTile(new Vector3Int(v.x,v.y,0), t);
    }
    public void setTile(float x, float y, Sprite s) {
        Vector2Int v = worldToGridPoint(x,y);
        map.SetTile(new Vector3Int(v.x,v.y,0), spriteToTile(s));
    }
    public void removeTile(float x, float y) {
        Vector2Int v = worldToGridPoint(x,y);
        map.SetTile(new Vector3Int(v.x,v.y,0), null);
    }
    public void floodPlace(float x, float y, Sprite s) { // the fill command
        Vector2Int v = worldToGridPoint(x,y);
        map.FloodFill(new Vector3Int(v.x,v.y,0),spriteToTile(s));
    }
    public void floodPlace(float x, float y, Tile t) { // the fill command
        Vector2Int v = worldToGridPoint(x,y);
        map.FloodFill(new Vector3Int(v.x,v.y,0),t);
    }

    //LOAD MAP
    public void loadMap(string file) {
        file = "Maps/"+file+layer;
        generateMap(file);
    }
    private void generateMap(string file) {
        TextAsset txt = Resources.Load(file) as TextAsset;
        string[] allTiles = txt.text.Split("\n");
        tileOptions = getSprites();
        for (int i = 0; i < allTiles.Length; i++) { // go through all tiles in the map
            string[] line = allTiles[i].Split(",");
            try {
                generateTile(Int32.Parse(line[2]),Int32.Parse(line[0]),Int32.Parse(line[1]));
            } catch {
                Debug.Log("not a number");
            }
        }
    }
    private void generateTile(int i, int x, int y) {
        map.SetTile(new Vector3Int(x,y,0), spriteToTile(tileOptions[i]));
    }
    private Tile spriteToTile (Sprite s) {
        if (System.Object.ReferenceEquals(s, null)) return null;
        Tile theTile = new Tile();
        theTile.sprite = s;
        return theTile;
    }

    private Sprite[] getSprites() {// load the sprites that should to be displayed into an array !!THIS MUST CHANGE WITH ALL TilesDisplay METHOD CHANGES !!
        switch (layer) {
            case 0:
                return Knowledge.getAllSpritesFromTexture("Ground");
                break;
            case 1:
                return Knowledge.getAllSpritesFromTexture("Walls");
                break;
            case 2:
                return Knowledge.getAllSpritesFromTexture("Detail");
                break;
            default:
                break;
        }
        return null;
    }

    public Vector2Int worldToGridPoint(float x, float y) {
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

    // for when this map isnt the currently selected group
    public void fadeMap() {
        map.color = new Color(1f,1f,1f,0.5f);
    }
    public void unfadeMap() {
        map.color = new Color(1f,1f,1f,1f);
    }
}
