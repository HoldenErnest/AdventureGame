using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollZoom : MonoBehaviour {
    
    private bool canScroll = true;
    private float minFov = 6f;
    private float maxFov = 90f; 
    private float sensitivity = 10f;
 
    void Update () {
        if (!canScroll) {
            return;
        }
        float fov = Camera.main.orthographicSize;
        fov += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.orthographicSize = fov;
    }

    public void setCanScroll(bool b) {
        canScroll = b;
    }
}
