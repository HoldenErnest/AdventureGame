using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hotbar : MonoBehaviour
{
    
    float totalReload;
    private bool reloading = false;
    private float startTime = 0;
    GameObject child;

    void Start() {
        child = this.gameObject.GetComponent<Transform>().GetChild(1).gameObject;
    }
    void FixedUpdate() {
        if (reloading) {
            float yVal = 1.0f-((Time.time-startTime)/totalReload);
            if (yVal <= 0.0f) {
                yVal = 0.01f;
                reloading = false;
            }
            child.GetComponent<Transform>().localScale = new Vector3(1.0f, yVal, 1.0f);
            
        }
    }

    public void updateIcon(Texture2D tex) {
        try {
            GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 32f);
        } catch (Exception e) {
            Debug.Log("cant set hotbar icon");
        }
    }

    public void startReload(float reload) {
        totalReload = reload;
        startTime = Time.time;
        reloading = true;
    }
}
