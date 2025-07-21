using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ChargedAttackAbility : WeaponHoldAbility
{
    [Header("Charged Attack")]
    [SerializeField] protected List<PhasesModifier> phase;
    [SerializeField] protected bool autoRelease = true;
    /*
    [SerializeField] private int bulletModifier = 1;
    [SerializeField] private int pierceModifier = 1;
    [SerializeField] private float speedModifier = 2f;
    */
    [HideInInspector] public bool isCharging = false;
    protected WeaponStats GetFixedAttributes(int index) {
        WeaponStats weaponStats = ScriptableObject.CreateInstance<WeaponStats>();
        int dmg, pierce; float knock, bullspd;
        //dmg = (int)(wpn.stats.baseDamage * fixedModfier[shootStrength]);
        dmg = (int)(wpn.stats.baseDamage * phase[index].dmgMod);
        knock = wpn.stats.knockback / phase[index].bulletNumMod;
        bullspd = wpn.stats.projectileSpeed * phase[index].spdMod;
        //pierce = wpn.stats.projectilePierce + shootStrength + 1;
        pierce = wpn.stats.projectilePierce * phase[index].pierceMod + phase[index].pierceAdd;
//        Debug.Log("Dmg[" + dmg + "] Knock[" + knock + "] BullSpd[" + bullspd + "] Pierce[" + pierce + "]");
        weaponStats.SetWeaponStats(dmg, knock, wpn.stats.atkCd, bullspd, 
            wpn.stats.weaponSize, pierce, phase[index].bulletNumMod, 
            wpn.stats.projectileBounce, wpn.stats.statusEffects, wpn.stats.projectileHoming);
        return weaponStats;
    }

    protected override void UseSkill() {
        base.UseSkill();
        isCharging = true;
        BlockPlayerInput();
    }
    protected override void DeactivateSkill() {
        base.DeactivateSkill();
        isCharging = false;
        RestorePlayerInput();
    }


    protected virtual void Shoot(int index) {
        if(phase.Count == 0) {
            Debug.LogError("No phases set for this ability");
            return;
        }
        if(phase.Count <= index) {
            Debug.LogError("Index out of range for phases");
            return;
        }
        
        InstantiateBullet(index);
    }

    protected virtual void InstantiateBullet(int index) {
        wpn.InstantiateBullets(phase[index].hasManaGain, GetFixedAttributes(index), phase[index].newBullet);
    }
    
    protected override float GetCooldown() {
        if(wpn == null)
            return base.GetCooldown();
        return wpn.stats.atkCd;
    }
    
    public override string GetDescription() {
        return base.GetDescription() + "\n" + GetChargeDetails();
    }

    protected virtual string GetChargeDetails() {
        return 
        //"Number of Charges " + maxTickNumber + 
        "Charge Time <color=#ff6e00>" + holdTick * maxTickNumber + " </color>s";
    }

    protected override bool ReleaseCondition() {
        if(!autoRelease)
            return ReleaseInput() && isAbilityActive;
        else
            return (ReleaseInput() || (!IsTickCapNotReached() && HasLastTickPassed(0.25f))) && isAbilityActive;
    }
}

[Serializable]
public class PhasesModifier {
    public float dmgMod = 1f;
    public int bulletNumMod = 1;
    public int pierceMod = 1;
    public int pierceAdd = 0;
    public float spdMod = 1f;
    public Bullet newBullet;
    public bool hasManaGain = false;
    /*
    public PhasesModifier(int bulletModifier, int pierceModifier, float speedModifier, Bullet bullet = null) {
        this.bulletNumMod = bulletModifier;
        this.pierceMod = pierceModifier;
        this.spdMod = speedModifier;
        this.newBullet = bullet;
    }
    */
}