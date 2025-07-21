using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TapAbility : Ability
{
    override protected void Update()
    {
        base.Update();
        if(InputCondition())
            if(StartCondition() && ManaRequirements()) {
                SetLastUsed();
                UseSkill();
            }
    }
    override protected void UseSkill() {
//        Debug.Log("tap ability");
        MenuManager.instance.ShowAbilityIndicatorFill(GetCooldown());
    }
    protected virtual bool InputCondition() {
        return Input.GetKeyDown(KeyCode.Space);
    }
    protected override bool StartCondition() {
        return CooldownCondition();
    }

    public override string GetAbilityType() {
        return "Tap";
    }
}
