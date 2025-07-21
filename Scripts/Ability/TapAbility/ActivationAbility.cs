using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivationAbility : TapAbility {
    [SerializeField] private float manaDrainTick = 1.0f;
    protected override void UseSkill() {
        DrainMana();
        MenuManager.instance.ShowAbilityIndicatorFill(GetCooldown());
        //AudioManager.instance.PlayPooledSFX("WpnSwap", transform, 1f);
        StartCoroutine(ActivateAbility(manaDrainTick));
    }
    private bool stopUsingAbility;
    private bool canDeactivate;
    //coroutine base
    protected IEnumerator ActivateAbility(float secs) {
        //Debug.Log("Swap 1");
        AbilityActivation();
        stopUsingAbility = false;
        do {
            if(stopUsingAbility) break;
            SetLastUsed();
            yield return new WaitForSeconds(secs/10);
            canDeactivate = true;
            yield return new WaitForSeconds(secs);
        } while(TryDrainMana());
        //Debug.Log("Swap 2");
        AbilityDeactivation();
        canDeactivate = false;
        yield return null;
    }
    protected abstract void AbilityActivation();
    protected abstract void AbilityDeactivation();
    protected override void Update() {
        base.Update();
        if(canDeactivate && InputCondition()) 
            stopUsingAbility = true;
    }

    public override void SetLastUsed() {
        base.SetLastUsed();
        MenuManager.instance.ShowAbilityIndicatorFill(cooldown);
    }
    
    protected override string GetManaReq() {
        string everySec = "/s";
        if(manaDrainTick != 1) everySec = "/"+manaDrainTick+"s";
        return base.GetManaReq() + everySec;
    }
    
    public override string GetDescription() {
        return base.GetDescription() + "\n" + "<i> Press again to swap back </i>";
    }
}
