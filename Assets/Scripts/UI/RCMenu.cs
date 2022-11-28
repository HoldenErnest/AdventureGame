using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RCMenu : MonoBehaviour
{
    public static Item selectedItem;
    public static ItemCell ic;


    public static void setCell(ItemCell i) {
        ic = i;
        selectedItem = ic.getItem();
    }

    public void removeItem() {
        Debug.Log("removed item " + selectedItem.itemName);
    }
    public void useItem() {
        Debug.Log("used item " + selectedItem.itemName);
    }
    public void equipItem() {
        if(isEquippable()) {
            Equipable theEquip = selectedItem as Equipable;
            if (!theEquip.isEquipped()) {
                Knowledge.player.equip((Equipable)selectedItem);
                Debug.Log("Equipped item " + selectedItem.itemName);
            } else {
                Knowledge.player.unequip((Equipable)selectedItem);
                Debug.Log("Unequipped item " + selectedItem.itemName);
            }
        } else {
            Debug.Log("cant equip item " + selectedItem.itemName);
        }
        ic.updateIsEquipped();
        updateCanEquip();
    }

    private bool isEquippable() {
        return selectedItem.GetType() == typeof(Equipable);
    }

    public void updateCanEquip() {
        Transform equipButton = this.transform.GetChild(1);
        if (!isEquippable()) {
            equipButton.gameObject.SetActive(false);
        } else {
            Equipable theEquip = selectedItem as Equipable;
            equipButton.gameObject.SetActive(true);
            TextMeshProUGUI textUI = equipButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (theEquip.isEquipped()) {
                textUI.text = "Unequip";
            } else {
                textUI.text = "Equip";
            }
        }
    }


}
