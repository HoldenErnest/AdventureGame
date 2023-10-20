// Holden Ernest - 10/15/2023
// a really bad way to store values for a multilist

using TMPro;
using UnityEngine;

public class ItemForMulti : MonoBehaviour {

    [SerializeField]
    private TMP_Text itemText;
    [SerializeField]
    private GameObject theButton;

    // yes I understand this sucks and I should use generic typing, I attempted it but unity inspector doesnt allow typed components?
    // anyway I gave up

    public Skill skillValue; // stored value this item holds
    public Item itemValue; // stored value this item holds
    public string otherValue; // stored value this item holds

    private MultiSelectItems thisMultiSelect;

    public void set(MultiSelectItems thisMulti, Skill i) {
        skillValue = i;
        thisMultiSelect = thisMulti;
        setText(i.skillName);
    }
    public void set(MultiSelectItems thisMulti, Item i) {
        itemValue = i;
        thisMultiSelect = thisMulti;
        if (i.GetType() == typeof(Equipable)) {
            setText(i.itemName + " (E)");
        } else {
            setText(i.itemName);
        }
    }
    public void set(MultiSelectItems thisMulti, string i) {
        otherValue = i;
        thisMultiSelect = thisMulti;
        setText(i);
    }

    public void onButtonPress() {
        thisMultiSelect.transferAnItem(this.gameObject);
    }

    private void setText(string s) {
        itemText.text = s;
    }
    public void setButtonTransfer(bool b) {
        if (b)
            theButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "+";
        else
            theButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "-";
    }

}
