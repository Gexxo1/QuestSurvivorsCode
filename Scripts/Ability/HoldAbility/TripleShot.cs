using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this ability REQUIRES a bow weapon
//note: this method is older than chargedprojectile, so it may not fully respect the base hold methods logic because of my unexperience
public class TripleShot : ChargedAttackAbility
{
    protected new Bow wpn => (Bow)base.wpn;
    //private Renderer chargeIndicator1,chargeIndicator2,chargeIndicator3;
    protected override void Awake()
    {
        base.Awake();
        /*
        if(wpn is not Bow) {
            Debug.LogWarning("TripleShot requires a bow weapon");
            enabled = false;
        }
        */
    }
    protected override bool StartCondition() {
        return base.StartCondition() && !anim.GetBool("isCharging");
    }
    protected override bool ReleaseCondition() {
        return wpn.IsFullyCharged();
    }
    protected override void UseSkill() {
        base.UseSkill();
        wpn.SetVisualArrowActive(true,1);
        wpn.SetVisualArrowActive(true,2);
        //isCharging = true;
        anim.SetBool("isCharging", isCharging);
        
    }
    protected override void DeactivateSkill() {
        base.DeactivateSkill();
        wpn.SetVisualArrowActive(false,1);
        wpn.SetVisualArrowActive(false,2);
        anim.SetBool("isCharging", isCharging);
        Shoot(0);
    }
    protected override void Shoot(int index) {
        base.Shoot(index);
        wpn.ResetShot();
        AudioManager.instance.PlayPooledSFX("Bow",transform,0.5f);
    }

    protected override bool StartInput() {
        return base.StartInput() && !wpn.isCharging;
    }
    
}
