using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresholdBuff : TemporaryStatsPowerup
{
    [Header("Treshold Buff")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private ThresholdType thresholdType;
    [Range(0,100)] [SerializeField] private int tresholdPercent;
    protected override void Awake() {
        base.Awake();
        PowerupManager.OnHpTreshold += OnHpTreshold;
    }
    void OnDestroy() {
        PowerupManager.OnHpTreshold -= OnHpTreshold;
    }
    protected bool areStatsApplied = false;
    public virtual void OnHpTreshold() {
        if(TresholdCondition() && !areStatsApplied) {
            ApplyStats();
            areStatsApplied = true;
        } 
        else if(!TresholdCondition() && areStatsApplied) {
            RemoveStats();
            areStatsApplied = false;
        }
    }
    public override string GetDescription() {
        return GetThresholdString() + base.GetDescription();
    }
    public override string GetUpgrDesc() {
        return GetThresholdString() + base.GetUpgrDesc();
    }
    protected override bool TresholdCondition() {
        float currentResoucePercent = GetResoucePercent();
        return thresholdType switch {
            ThresholdType.Above => currentResoucePercent >= tresholdPercent,
            ThresholdType.Below => currentResoucePercent <= tresholdPercent,
            ThresholdType.Equal => currentResoucePercent == tresholdPercent,
            _ => false,
        };
    }
    //private (internal) methods
    private string GetThresholdString() {
        string percentString = "";
        string thresholdString = GetThresholdTypeString();
        if(thresholdString != "equal")
            percentString = " " + tresholdPercent + "%";
        else if(thresholdString == "equal" && (tresholdPercent == 100 || tresholdPercent == 0)) {
            if(tresholdPercent == 100)
                thresholdString = "full";
            else if(tresholdPercent == 0)
                thresholdString = "empty";
        }
        return "When " + GetResorceTypeString() + " is " + thresholdString + percentString + ":\n";
    }
    private string GetThresholdTypeString() {
        return thresholdType switch {
            ThresholdType.Above => "above",
            ThresholdType.Below => "below",
            ThresholdType.Equal => "equal",
            _ => "unknown",
        };
    }
    private string GetResorceTypeString() {
        return resourceType switch {
            ResourceType.Healthpoint => "HP",
            ResourceType.Manapoint => "MP",
            _ => "unknown",
        };
    }
    private float GetResoucePercent() {
        return resourceType switch {
            ResourceType.Healthpoint => (float)player.currHP / player.stats.healthpoint * 100,
            ResourceType.Manapoint => (float)player.currMp / player.stats.manapoint * 100,
            _ => 0,
        };
    }
}

public enum ThresholdType {
    Above,
    Below,
    Equal,
}
public enum ResourceType {
    Healthpoint,
    Manapoint,
}