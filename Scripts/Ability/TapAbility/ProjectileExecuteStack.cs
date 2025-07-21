using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExecuteStack : ThrowProjectileStats {
    [Header("Stack Stats")]
    private int stackCounter = 0;
    public List<Stack> stacks;
    protected override void Awake() {
        base.Awake();
        PowerupManager.OnEnemyExecuted += OnExecute;
    }
    void OnDestroy() {
        PowerupManager.OnEnemyExecuted -= OnExecute;
    }

    public void OnExecute(HitSource hitSource) {
        
        string projectileName = projectile.name;
        //rimuovo il (Clone) e gli spazi
        string receivedBulletName = hitSource.name.Replace("(Clone)", "").Trim();
        if(projectileName != receivedBulletName)
            return;
        
        //Debug.Log("BloodDart Execute Effect");

        stackCounter++;
        MenuManager.instance.UpdateStackCounter(stackCounter);

        foreach (Stack stack in stacks) {
            if(stackCounter == 0)
                continue;
            if (stack.everyStacks % stackCounter == 0) {
                if(stack.increasedStats != null)
                    player.AddStats(stack.increasedStats);
                if(stack.increasedStats2 != null)
                    player.inventory.AddStatsToMainWeapon(stack.increasedStats2);
            }
        }
        
        
    }

    public int GetStackCounter() {
        return stackCounter;
    }

    public override string GetDescription() {
        string stacksDesc = "";
        foreach (Stack stack in stacks) {
            string increasedStats = stack.increasedStats != null ? stack.increasedStats.GetUpgradeDescription() : "";
            string increasedStats2 = stack.increasedStats2 != null ? stack.increasedStats2.GetUpgradeDescription() : "";
            stacksDesc += "Every " + stack.everyStacks + " stacks:\n" + increasedStats + " " + increasedStats2;
        }
        return description + "\n" + stacksDesc + GetRequirements();
    }
}

[Serializable]
public class Stack {
    public StatsUp increasedStats;
    public WeaponStats increasedStats2;
    public int everyStacks;
}