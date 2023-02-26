// Holden Ernest - 2/24/2023
// Generic List object to be saved in json format

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TextList : MonoBehaviour {
    
    public string listName;
    public string[] data;

    public string getRandom() {
        return data[UnityEngine.Random.Range(0, data.Length-1)] ?? "";
    }
    public string get(int i) {
        return data[i] ?? "";
    }

    public void printAll() {
        Debug.Log(data);
        string s = "[ ";
        foreach(string d in data) {
            s += (d + ", ");
        }
        Debug.Log(s.Substring(0,s.Length-2) + " ]");
    }
}