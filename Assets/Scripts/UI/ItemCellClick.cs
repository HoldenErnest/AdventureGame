//Holden Ernest - September 21, 2022

//controls when right-clicked on an item in inventory(on every ItemCell object)

using UnityEngine;
using UnityEngine.EventSystems;


 public class ItemCellClick : MonoBehaviour, IPointerClickHandler {
 
    private GameObject rcItemUI;

    public void Start() {
        rcItemUI = Knowledge.player.GetController().invUI.GetComponent<InventoryUI>().rcItemMenu; // pretty shitty way to get to the Inventory ui which I decided to only refrence in the controller class
        //I need fix ^.. find a better way to refrence the rightClick UI object(has to be static because ItemCellClick is only used in the ItemCell prefabs).
    }

    public void OnPointerClick (PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            ItemCell cellItem = this.gameObject.GetComponent<ItemCell>(); // the item right clicked
            rcItemUI.SetActive(true);
            RCMenu.setCell(cellItem);
            rcItemUI.GetComponent<RCMenu>().updateCanEquip();
            //rcItemUI.GetComponent<RCscript>().setItem(sItem);// update the button options based on what kind of object is selected.
            rcItemUI.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x,eventData.position.y,1));

            //set not active at somepoint, probably on next rightclick but that would have to be called from the Controller
        }
    }
 
 }