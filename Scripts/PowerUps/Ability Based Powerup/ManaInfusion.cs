using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaInfusion : PowerupAbility {
    [SerializeField] float costReducedPerTier = 0.1f;
    float costReducedSum = 0;
    protected override void Awake() {
        base.Awake();
        PowerupManager.OnAbilityCast += OnCast;
    }
    void OnDestroy() {
        PowerupManager.OnAbilityCast -= OnCast;
    }
    public override void onUpgrade() {
        base.onUpgrade();
        costReducedSum += costReducedPerTier;
    }
    public void OnCast(Player player, int manaCost) {
        //Debug.Log("ManaInfusion OnCast " + manaCost + " - " + costReduced);
        int reducedCost = (int)(manaCost * (1 - costReducedSum)); // Calcola il nuovo costo del mana
        if(player.currHP - reducedCost < 0)
            player.currHP = 1;
        else
            player.currHP -= reducedCost;
        GameManager.instance.OnHitpointChange();
    }

    public override string GetUpgrDesc() {
        return "Hp loss reduced by " + costReducedPerTier*100 + "%";
    }
}
