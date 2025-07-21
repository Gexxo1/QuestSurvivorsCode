using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Classe di intermezzo, non viene implementata nei gameobject
public abstract class Weapon : Item
{
    [Header("Weapon")]
    [SerializeField] private WeaponStats originalStat;
    [HideInInspector] public WeaponStats stats;
    //Upgrade
    [HideInInspector] public SpriteRenderer spriteRenderer;
    //Possessore dell'arma
    [HideInInspector] public Fighter wielder;
    public string animTriggerName = "none";
    [HideInInspector] public Animator anim;
    [SerializeField] protected bool manaGainDisabled = false;
    protected virtual void Awake() {
        wielder = GetComponentInParent<Fighter>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponentInParent<Animator>();
        SetupStats();
    }
    protected virtual void Start() {
        //23/10/24 --> SetupStats() moved to awake because it was causing null reference exception
    }
    public void SetBlock(bool b) {
        wielder.setAttackBlocked(b);
    }
    private void SetupStats() {
        if(originalStat == null) {
            Debug.LogWarning("wielder's [" + wielder + "] weapon [" + title +  "] original Stat is null --> [" + originalStat + "]");
            return;
        }
        if(originalStat != null)
            stats = originalStat.InstantiateWeaponStats();
        //Debug.Log(gameObject.name + " setup stats: [" + stats + "]");
    }
    public void SetWielder(Fighter f) {
        wielder = f;
    }
    public WeaponStats GetOriginalWeaponStats() {
        return originalStat;
    }
    public virtual string GetWeaponBaseStatsDetails(bool showApproxStats = true) {
        return originalStat.GetWeaponBaseStats(showApproxStats);
    }

    public abstract string GetWeaponType();
}
