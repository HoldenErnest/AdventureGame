using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpecialObject : MonoBehaviour {

    public int[] pos; // {x,y}
    public string type; // type of special object this is (what gameobject it will be instanced into)
    public int randomSaveThing;

}
