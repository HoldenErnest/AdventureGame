// Holden Ernest - 4/18/2023
// A parent class holding all the information needed to parse a certain object type into a name and sprite for displaying
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInfo : MonoBehaviour {
    
    private Sprite icon;
    private string title;

    public DisplayInfo(Sprite s) {
        icon = s;
        title = s.name;
    }
    
    public virtual string getTitle() {
        return title;
    }
    public virtual Sprite getSprite() {
        return icon;
    }
}
