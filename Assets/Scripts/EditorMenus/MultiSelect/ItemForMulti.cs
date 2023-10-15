// Holden Ernest - 10/15/2023
// a dumb multiselect item UI

using TMPro;
using UnityEngine;

public class ItemForMulti : MonoBehaviour {

    [SerializeField]
    private TMP_Text itemText;
    [SerializeField]
    private GameObject theButton;

    public string value; // stored value this item holds

    private MultiSelectItems thisMultiSelect;

    public void set(MultiSelectItems thisMulti, string s) {
        value = s;
        thisMultiSelect = thisMulti;
        setText(s);
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
