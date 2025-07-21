using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//staff specific ability
//note: this method is older than chargedprojectile, so it may not fully respect the base hold methods logic because of my unexperience
public class DarkRayAbility : WeaponHoldAbility
{
    [SerializeField] private DamageRay damageRayPrefab;
    private DamageRay dmgRayRef;
    protected override void UseSkill() {
        base.UseSkill();
        if(wpn.animTriggerName == "Cast")
            StartAnimation();
        else
            Debug.LogError("DarkRayAbility: you're trying to use dark ray ability with a weapon that doesn't have the right animation");
        
    }
    override protected void DeactivateSkill() {
        base.DeactivateSkill();
        StopAnimation();
    }
    protected override void HoldSkill() {
        base.HoldSkill();
    }

    protected override void DoSomethingEveryTick() {
        base.DoSomethingEveryTick();
        SetMainWeaponLastShot();
    }

    protected void StartAnimation() {
        anim.SetTrigger("StartSkill");
        BlockPlayerInput();
    }
    protected void StopAnimation() {
        anim.SetTrigger("StopSkill");
    }

    //Used by animations
    public void DestroyRay() {
        dmgRayRef.getDestroyed();
        dmgRayRef = null;
    }
    public void StartRay() {
        if(dmgRayRef == null) {
            dmgRayRef = Instantiate(damageRayPrefab,wpn.GetFirstEndpoint().transform).Setup(player,wpn.stats);
        }
    }
}
