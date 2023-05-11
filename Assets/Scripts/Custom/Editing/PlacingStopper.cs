using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacingStopper : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler {

    public void OnPointerEnter(PointerEventData eventData){
        Camera.main.gameObject.GetComponent<EditPlacer>().hoveringDisplay = true;
    }
    public void OnPointerExit(PointerEventData eventData){
        Camera.main.gameObject.GetComponent<EditPlacer>().hoveringDisplay = false;
    }
}
