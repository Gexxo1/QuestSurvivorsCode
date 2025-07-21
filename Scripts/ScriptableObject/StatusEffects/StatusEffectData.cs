using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "ScriptableObjects/StatusEffects/BasicStatusEffect")]
public class StatusEffectData : GenericStats {
    #region internal variables & getters
    public string statusName = "none"; //or status type --> it's an unique identifier, ex.: there's only 1 poison type status and only 1 fire type status, etc.
    public int damageOverTime = 0;
    [SerializeField] [Range(0,100)] public float slowAmountPercent = 0f;
    public float duration = 0f;
    public Color color = Color.white;
    
    public float GetSlowAmount() {
        return 1 - slowAmountPercent / 100;
    }
    public float GetTickSpeed() {
        return tickSpeed;
    }
    public bool IsInfinite() {
        return isInfinite;
    }
    #endregion 
    #region serialization
    [SerializeField] private float tickSpeed = 1f;
    [HideInInspector] public float currentEffectTime = 0f;
    [HideInInspector] public float lastTickTime = 0f;
    [SerializeField] private bool isInfinite = false;
    #endregion
    #region Apply & Remove
    public virtual void ApplyToTarget(Fighter f) { target = f; }
    public virtual void OnRemove() {
        IsFinished = false;
    }
    #endregion
    public void ResetStatus() { //this is called when the status is removed
        currentEffectTime = 0;
        lastTickTime = 0;
    }
    public void RefreshStatus() {
        currentEffectTime = 0;
    }
    
    public bool IsFinished { get; private set; }
    public void Update() {
        if(IsFinished) return;
        
        currentEffectTime += Time.deltaTime;
//        if(statusName != "Regen") Debug.Log("Active status: [" + statusName + "] - currentEffectTime: [" + currentEffectTime + "] >= " + lastTickTime + " + " + tickSpeed);
        if(currentEffectTime >= lastTickTime + tickSpeed) {
            lastTickTime = currentEffectTime;
            StatusEffectDamage();
            if(currentEffectTime >= duration && !isInfinite) {
                IsFinished = true;
                ResetStatus();
                return;
            }
        }
    }
    private Fighter target; 
    
    public void StatusEffectDamage() {
        if(target == null) {
            Debug.LogWarning("DOT Target is null");
            return;
        }
        if(damageOverTime != 0) 
            target.getHit(GameManager.instance.RawDamageCalculation(null,target.transform.position,damageOverTime,DamageType.status,color),0);
    }
    public StatusEffectData(string name, float dur, int dot, float slow, Color32 c) {
        statusName = name;
        duration = dur;
        damageOverTime = dot;
        slowAmountPercent = slow;
        color = c;
    }
    public void addToStats(StatusEffectData add)
    {
        duration += add.duration;
        damageOverTime += add.damageOverTime;
        slowAmountPercent += add.slowAmountPercent;
        if(slowAmountPercent > 100)
            slowAmountPercent = 100;
    }
    
    public override string ToString() {
        return "statusName [" + statusName + "] duration [" + duration + "] damageOverTime [" + damageOverTime + "] slowAmountPercent [" + GetSlowAmount() + "%] Color[" + color + "]";
    }
    public string GetDescription(bool isDotUpgrade = false) {
        string s = "";
        if(!isDotUpgrade) {
            s = "Applies " + statusName;
            if(duration != 0 && !isInfinite)
                s += " for " + duration + " seconds:\n";
            else
                s += " permanently:\n";
            string overTime = "every ";
            if(tickSpeed == 1f)
                overTime += "second";
            else
                overTime += tickSpeed + " seconds";
            if(damageOverTime != 0)
                if(damageOverTime > 0)
                    s += "Deals " + damageOverTime + " damage " + overTime + ".\n";
                else 
                    s += "Recovers " + -damageOverTime + " health " + overTime + ".\n";
            if(slowAmountPercent != 0)
                if(slowAmountPercent > 0)
                    s += "Slows for " + slowAmountPercent + "%.\n";
                else 
                    s += "Speeds up for " + -slowAmountPercent + "%.\n";
        }
        else 
            s += GetDotUpgradeDesc();
        return s;
    }
    /*
    public string GetDescription(bool isDotUpgrade = false) {
        string s = "";
        if(!isDotUpgrade) {
            s = "Applies " + statusName;
            if(duration != 0 && !isInfinite)
                s += " for " + duration + " seconds:\n";
            else
                s += " indefinitely:\n";
            if(damageOverTime != 0)
                if(damageOverTime > 0)
                    s += "Deals " + damageOverTime + " damage over time.\n";
                else 
                    s += "Recovers " + -damageOverTime + " health over time.\n";
            if(slowAmountPercent != 0)
                if(slowAmountPercent > 0)
                    s += "Slows for " + slowAmountPercent + "%.\n";
                else 
                    s += "Speeds up for " + -slowAmountPercent + "%.\n";
        }
        else 
            s += GetDotUpgradeDesc();
        return s;
    }
    */

    public string GetUpgradeDescription(bool isDotUpgrade = false) {
        string s = "";
        if(!isDotUpgrade) {
            if(duration != 0)
                s += "Duration increased by " + duration + " seconds.\n";
            if(damageOverTime != 0)
                if(damageOverTime > 0)
                    s += "Damage Over Time increased by " + damageOverTime + ".\n";
                else 
                    s += "Healing Over Time increased by " + -damageOverTime + ".\n";
            if(slowAmountPercent != 0)
                if(slowAmountPercent > 0)
                    s += "Slow increased by " + slowAmountPercent + "%.\n";
                else 
                    s += "Speed increased by " + -slowAmountPercent + "%.\n";
        }
        else 
            s += GetDotUpgradeDesc();
        return s;
    }

    private string GetDotUpgradeDesc() {
        string s = "";
        if(damageOverTime != 0)
            s += "Increases all 'Damage Over Time' by " + damageOverTime + ".\n";
        if(slowAmountPercent != 0)
            s += "Increases all slow effects by " + slowAmountPercent + "%.\n";
        if(duration != 0)
            s += "Increases all status effect durations by " + duration + " seconds.\n";
        return s;
    }
}
