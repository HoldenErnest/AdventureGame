// Holden Ernest - 4/18/2023
// A parent class holding all the information needed to parse a certain object type into a name and sprite for displaying
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInfo : MonoBehaviour {
    
    private Sprite icon;
    private string title;
    private int pos; // the position 

    public DisplayInfo(int p, Sprite s) {
        pos = p;
        icon = s;
        title = s.name;
    }
    public DisplayInfo(int p, Character c) {
        pos = p;
        //icon = c.icon;
        title = c.name;
    }
    
    public virtual string getTitle() {
        return title;
    }
    public virtual Sprite getSprite() {
        return icon;
    }
    public int getPosition() {
        return pos;
    }
}
