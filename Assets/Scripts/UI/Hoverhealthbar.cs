using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverhealthbar : Healthbar
{
    public Vector3 offset;

    void Update() {
        hoverSlider();
    }

    public void hoverSlider() {
        s.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
    public void setVisible() {
        s.gameObject.SetActive(true);
    }
    public void setInvisible() {
        s.gameObject.SetActive(false);
    }
}
