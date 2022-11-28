using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class higherPlane : MonoBehaviour {

    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.GetComponent<Character>() != null) {
            col.gameObject.GetComponent<Character>().setHigherPlane(1);
            if (col.gameObject.name == "Player") {
                Camera.main.gameObject.GetComponent<CameraFollow>().zoomToNum(6f);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.GetComponent<Character>() != null) {
            col.gameObject.GetComponent<Character>().setHigherPlane(0);
            if (col.gameObject.name == "Player") {
                Camera.main.gameObject.GetComponent<CameraFollow>().zoomToNum(5f);
            }
        }
    }

}
