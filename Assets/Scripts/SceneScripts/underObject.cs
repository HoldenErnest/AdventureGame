using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class underObject : MonoBehaviour
{
    
    public float smallAlpha = 0.0f;
    private float bigAlpha;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bigAlpha = sr.color.a;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Character>() != null) {
            if (col.gameObject.GetComponent<Character>().isPlayer()) {
                fadeOut();
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Character>() != null) {
            if (col.gameObject.GetComponent<Character>().isPlayer()) {
                fadeIn();
            }
        }
    }
    private void fadeOut() {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, smallAlpha);
    }
    private void fadeIn() {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, bigAlpha);
    }
}
