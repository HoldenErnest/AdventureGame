using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacingStopper : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler {

    public void OnPointerEnter(PointerEventData eventData){
        EditPlacer ep;
        if (Camera.main.gameObject.TryGetComponent(out ep)) {
            ep.hoveringDisplay = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData){
        EditPlacer ep;
        if (Camera.main.gameObject.TryGetComponent(out ep)) {
            ep.hoveringDisplay = false;
        }
    }
}
