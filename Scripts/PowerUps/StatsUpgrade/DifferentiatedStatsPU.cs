using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentiatedStatsPU : StatsPowerUp
{
    [Header("Differentiated Stats")]
    [SerializeField] StatsUp[] statsToAdd;
    /* explained
tiers   --> 0   -   1   -   2   -   3   -   4
            base    [0]     [1]     [2]     [3]
    */
    public override void onUpgrade() {
        if(tier < 1)
            base.onUpgrade();
        else
            DifferentiatedStatIncrease(tier-1);
    }

    private void DifferentiatedStatIncrease(int index) {
        //Debug.Log("Adding index [" + index + "] stat: " + statsToAdd[index]);
        GameManager.instance.player.AddStats(statsToAdd[index]);
    }

    public override string GetUpgrDesc() {
        //if(tier < 1) return base.GetUpgrDesc();
        //Debug.Log("UpgrDesc tier " + tier);
        //if(tier < 1) return statsToAdd[0].GetUpgradeDescription();
        return statsToAdd[tier].GetUpgradeDescription();
    }
}
