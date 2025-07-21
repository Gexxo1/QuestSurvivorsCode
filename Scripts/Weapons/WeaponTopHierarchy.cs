using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTopHierarchy : ScriptRedirector
{
    [SerializeField] private GameObject wpnOrigin;
    [SerializeField] private Pickup pickupVersion;
    //[Header("Ability - if there's not any ability can be null")]
    private Ability relatedAbility;
    [Header("Skins - Can be null")]
    [SerializeField] private Sprite newSprite;
    [SerializeField] private Bullet newBullet;
    
    private void Start() { 
        //getWeaponCore().setWeaponSkin(newSprite, newBullet);
        //ability
        GameObject ability = GameObject.Find("Ability");
        relatedAbility = ability.GetComponentInChildren<Ability>();
    }
    public ProjectileWeapon getWeaponCore() {
        //return interestedGO.GetComponentInChildren<Weapon>();
        return interestedGO.GetComponentInChildren<ProjectileWeapon>();
    }

    //Possibile parameters: Dark ray --> "DestroyRay" , "StartRay"
    public void AbilityDelegateMethod(string method)
    {
        if(relatedAbility.TryGetComponent(out DarkRayAbility dr))
            relatedAbility.SendMessage(method);
        else if(relatedAbility.TryGetComponent(out TripleShot ts))
            relatedAbility.SendMessage(method);
        else
            Debug.LogWarning("WeaponTopHierarchy: AbilityDelegateMethod() - Ability not found");
    }
    public void WeaponDelegateMethod(string method)
    {
        getWeaponCore().SendMessage(method);
    }

    public void BowDelegateMethod(string method) {
        if(getWeaponCore() is Bow bow && bow.isCharging)
            bow.SendMessage(method);
        else if(relatedAbility is TripleShot ts && ts.isCharging)
            relatedAbility.SendMessage(method);
        else
            Debug.LogWarning("WeaponTopHierarchy: BowDelegateMethod() - Something went wrong");
    }

    public Pickup getPickup() {
        return pickupVersion;
    }

    //Codice per armi che hanno attacco1 e attacco2 --> viene chiamato quando facciamo swing1
    public void setSwingN() {
        if(getWeaponCore() is MeleeWeapon mw)
            mw.invertSwing();
        else if(getWeaponCore() is EnemyMeleeWeapon emw)
            emw.invertSwing();
        else
            Debug.LogWarning("WeaponTopHierarchy: setSwingN() - Weapon not found");
    }

    public GameObject getWpnOrigin() {
        return wpnOrigin;
    }
}
