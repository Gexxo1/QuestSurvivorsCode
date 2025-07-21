using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEnemy : Enemy {
    [SerializeField] protected float baseAbilityCd = 0; 
    protected bool abilityInProcess = false;
    protected void ExtractRandomCooldown() {
        minCd = (int)(baseAbilityCd * 0.5f);
        if(minCd <= 0)
            minCd = 1;
        maxCd = (int)(baseAbilityCd * 1.5f);
        randAbilityCd = Random.Range(minCd, maxCd);
    }
    
    protected override void OnEnable() {
        base.OnEnable();
        ExtractRandomCooldown();
    }
    
    protected abstract void UseAbility();
    private int minCd, maxCd;
    protected float timeCounter = 0;
    protected float randAbilityCd = 1;
    protected virtual bool CanUseAbility() {
        return timeCounter >= randAbilityCd;
    }
    protected override void FixedUpdate() {
        base.FixedUpdate();
        if(!abilityInProcess)
            timeCounter += Time.deltaTime;
        
        if (CanUseAbility()) {
            UseAbility();
            ResetAbilityAttributes();
            ExtractRandomCooldown();
        }
    }

    protected void ResetAbilityAttributes() {
        randAbilityCd = Random.Range(minCd, maxCd);
        timeCounter = 0;
    }
}   
