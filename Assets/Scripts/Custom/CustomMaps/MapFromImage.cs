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

    void Start()
    {
        if (System.IO.File.Exists(Application.streamingAssetsPath + $"/custom{layer}.png")) {
            Texture2D tex = new Texture2D(2,2);
            byte[] a = System.IO.File.ReadAllBytes (Application.streamingAssetsPath + $"/custom{layer}.png");
            tex.LoadImage(a);
            
            img = tex;
        }

        map = this.GetComponent<Tilemap>();
        this.transform.position = new Vector3(((int)(img.width/2))*-1,((int)(img.height/2))*-1,0);
        generateMap();
    }

    private void generateMap() {
        foreach (pixelMap ptile in tiles) {
            for (int x = 0; x < img.width; x++) {
                for (int y = 0; y < img.height; y++) {
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

    public TileBase retrieveTile(int x, int y, pixelMap pixel) { // retrieves the tile needed based on the tile type
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
}
