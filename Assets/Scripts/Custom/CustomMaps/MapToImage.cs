using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapToImage : MonoBehaviour
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

    }


    public void setTile(float x, float y, Sprite s) {
        //Color curPixel = img.GetPixel(x,y);
        //if (curPixel.a == 0) return;

        Vector2Int v = worldToGridPoint(x,y);
        if (withinImage(v.x, v.y)) {
            map.SetTile(new Vector3Int(v.x,v.y,0), spriteToTile(s));
        } else {
            Debug.Log("mmmmmmmmmmmm");
        }
    }

    private Tile spriteToTile (Sprite s) {
        Tile theTile = new Tile();
        theTile.sprite = s;
        //theTile.transform.localScale = new Vector3(2,2,1);
        //theTile.RefreshTile();
        return theTile;

    }

    private Vector2Int worldToGridPoint(float x, float y) {
        int nx = Mathf.RoundToInt(x-0.5f) - ((int)(width/2))*-1;
        int ny = Mathf.RoundToInt(y-0.5f) - ((int)(height/2))*-1;
        Debug.Log(x + ", " + y + ". is mouse");
        return new Vector2Int(nx,ny);
    }

    // Returns if the tile at x,y is active
    public bool activeTile(Vector2Int v) {
        int tx = v.x + (int)(getWidth()/2);
        int ty = v.y + (int)(getHeight()/2);
        
        if (!withinImage(tx,ty)) {
            return false;
        }

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
    private bool withinImage(int x, int y) {
        if (x < 0 || x >= getWidth() || y < 0 || y >= getHeight()) {
            Debug.Log("PIXEL [" + x + "," + y + "] NOT WITHIN IMAGE!");
            return false;
        }
        return true;
    }
}
