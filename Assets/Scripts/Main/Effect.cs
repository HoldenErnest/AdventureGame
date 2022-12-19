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
    
    private int damageFromUser = 0;
    public Effect() {
        effectType = "Unknown Effect";
    }

    void Start() {
        getUser();
        setDamage();
    }

    public Character getUser() { // dont try to call after Start() because user might die when effect is still on
        return user.GetComponent<Character>();
    }
    public bool isActive() {
        return effectActive;
    }
    public void setDamage() {
        damageFromUser = getUser().attackByType(damage, effectType);
    }
    public int getDamageAfterUser() {
        return damageFromUser;
    }

    public void endEffect() {
        effectActive = false;
    }
    public int getEffectTier() {
        return effectTier;
    }
}
