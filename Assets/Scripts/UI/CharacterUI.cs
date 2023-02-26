// Holden Ernest - 2/25/2023
// The UI for when a character is hovered (variables set through Tools.cs)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterUI : MonoBehaviour {

    private string name;
    private int health;
    private int haxHealth;
    private int title;
    private Stats stats;

    public TextMeshProUGUI nameText;
    private Color enemy = new Color(1.0f, 0.2f, 0.2f, 1.0f);
    private Color friend = new Color(0.2f, 1.0f, 0.2f, 1.0f);
    private Color neutral = new Color(0.7f, 0.7f, 0.7f, 1.0f);

    public void setCharacter(Character c) {
        setName(c.name);
        
        if (c.gameObject.GetComponent<Team>().getTeam() == 0) {
            setTheme(friend);
        } else if (c.gameObject.GetComponent<Team>().getTeam() != 0) {
            setTheme(enemy);
        }
    }

    private void setName(string s) {
        nameText.text = s;
    }
    private void setHealth() {

    }
    private void setMaxHealth() {

    }
    private void setTheme(Color c) {
        nameText.color = c;
    }
    
}
