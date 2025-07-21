using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TemporaryStatsPowerup : PowerUp
{ //this is abstract because the class lets implement the condition where the stats are applied into another class
//this class doesn't inherit statspowerup because the stats themselves are improved upon upgrade (and not directly improved into the player)
    [Header("Temporary Stats")]
    //base stats
    [SerializeField] protected StatsUp baseStats;
    [SerializeField] protected StatsUp upgradeStats;
    private StatsUp statsInstance; //this is needed because onupgrade are enhanced
    private StatsUp appliedStats;
    //weapon stats
    [SerializeField] protected WeaponStats wpnBaseStats;
    [SerializeField] protected WeaponStats wpnUpgradeStats;
    private WeaponStats wpnStatsInstance; 
    private WeaponStats wpnAppliedStats;
    protected virtual void Start() {
        if(baseStats != null) {
            if(statsInstance == null)
                statsInstance = Instantiate(baseStats);
            else
                Debug.LogWarning("Stats instance already exists");
        }
        if(wpnBaseStats != null) {
            if(wpnStatsInstance == null)
                wpnStatsInstance = Instantiate(wpnBaseStats);
            else
                Debug.LogWarning("Weapon stats instance already exists");
        }
    }
    public override void onUpgrade() {
        //if(!TresholdCondition()) Debug.Log("Treshold not met: will not update stats");
        if(TresholdCondition())
            RemoveStats();
        if(baseStats != null) {
            if(statsInstance == null)
                statsInstance = Instantiate(baseStats);
            statsInstance.AddToStats(upgradeStats);
        }
        if(wpnBaseStats != null) {
            if(wpnStatsInstance == null)
                wpnStatsInstance = Instantiate(wpnBaseStats);
            wpnStatsInstance.AddToStats(wpnUpgradeStats);
        }
        if(TresholdCondition())
            ApplyStats();
    }
    public override string GetDescription() {
        return GetDescString(baseStats, wpnBaseStats);
    }
    public override string GetUpgrDesc() {
        return GetDescString(upgradeStats, wpnUpgradeStats);
    }
    private string GetDescString(StatsUp stats, WeaponStats wpnStats) {
        string statsDesc = "";
        if(stats != null)
            statsDesc = stats.GetUpgradeDescription();
        if(wpnStats != null) {
            statsDesc += wpnStats.GetUpgradeDescription();
        }
        return statsDesc;
    }
    private bool statsApplied = false;
    protected virtual void ApplyStats() {
        if(statsApplied) {
            Debug.LogWarning("Stats already applied");
            return;
        }
        if(statsInstance != null) {
            appliedStats = statsInstance;
            player.AddStats(appliedStats);
            Debug.Log(title + " Stats applied");
            statsApplied = true;
        }
        if(wpnStatsInstance != null) {
            wpnAppliedStats = wpnStatsInstance;
            player.inventory.AddStatsToMainWeapon(wpnAppliedStats);
            Debug.Log(title + " Stats2 applied");
            statsApplied = true;
        }
        GameManager.instance.ShowText("+stats", player.transform.position, true);
        MenuManager.instance.AddBuffsToUI(title, GetSprite());
    }
    protected virtual void RemoveStats() {
        if(!statsApplied) {
            Debug.LogWarning("Stats already removed");
            return;
        }
        if(appliedStats != null) {
            player.RemoveStats(appliedStats);
            appliedStats = null;
            Debug.Log(title + " Stats removed");
            statsApplied = false;
        }
        if(wpnAppliedStats != null) {
            player.inventory.RemoveStatsToMainWeapon(wpnAppliedStats);
            wpnAppliedStats = null;
            Debug.Log(title + " Stats2 removed");
            statsApplied = false;
        }
        GameManager.instance.ShowText("-stats", player.transform.position, true);
        MenuManager.instance.RemoveBuffFromUI(title);
    }

    protected abstract bool TresholdCondition();
}
