using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapFromImage : MonoBehaviour
{
    public Texture2D img;
    public pixelMap[] tiles;
    public int layer;

    private Tilemap map;

    private int width = 0;
    private int height = 0;

    void Start() {   

        width = img.width;
        height = img.height;

        if (System.IO.File.Exists(Application.streamingAssetsPath + $"/custom{layer}.png")) {
            Texture2D tex = new Texture2D(2,2);
            byte[] a = System.IO.File.ReadAllBytes (Application.streamingAssetsPath + $"/custom{layer}.png");
            tex.LoadImage(a);
            
            img = tex;
        }

        map = this.GetComponent<Tilemap>();
        this.transform.position = new Vector3(((int)(width/2))*-1,((int)(height/2))*-1,0);
        generateMap();
    }

    // Generate all tiles on the image
    private void generateMap() {
        foreach (pixelMap ptile in tiles) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    generateTile(ptile, x,y);
                }
            }
        }
    }
    private void generateTile(pixelMap ptile, int x, int y) {
        Color curPixel = img.GetPixel(x,y);
        if (curPixel.a == 0) return;
            if (ptile.color.Equals(curPixel)) {
                map.SetTile(new Vector3Int(x,y,0), retrieveTile(x,y,ptile));
            }


    }

    // retrieves the tile needed based on the tile type
    public TileBase retrieveTile(int x, int y, pixelMap pixel) {
        switch (pixel.type){
        case "single":
            return pixel.tiles[0];
            break;
        case "random":
            return pixel.tiles[Random.Range(0,pixel.tiles.Length-1)];
            break;
        }
        return null;
    }

    // Returns if the tile at x,y is active
    public bool activeTile(Vector2Int v) {
        int tx = v.x + (int)(getWidth()/2);
        int ty = v.y + (int)(getHeight()/2);
        
        if (tx < 0 || tx >= getWidth()) return false;
        if (ty < 0 || ty >= getHeight()) return false;

        Color pix = img.GetPixel(tx,ty);
        //Debug.Log(v + ": " + (pix != Color.clear) + "[" + tx + ", " + ty + "]");
        return (pix != Color.clear);
    }
    public int getHeight() {
        return height;
    }
    public int getWidth() {
        return width;
    }
}
