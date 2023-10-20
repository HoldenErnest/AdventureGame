// Holden Ernest - 10/20/2023
// A hopefully simple script for loading options into a TMP dropdown menu

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownLoader : MonoBehaviour {

    public TMP_Dropdown theDropdown;
    public bool loadBodyTextures;
    void Start() {
        loadAll();
    }

    private void loadAll() {
        if (loadBodyTextures) {
            theDropdown.options.Clear ();
            string[] list = Knowledge.getAllBodyTextureNames();
            foreach (string t in list) {
                theDropdown.options.Add (new TMP_Dropdown.OptionData() {text=t});
            }
        }
    }


}
