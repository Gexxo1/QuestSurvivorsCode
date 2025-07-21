using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class exists to differentiate from tripleshot, which is also inherited from chargedattackability
//tripleshot class is specific to bow weapons, then chargedprojectile class is meant to be a more re-usable class
public class ChargedProjectile : ChargedAttackAbility {
    [Header("Charged Projectile - can be null")]
    [SerializeField] WeaponStats addProjStats;
    //protected float chargeTime = 0f;
    protected override bool StartCondition() {
        return base.StartCondition() && !isCharging;
    }
    //protected override bool DeactivateCondition() { return isFullyCharged; }    
    protected override void DoSomethingEveryTick() {
        base.DoSomethingEveryTick();
        inventory.SetIndicatorActive(tickPassedCounter, true);
    }
    protected override void DeactivateSkill() {
        Shoot(tickPassedCounter);
        base.DeactivateSkill();
        //int finalChargeTime = (int)Mathf.Min(chargeTime, maxTickNumber);
        //Debug.Log("Final Charge Time [" + chargeTime + "] --> [" +  finalChargeTime + "]");
        inventory.DisableAllIndicators();
    }
    
    protected override void Shoot(int index) {
        base.Shoot(index);
//        Debug.Log("Shooting " + index + " --> " + phase[index].newBullet);
    }

    protected override void InstantiateBullet(int index) {
        WeaponStats stats = Instantiate(GetFixedAttributes(index));
        stats.AddToStats(addProjStats);

        wpn.InstantiateBullets(phase[index].newBullet, stats);
    }

}
