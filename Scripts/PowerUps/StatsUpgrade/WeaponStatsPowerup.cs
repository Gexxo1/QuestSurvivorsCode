using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//non confondere con weaponpowerup, questo è un powerup che aumenta le stats dell'arma (non ha la weapon)
public class WeaponStatsPowerup : PowerUp
{
    [SerializeField] WeaponStats statToAdd;
    [SerializeField] WeaponStats statUpgrade;
    protected virtual void Start() {
        //currWeaponStats = GameManager.istance.player.inventory.getCurrWeapon().stats;
        StatIncrease(statToAdd);
    }
    public override void onUpgrade() {
        if(statUpgrade != null)
            StatIncrease(statUpgrade);
        else
            StatIncrease(statToAdd);
    }
    private void StatIncrease(WeaponStats stats) {
        GameManager.instance.player.AddToGlobalWpnStats(stats);
    }
    public override string GetDescription() {
        return statToAdd.GetDescription();
    }
    public override string GetUpgrDesc() {
        if(statUpgrade != null)
            return statUpgrade.GetUpgradeDescription();
        else
            return statToAdd.GetDescription();
    }
}
