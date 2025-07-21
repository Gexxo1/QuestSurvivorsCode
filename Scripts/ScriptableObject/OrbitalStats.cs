using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbitStats", menuName = "ScriptableObjects/WeaponStats/OrbitalStats")]
public class OrbitalStats : WeaponStats {
    protected override string GetProjSpdDesc(bool dontShowInfo = false) {
        if(projectileSpeed == 0) return "";
        return "Orbital Speed " + IncreaseOrDecrease(projectileSpeed)
        + ShowDetailsCheck(GetSignedValue(projectileSpeed), dontShowInfo) + ".\n";
    }
    protected override string GetProjNumDesc(bool dontShowInfo = false) {
        if(projectileNumber == 0) return "";
        return "Orbital number " + IncreaseOrDecrease(projectileNumber)
        + ShowDetailsCheck(GetSignedValue(projectileNumber), dontShowInfo) + ".\n";
    }
    /*
    public int orbitalsNo = 1;
    public float orbitSpeed = 5f;
    public float orbitRadius = 15f;
    
    public void addToStats(OrbitalStats add) {
        base.AddToStats(add);
        orbitalsNo += add.orbitalsNo;
        orbitSpeed += add.orbitSpeed;
        orbitRadius += add.orbitRadius;
    }
    */
    /*
        * projectile number --> orbitalsNo
        * projectile speed --> orbitSpeed
        * projectile radius --> handled internally in the powerup
    */
    //to do sostituire con i metodi di base
    /*
    public override string GetDescription(bool dontShowInfo = false) {
        string s = base.GetDescription(dontShowInfo);
        if(orbitalsNo != 0) {
            string sign = orbitalsNo > 0 ? "increased" : "decreased";
            int v = orbitalsNo > 0 ? orbitalsNo : -orbitalsNo;
            string details = dontShowInfo ? " " : " by " + v;
            s += "Orbital number " + sign + details + ".\n";
        }
        if(orbitSpeed != 0) {
            string sign = orbitSpeed > 0 ? "increased" : "decreased";
            float v = orbitSpeed > 0 ? orbitSpeed : -orbitSpeed;
            string details = dontShowInfo ? " " : " by " + v * 100 + "%";
            s += "Orbital speed " + sign + details + ".\n";
        }
        if(orbitRadius != 0) {
            string sign = orbitRadius > 0 ? "increased" : "decreased";
            float v = orbitRadius > 0 ? orbitRadius : -orbitRadius;
            string details = dontShowInfo ? " " : " by " + v;
            s += "Orbital radius " + sign + details + ".\n";
        }

        return s;
    }
    */
}
