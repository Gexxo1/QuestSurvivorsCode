using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage
{
    public Vector3 origin;
    public int amount;
    public int originalAmount;
    public float knockback;
    public List<StatusEffectData> statusEffects;
    public bool isCrit;
    //public GameObject sourceWpn;
    public Fighter hitter;
    public Fighter hitFighter;
    public int sourceHeal;
    public Vector2 hitPoint;
    public HitSource hitSource;
    public DamageType type;
    public Color32 textColor;
    public bool hasManaGain;
    public bool playSound;
}
