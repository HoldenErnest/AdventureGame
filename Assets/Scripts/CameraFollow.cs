using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraFollow : MonoBehaviour {

    public Transform playerPos;
    private Transform thisPos;
    private Rigidbody2D rb;

    public float catchSpeed;
    public float minDist;


    private float zoomSpeed = 0.02f;

    private float zoomTo = 0;

    void Start() {
        thisPos = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate() {
        move();
        if (zoomTo != 0) {
            updateZoom();
        }
    }

    void move() {
        
        float distX = Math.Abs(playerPos.position.x - thisPos.position.x);
        float distY = Math.Abs(playerPos.position.y - thisPos.position.y);
        if (distX > minDist) rb.velocity = new Vector2(catchSpeed * (distX-minDist) * ((playerPos.position.x - thisPos.position.x)/distX),rb.velocity.y); // camSpeed * distanceFromPlayer * direction
        else rb.velocity = new Vector2(0, rb.velocity.y);
        if (distY > minDist) rb.velocity = new Vector2(rb.velocity.x, catchSpeed * (distY-minDist) * ((playerPos.position.y - thisPos.position.y)/distY));
        else rb.velocity = new Vector2(rb.velocity.x, 0);
    }
    private void updateZoom() {
        float currentSize = this.gameObject.GetComponent<Camera>().orthographicSize;
        float difToZoom = Math.Abs(currentSize - zoomTo);
        if (difToZoom <= 0.05f) {
            zoomTo = 0;
        }
        if (currentSize < zoomTo) {
            this.gameObject.GetComponent<Camera>().orthographicSize += zoomSpeed*difToZoom;
        } else if (currentSize > zoomTo) {
            this.gameObject.GetComponent<Camera>().orthographicSize -= zoomSpeed*difToZoom;
        }
    }

    public void zoomToNum(float zoom) {
        zoomTo = zoom;
    }
}