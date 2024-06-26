// Holden Ernest - 10/11/2023
// A placeholder for gameobjects, allowing them to be saved with all of these params

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpecialObject : MonoBehaviour {

    public int x;
    public int y;
    public int type; // type of special object this is (what gameobject it will be instanced into)
    public string file;
    public GameObject objToHold;

    public SpecialObject(int x1, int y1, GameObject g)
    {
        
        x = x1;
        y = y1;
        Debug.Log(x1 +">>"+x + ", "+y1+">>"+y);
        objToHold = g;
        file = g.name;
        type = objectToType(g);
    }

    private int objectToType(GameObject g) {
        if (g.GetComponent<Chest>() != null)
            return 0;
        else if (g.GetComponent<Spawner>() != null)
            return 1;
        return -1;
    }
    public string toJson() {
        switch(type) {
            case 0:
                return JsonUtility.ToJson(objToHold.GetComponent<Chest>());
            case 1:
                return JsonUtility.ToJson(objToHold.GetComponent<Spawner>());
        }
        return "{}";
    }
    public static void loadComponentFromJson(GameObject g, int typ, string json) {
        switch(typ) {
            case 0:
                JsonUtility.FromJsonOverwrite(json, g.GetComponent<Chest>());
                break;
            case 1:
                JsonUtility.FromJsonOverwrite(json, g.GetComponent<Spawner>());
                break;
        }
    }

    public bool hasPosition(int xx, int yy) {
        return xx == x && yy == y;
    }
}
