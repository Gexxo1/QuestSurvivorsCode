using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdAndPermanentBuff : TresholdBuff {
    [Header("Permanent Stat Upgrade")]
    [SerializeField] StatsUp permaStatToAdd;
    protected override void Start() {
        base.Start();
        StatIncrease();
    }
    public override void onUpgrade() {
        base.onUpgrade();
        StatIncrease();
    }
    private void StatIncrease() {
        player.AddStats(permaStatToAdd);
    }
    public override string GetDescription() {
        return permaStatToAdd.GetUpgradeDescription() + base.GetDescription();
    }
    public override string GetUpgrDesc() {
        return permaStatToAdd.GetUpgradeDescription() + base.GetUpgrDesc();
    }
}
