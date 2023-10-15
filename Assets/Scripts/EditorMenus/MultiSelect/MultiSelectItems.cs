// Holden Ernest - 10/15/2023
// The script to add Items to a multiSelect UI thing

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MultiSelectItems : MonoBehaviour {

    public string title;

    [SerializeField]
    private GameObject genericItemBP; // default gameobject to instantiate with each new item
    [SerializeField]
    private GameObject content; // default gameobject to instantiate with each new item
    [SerializeField]
    private TMP_Text titleText;

    [HideInInspector]
    public bool transferItems; // everytime you click an item it is transfered to the other list

    [HideInInspector]
    public MultiSelectItems otherList;

    private List<GameObject> items = new List<GameObject>(); // the base list of items

    void Start() {
        updateTitle();
        addItem("bruh");
        addItem("dont");
        addItem("even care");
    }

    private void updateTitle() {
        titleText.text = title;
    }

    private void addItem(string s) {
        GameObject g = Instantiate(genericItemBP, content.transform);
        items.Add(g);
        g.GetComponent<ItemForMulti>().set(this, s);
        g.GetComponent<ItemForMulti>().setButtonTransfer(transferItems);
    }
    public void transferAnItem(GameObject g) {
        string itemVal = g.GetComponent<ItemForMulti>().value;
        if (transferItems) {
            otherList.addItem(itemVal);
        } else {
            items.Remove(g);
            Destroy(g);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MultiSelectItems))]
public class RandomScript_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector(); // for other non-HideInInspector fields

		MultiSelectItems script = (MultiSelectItems)target;

		// draw checkbox for the bool
		script.transferItems = EditorGUILayout.Toggle("Transfer Items", script.transferItems);
		if (script.transferItems) // if bool is true, show other fields
		{
			script.otherList = EditorGUILayout.ObjectField("Other Multi-Select", script.otherList, typeof(MultiSelectItems), true) as MultiSelectItems;
		}
	}
}
#endif
