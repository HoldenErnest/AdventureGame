using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CustomSlider : MonoBehaviour {

    public Slider s;
    public Color low;
    public Color high;

    public bool showText = true;
    public TextMeshProUGUI text;
    void Start() {
        if (!showText && text != null)
            text.gameObject.SetActive(false);
    }

    public void updateSlider(int currentValue, int maxValue) { // update all visual aspects of the slider
        s.maxValue = maxValue;
        s.value = currentValue;
        if(showText)
            text.text = currentValue + "/" + maxValue;
        s.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, s.normalizedValue);
    }

    public void updateSliderFromValues() {
        if(showText)
            text.text = Math.Round(s.value,2) + "/" + s.maxValue;
        s.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, s.normalizedValue);
    }

}
