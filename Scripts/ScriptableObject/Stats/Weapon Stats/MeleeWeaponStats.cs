using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponStats", menuName = "ScriptableObjects/WeaponStats/Melee")]
public class MeleeWeaponStats : WeaponStats
{
    [Header("Melee Weapon Only")]
    public float projectilePersistence = 0f; //0.3f //tempo di distruzione del proiettile
}
