using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUpgradePowerup : PowerUp
{
    [Header("Dash Upgrade")]
    protected DashBase ability;
    [SerializeField] private float cdDecreasePercent;
    [SerializeField] private float durationIncrease;
    protected virtual void Start() {
        ability = player.inventory.GetDash();
        ability.UpgradeDash(cdDecreasePercent, durationIncrease);
    }
    public override void onUpgrade() {
        ability.UpgradeDash(cdDecreasePercent, durationIncrease);
    }

    public override string GetDescription() {
        return "Dash cooldown reduced by " + cdDecreasePercent*100 + "% \n" +
                "Dash invicibility frames increased by " + durationIncrease*100 + "%";
    }
    public override string GetUpgrDesc() {
        return GetDescription();
    }
}
