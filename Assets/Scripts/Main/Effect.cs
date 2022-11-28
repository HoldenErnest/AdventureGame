using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Effect : MonoBehaviour
{

    public string effectType;
    public int effectTier = 1;
    public int damage;
    public int duration;
    public GameObject user; // the caster who put the effect in place
    private bool effectActive = true;
    
    public Effect() {
        effectType = "Unknown Effect";
    }

    void Start() {
        getUser();
        
    }

    public Character getUser() {
        return user.GetComponent<Character>();
    }
    public bool isActive() {
        return effectActive;
    }

    public void endEffect() {
        effectActive = false;
    }
    public int getEffectTier() {
        return effectTier;
    }
}
