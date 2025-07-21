using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldAbility : Ability
{
    [Header("Hold Ability")]
    [SerializeField] protected float holdTick = 1.0f;
    [SerializeField] protected int maxTickNumber = -1; //if -1 --> infinite
    protected int tickPassedCounter = -1;
    [SerializeField] protected bool drainManaEveryTick = false; //drainmanaeverytick = true --> drain mana only on deactivation
    
    protected bool isAbilityActive = false;
    
    //attenzione: l'ordine di use skill, deactivate skill e hold skill è importante, non va alterato (per i bool)

    override protected void Update() {   
        base.Update();
        //Debug.Log("Is Skill Active?: " + isAbilityActive);
        
        if(StartInput()) {
            if(StartCondition() && ManaRequirements()) {
                SetLastUsed();
                UseSkill();
            }
        }
        if(ReleaseCondition())
            DeactivateSkill();
        if(HoldingCondition())
            HoldSkill();
    }
    /* diff between useskill, deactivateskill, holdskill
        * useskill: called when the ability is activated (pressed key)
        * deactivateskill: called when the ability is deactivated (released key/other conditions)
        * holdskill: called every frame the ability is active
        note: this may not be true for charged attack (because i designed this scheme before charged attack was implemented)
    */
    override protected void UseSkill() {
        //Debug.Log("ON " + counter);
        isAbilityActive = true;
        DoSomethingEveryTick();
    }
    virtual protected void HoldSkill() {
        chargeTime += Time.deltaTime;
//        Debug.Log(tickPassedCounter + " < " + (maxTickNumber-1));
        if(HasLastTickPassed() && IsTickCapNotReached())
            DoSomethingEveryTick();
    }
    virtual protected void DeactivateSkill() {
        //Debug.Log("OFF " + counter);
        isAbilityActive = false;
        chargeTime = lastTick = 0;
        if(!drainManaEveryTick)
            DrainMana();
        tickPassedCounter = -1;
    }
    private float lastTick = 0;
    protected float chargeTime = 0;
    protected virtual void DoSomethingEveryTick() { 
        if(drainManaEveryTick)
            DrainMana();
        lastTick = chargeTime;
        tickPassedCounter++;
        
        //Debug.Log("Tick Passed [" + tickPassedCounter + "] Charge Time [" + chargeTime + "]");
    }
    protected bool HasLastTickPassed(float extraTime = 0) {
        return chargeTime - lastTick > holdTick + extraTime;
    }
    protected bool IsTickCapNotReached() {
        if(maxTickNumber == -1)
            return true;
        return tickPassedCounter < maxTickNumber - 1;
    }
    public override void SetLastUsed() {
        base.SetLastUsed();
        MenuManager.instance.ShowAbilityIndicatorFill(GetCooldown());
    }

    protected virtual bool StartInput() {
        return Input.GetKey(KeyCode.Space);
    }
    protected override bool StartCondition() {
        return CooldownCondition() && !isAbilityActive;
    }
    /*
    protected virtual bool DeactivateCondition() {
        return (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)
            || GameManager.instance.player.currMp < manaCost) && holdingSkill || !holdingSkill;
    }*/
    protected virtual bool ReleaseInput() {
        return Input.GetKeyUp(KeyCode.Space);
    }
    protected virtual bool HoldingCondition() {
        return isAbilityActive && ManaRequirements();
    }
    protected virtual bool ReleaseCondition() {
        return (ReleaseInput() || !ManaRequirements()) && isAbilityActive;
        //return !isSkillActive && (StopInputCondition() || !ManaRequirements());
    }
    public override string GetAbilityType() {
        return "Hold";
    }

    public int GetMaxTickNumber() {
        return maxTickNumber;
    }
}