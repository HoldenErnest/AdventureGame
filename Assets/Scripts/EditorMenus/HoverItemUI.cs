using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    [TextArea(3,10)]
    public string description;

    public void OnPointerEnter(PointerEventData eventData){
        Camera.main.GetComponent<HoverTooltip>().setTooltip(title, description, eventData.position);
    }
    public void OnPointerExit(PointerEventData eventData){
        Camera.main.GetComponent<HoverTooltip>().hideTooltip();
    }
}
