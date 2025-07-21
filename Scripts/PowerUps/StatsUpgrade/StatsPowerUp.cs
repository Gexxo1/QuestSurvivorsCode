using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class StatsPowerUp : PowerUp
{
    [SerializeField] StatsUp statToAdd;
    protected virtual void Start() {
        StatIncrease();
    }
    public override void onUpgrade() {
        StatIncrease();
    }
    private void StatIncrease() {
        player.AddStats(statToAdd);
    }
    public override string GetDescription() {
        description = statToAdd.GetUpgradeDescription();
        return base.GetDescription();
    }
    public override string GetUpgrDesc() {
        return GetDescription();
    }
}
