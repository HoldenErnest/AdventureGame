// Holden Ernest - 10/20/2023
// A hopefully simple script for loading options into a TMP dropdown menu

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownLoader : MonoBehaviour {

    public TMP_Dropdown theDropdown;
    public bool haveNoneOption = false;

    public bool loadBodyTextures;
    public bool loadSkills;
    public bool loadSkillPrefabs;

    void Start() {
        loadAll();
    }

    private void loadAll() {
        theDropdown.options.Clear ();
        if (haveNoneOption) {
            theDropdown.options.Add (new TMP_Dropdown.OptionData() {text="none"});
        }
        if (loadBodyTextures) {
            string[] list = Knowledge.getAllBodyTextureNames();
            foreach (string t in list) {
                theDropdown.options.Add (new TMP_Dropdown.OptionData() {text=t});
            }
        }
        if (loadSkills) {
            Skill[] list = Knowledge.getAllSkills();
            foreach (Skill skl in list) {
                theDropdown.options.Add (new TMP_Dropdown.OptionData() {text=skl.skillName});
            }
        }
        if (loadSkillPrefabs) {
            GameObject[] list = Knowledge.getAllSkillPrefabs();
            foreach (GameObject ob in list) {
                theDropdown.options.Add (new TMP_Dropdown.OptionData() {text=ob.name});
            }
        }
    }


}
