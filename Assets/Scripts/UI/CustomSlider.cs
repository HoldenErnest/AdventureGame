using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomSlider : MonoBehaviour {

    public Slider s;
    public Color low;
    public Color high;
    public TextMeshProUGUI text;

    public void updateSlider(int currentValue, int maxValue) { // update all visual aspects of the slider
        s.maxValue = maxValue;
        s.value = currentValue;
        text.text = currentValue + "/" + maxValue;
        s.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, s.normalizedValue);
    }

}
