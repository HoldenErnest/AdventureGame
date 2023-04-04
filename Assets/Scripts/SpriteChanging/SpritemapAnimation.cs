using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SpritemapAnimation : MonoBehaviour {

    private SpriteRenderer sr;
    private Sprite[,] sprites;
    private readonly int rowLengths = 5;
    private Texture2D texture;

    public int catagory = 0; // the row to access
    public int index = 0; // the collumn >> what sprite to access from the current catagory

    private int lastC = -1;
    private int lastI = -1;

    void Start() {
        
    }
    void Update() {
        if (catagory != lastC || index != lastI)
            updateSprite();
    }

    public void updateSprite() {
        sr.sprite = getSprite(catagory, index);
        
    }
    public Sprite getCurrentSprite() {
        return sr.sprite;
    }
    public Sprite getBaseSprite() {
        return getSprite(0,0);
    }

    public Sprite getSprite(int c, int i) {
        //Debug.Log($"new rect: {i}, {c} last: {lastI}, {lastC}"); //v 288 = texture.height
        Sprite sp = Sprite.Create(texture, new Rect(i*32, (288-32)-(c*32), 32, 32), new Vector2(0.5f, 0.5f), 32f);
        try {
            sr.sprite = sp;
        } catch (Exception e) {
            //sagnas
        }   
        lastC = catagory;
        lastI = index;
        return sp;
    }

    public void resetSprite() {
        catagory = 0;
        index = 0;
        updateSprite();
    }

    public void updateTexture(Texture2D tex) { // when the actual body texture changes, change the sprites
        if (sr == null)
            sr = gameObject.GetComponent<SpriteRenderer>();
        texture = tex;
        resetSprite();
    }

}
