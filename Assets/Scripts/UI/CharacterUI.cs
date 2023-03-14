// Holden Ernest - 2/25/2023
// The complete STATIC UI for when a character is hovered (variables set through Tools.cs)
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
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI[] statsText;// con, str, dex, int, evd, spd, amr
    public CustomSlider healthBar;

    private Color enemy = new Color(1.0f, 0.2f, 0.2f, 1.0f);
    private Color friend = new Color(0.2f, 1.0f, 0.2f, 1.0f);
    private Color neutral = new Color(0.7f, 0.7f, 0.7f, 1.0f);

    private Character theCharacter;

    /*
    int i = 0;
    bool b = false;
    void Update() {
        if (!b)
        StartCoroutine(colorTest());
    }
    // garbage quick test to see the color changes
    private IEnumerator colorTest() { 
        Debug.Log("thing happen");
        b = true;
        setStat(0,i);
        yield return new WaitForSeconds(0.2f);
        i++;
        if (i > 50)i = 0;
        b = false;
    }
    */
    
    public void setCharacter(Character c) {
        theCharacter = c;
        setName(c.name);
        setTitle(c.title);
        setStats(c.userStats);
        setHealth(c.getHp(), c.userStats.getMaxHp(c.baseMaxHp));
        setLevel(c.userStats.getLevel());
        if (c.gameObject.GetComponent<Team>().getTeam() == 0) {
            setTheme(friend);
        } else if (c.gameObject.GetComponent<Team>().getTeam() != 0) {
            setTheme(enemy);
        }
    }

    private void setName(string s) {
        nameText.text = s;
    }
    // what is changed based on team
    private void setTheme(Color c) {
        //nameText.color = c;
    }

    private void setTitle(string s) {
        titleText.text = s;
    }
    private void setLevel(int l) {
        levelText.text = "" + l;
    }
    private void setHealth(int hp, int maxHp) {
        healthBar.updateSlider(hp, maxHp);
    }
    public void updateHealth() {
        if (theCharacter)
            setHealth(theCharacter.getHp(), theCharacter.userStats.getMaxHp(theCharacter.baseMaxHp));
    }
    private void setStats(Stats s) {
        setStat(0,s.constitution);
        setStat(1,s.strength);
        setStat(2,s.dexterity);
        setStat(3,s.intelligence);
        setStat(4,s.evasion);
        setStat(5,s.speed);
        setStat(6,s.armor);
    }

    private void setStat(int stat, int points) { // changes the specified stat in the statsText array, how many points should it be set to
        statsText[stat].text = points.ToString();

        statsText[stat].color = getColorFromPoints(points);
    }
    private Color getColorFromPoints(int points) {
        //set stat color
        float r = 1.0f, g = 1.0f, b = 1.0f;
        float t = (((points%10.0f)+1.0f) / 10);
        // t will count up with more points while (1.0 - t) counts down
        b = 1.0f;
        g = 1.0f - t;
        r = 1.0f - t;
        if (points >= 10) {
            b = 1.0f - t;
            g = t;
            r = 0.0f;
            if (points >= 20) {
                b = 0.0f;
                g = 1.0f - t;
                r = t;
                if (points >= 30) {
                    b = t;
                    g = 0.0f;
                    r = 1.0f;
                    if (points >= 40) {
                        b = 1.0f;
                        g = 0.0f;
                        r = 1.0f;
                }
                }
            }
        }

        return new Color(r,g,b,1.0f);
    }
    
}
