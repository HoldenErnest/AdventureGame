// Holden Ernest - 3/10/2023
// This class takes UI inputs and converts them to a new characterCreator in which to save later

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class saveCharacter : MonoBehaviour
{
    public TMP_InputField filename;  // 0    1    2    3    4    5    6
    public TMP_InputField nameInput;
    public TMP_InputField titleInput;
    public TMP_InputField descInput;
    public TMP_InputField healthInput;
    public TMP_InputField[] stats; // lvl, con, str, dex, int, evd, spd
    public TextMeshProUGUI pointsText;
    public TMP_Dropdown bodyTexture;
    public CustomDropdown items;
    public CustomDropdown equips;
    public CustomDropdown skills;

    private CharacterCreator thisCharacter;

    // Start is called before the first frame update
    void Start()
    {
        thisCharacter = Knowledge.getCharBlueprint("genericCharacter");
    }
    private string[] listToArray(List<string> b) {
        string[] a = new string[b.Count];
        for (int i = 0; i < b.Count; i++) {
            a[i] = b[i];
        }
        return a;
    }
    private List<string> ArrayToList(string[] b) {
        List<string> a = new List<string>();
        foreach (string t in b) {
            a.Add(t);
        }
        return a;
    }

    // let the user know how many points they have left
    public void updateLevel() {
        thisCharacter.stats.constitution = Int32.Parse(stats[1].text);
        thisCharacter.stats.strength = Int32.Parse(stats[2].text);
        thisCharacter.stats.dexterity = Int32.Parse(stats[3].text);
        thisCharacter.stats.intelligence = Int32.Parse(stats[4].text);
        thisCharacter.stats.evasion = Int32.Parse(stats[5].text);
        thisCharacter.stats.speed = Int32.Parse(stats[6].text);
        thisCharacter.stats.setLevel(Int32.Parse(stats[0].text));
        pointsText.text = "Points available: " + thisCharacter.stats.getAvailableAttr();
    }

    // onClick event to save the current input fields to a character
    public void createCharacter() {
        updateLevel();
        thisCharacter.name = nameInput.text;
        thisCharacter.title = titleInput.text;
        //thisCharacter.description = descInput.text;
        thisCharacter.baseHealth = Int32.Parse(healthInput.text);
        thisCharacter.bodyTexture = bodyTexture.options[bodyTexture.value].text;
        //thisCharacter.items = listToArray(items.theList);
        thisCharacter.equipment = listToArray(equips.theList);
        thisCharacter.startingSkills = listToArray(skills.theList);

        Knowledge.characterToJson(thisCharacter);
    }

    public void loadCharacter() {
        CharacterCreator c = Knowledge.getCharBlueprint(filename.text);
        if (!c) {
            Debug.Log(filename.text + ".json not found when loading character.");
            return;
        }
        thisCharacter = c;
    }
    private void updateInputs() {
        nameInput.text = thisCharacter.name;
        titleInput.text = thisCharacter.title;
        healthInput.text = "" + thisCharacter.baseHealth;
        //items.theList = ArrayToItems(thisCharacter.items);
        equips.theList = ArrayToList(thisCharacter.equipment);
        skills.theList = ArrayToList(thisCharacter.startingSkills);
        //bodyTexture.value = bodyTexture.options.IndexOf(new TMP_Dropdown.OptionData() {text=thisCharacter.bodyTexture});
    }
}
