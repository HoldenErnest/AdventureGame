using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ColorChanger : MonoBehaviour {

    // the colors to swap between;
    public Color[] colors;

    public float delay = 0.15f; // in seconds, time between each pass
    public float threshhold = 0.01f; // if within 1% of target color, move to next target
    public float multiplier = 0.01f; // move 1% closer to target color on each pass

    private Color currentColor;
    public int targetColorIndex;

    private Image ir;
    private SpriteRenderer sr;

    //private Coroutine running;

    void Start() {
        currentColor = colors[0];
        ir = GetComponent<Image>();
        sr = GetComponent<SpriteRenderer>();
        setColor(currentColor);
        StartCoroutine("delayColorChange");
    }

    /*
    void FixedUpdate() {
        if (running == null) {
            running = StartCoroutine("delayColorChange");
        }
    }
    */

    private IEnumerator delayColorChange () {
        changeColor(ref currentColor, colors[targetColorIndex]);
        yield return new WaitForSeconds(delay);
        StartCoroutine("delayColorChange");
    }

    // changes current to a new Color slightly changed towards the target color
    private void changeColor (ref Color current, Color target) {
        if (similarColors(current, target)) {
            changeTargetColor();
            return;
        }
        // change in the elements
        float r = differentByPercentage(current.r, target.r);
        float g = differentByPercentage(current.g, target.g);
        float b = differentByPercentage(current.b, target.b);
        float a = differentByPercentage(current.a, target.a);

        current = new Color(r, g, b, a);

        //update the color
        setColor(currentColor);
    }
    // the .Equals() for colors
    private bool similarColors(Color a, Color b) {
        float threshhold = 0.01f;
        if (a.r < b.r - threshhold || a.r > b.r + threshhold) return false;
        if (a.g < b.g - threshhold || a.g > b.g + threshhold) return false;
        if (a.b < b.b - threshhold || a.b > b.b + threshhold) return false;
        return true;
    }
    private void changeTargetColor() { // changes the target color to the next color
        if (targetColorIndex < colors.Length - 1)
            targetColorIndex++;
        else targetColorIndex = 0;
    }
    // calculates and returns a difference in the two 
    private float differentByPercentage(float current, float target) {
        return Mathf.Lerp(current, target, multiplier); // get multiplier% closer
    }
    // update the actual color to the image or sprite
    private void setColor(Color c) {
        if (ir != null) {
            ir.color = c;
        } else if (sr != null) {
            sr.color = c;
        } else {
            Debug.Log("Color Component for ColorChanger object " + gameObject.name + " not found!");
        }
    }
}
