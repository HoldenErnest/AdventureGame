using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour {

    public Slider s;
    public Color low;
    public Color high;
    public TextMeshProUGUI text;

    public void updateSlider(int currentHp, int maxHp) { // update all visual aspects of the slider
        s.maxValue = maxHp;
        s.value = currentHp;
        text.text = currentHp + "/" + maxHp;
        s.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, s.normalizedValue);
    }

}
