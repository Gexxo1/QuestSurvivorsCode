using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChargedWeaponStats", menuName = "ScriptableObjects/WeaponStats/Charged")]
public class ChargedWeaponStats : WeaponStats
{
    //[Header("Charged Weapon Only")]
    //public float chargedModfier = 0f;

    //this class is kinda unused, but it's here for future implementations
    protected override string GetAtkSpdStr(bool showApproxStats = true) {
        string atkSpdStr = "";
        if(showApproxStats) {
            if(atkCd == 0.5)
                atkSpdStr = "Attack Speed: Normal\n";
            else if(atkCd < 0.5)
                atkSpdStr = "Attack Speed: Fast\n";
            else if(atkCd > 0.5)
                atkSpdStr = "Attack Speed: Slow\n";
        }
        else
            atkSpdStr = "Attack Speed: " + atkCd + "\n";
        return atkSpdStr;
    }
}
