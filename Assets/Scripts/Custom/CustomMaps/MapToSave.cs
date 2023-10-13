using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.AI;

public class MapToSave : MonoBehaviour
{
    public int layer;

    private Tilemap map;

    Sprite[] tileOptions; // all the different possible sprites for this map

    List<SpecialObject> specialObjects = new List<SpecialObject>();

    void Awake() {

        map = this.GetComponent<Tilemap>();
        //GenerateMap();

    }
    public void saveMap(string save) { // saves the map (based off of tiles on the tilemap)
        string filePath = Path.Combine(Application.streamingAssetsPath,"Maps");
        filePath = Path.Combine(filePath,save);
        Directory.CreateDirectory (filePath);
        filePath += "/"+layer+".txt";

        StreamWriter writer = new StreamWriter(filePath);
        
        BoundsInt bounds = map.cellBounds;

        tileOptions = getSprites();

        if (layer == 4) { // if saving objects, load into json instead

            for (int i = 0; i < specialObjects.Count; i++) {
                SpecialObject spo = specialObjects[i];
                writer.WriteLine(spo.x+","+spo.y+","+spo.type+","+spo.file+","+spo.toJson());
            }
            writer.Close();
            return;
        }

        for (int x = bounds.x; x < bounds.size.x; x++) { // loops through every tile in the map array
            for (int y = bounds.y; y < bounds.size.y; y++) {
                Tile tile = map.GetTile<Tile>(new Vector3Int(x,y,0));
                if (tile != null) {
                    string tileKey = tile.sprite.name;
                    if (layer != 5) {
                        for (int i = 0; i < tileOptions.Length; i++) { // go through all sprites it could be, find the index where this sprite is
                            if (tileOptions[i].name.Equals(tileKey)) {
                                writer.WriteLine(x+","+y+","+i);
                                break;
                            }
                        }
                    } else { // the objects save their actual savefiles
                        writer.WriteLine((x)+","+(y)+","+(tileKey.Substring(14)));
                    }
                }
            }
        }
        writer.Close();
    }

    public void setTile(float x, float y, Tile t) {
        Vector2Int v = worldToGridPoint(x,y);
        if (layer == 4) { // if its at the object layer you might need to filter out old results
            SpecialObject spo = new SpecialObject(x,y,t.gameObject);
            
            for (int i = specialObjects.Count-1; i >= 0; i--) {
                if (specialObjects[i].hasPosition(x,y)) { // this object is at the same spot as the one we want to place
                    specialObjects.RemoveAt(i);
                    Debug.Log("removing same pos object");
                    break;
                }
            }
            specialObjects.Add(spo);
        }
        map.SetTile(new Vector3Int(v.x,v.y,0), t);
    }
    public void setTile(float x, float y, Sprite s) {
        Vector2Int v = worldToGridPoint(x,y);
        map.SetTile(new Vector3Int(v.x,v.y,0), spriteToTile(s));
    }
    public void setTile(float x, float y, GameObject g) {
        Vector2Int v = worldToGridPoint(x,y); //TODO: MAKE SURE THIS POINT IS CORRECT!
        // TODO: instantiate g at position;
        Tile tempT = new Tile{gameObject = g}; // make a new tile and set its gameOBject
        setTile(x,y,tempT);
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
        file = "Maps/"+file+"/"+layer;
        generateMap(file);
    }
    private void generateMap(string file) { // load tiles from the text file
        TextAsset txt = Resources.Load(file) as TextAsset;
        if (txt==null || txt.text.Length < 1) return;
        string[] allTiles = txt.text.Split("\n");
        tileOptions = getSprites();
        for (int i = 0; i < allTiles.Length; i++) { // go through all tiles in the map
            string[] line = allTiles[i].Split(",");
            if (line != null && line.Length > 1) // make sure its a real parsable line
            try {
                if (SceneManager.GetActiveScene().name.Equals("EditorScene") && layer != 4) // if you need to place a tile (including for editing purposes)
                    generateTile(Int32.Parse(line[2]),Int32.Parse(line[0]),Int32.Parse(line[1]));
                else { // otherwise place whatever object it coorasponds to
                    if (layer == 5) {
                        CharacterCreator theChar = Knowledge.getCharBlueprint(Int32.Parse(line[2]));
                        if (!theChar.important || (theChar.homePos[0] == -0.1f && theChar.homePos[1] == -0.1f)) // if this character isnt important keep its map-specified home position
                            theChar.homePos = new float[] {(float)Int32.Parse(line[0]),(float)Int32.Parse(line[1])};//  additionally if its an important character that hasent been loaded yet, spawn it at its map-specific location
                        Character newC = theChar.createCharacter();
                        if (newC.isImportant()) {
                            Debug.Log("found an important character");
                            GameSaver.addNpc(newC);
                        }
                    } else if (layer == 4) { // objects
                        int tx = Int32.Parse(line[0]);
                        int ty = Int32.Parse(line[1]);
                        int tid = Int32.Parse(line[2]);
                        string tfile = line[3];
                        string json = allTiles[i];
                        Debug.Log("started making object");
                        for (int numCommas = 0; numCommas < 4; numCommas++) {
                            json = json.Substring(json.IndexOf(",")+1);
                        }
                        Debug.Log(json + "is the objects json ");

                        GameObject newG = Knowledge.getObject(tfile);
                        newG.transform.position = new Vector3(tx, ty, 0);
                        SpecialObject.loadComponentFromJson(newG, tid, json);
                        newG.GetComponent<SpriteRenderer>().sortingOrder = 3;
                        generateTile(newG, tx, ty); // create the object in the map

                    }
                }
            } catch (Exception e) { // catch if any specific tile or object doesnt want to load
                Debug.Log(e);
            }
        }
    }
    private void generateTile(int i, int x, int y) { // get a tile based on the index saved for the tileOptions for this specific map
        map.SetTile(new Vector3Int(x,y,0), spriteToTile(tileOptions[i]));
    }
    private void generateTile(GameObject g, int x, int y) { // get a tile based on the index saved for the tileOptions for this specific map
        map.SetTile(new Vector3Int(x,y,0), new Tile{gameObject=g});
    }
    private Tile spriteToTile (Sprite s) {
        try {
            if (System.Object.ReferenceEquals(s, null)) return null;
        } catch (Exception e) {
            Debug.Log(e);
        }
        Tile theTile = new Tile();
        theTile.sprite = s;
        
        return theTile;
    }

    private Sprite[] getSprites() {// load the sprites that should to be displayed into an array !!THIS MUST CHANGE WITH ALL TilesDisplay METHOD CHANGES !!
        switch (layer) {
            case 0:
                return Knowledge.getAllSpritesFromTexture("Ground");
            case 1:
                return Knowledge.getAllSpritesFromTexture("Walls");
            case 2:
                return Knowledge.getAllSpritesFromTexture("Detail");
            case 5:
                return Knowledge.getAllCharSprites();
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
