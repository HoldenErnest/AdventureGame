using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwapper : MonoBehaviour {
    
    private Image img;
    public Sprite defaultSprite; // if cant find the sprite set it to this
    public Sprite[] sprites;
    void Awake() {
        img = GetComponent<Image>();
    }
    //set the sprite to the arrayindex
    public void setSprite(int n) {
        if (n < 0 || n >= sprites.Length) {
            img.sprite = defaultSprite;
            return;
        }
        img.sprite = sprites[n];
    }
}
