using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaObjective", menuName = "ScriptableObjects/Objectives/ManaObjective")]
public class ManaUsageObjective : Objective
{
    [SerializeField] int requiredMana;

    public override bool IsConditionTrue(int value)  {
        if(requiredMana == 0)
            return value == 0;
        return value >= requiredMana;
    }
    public override string GetReqDesc() {
        if(requiredMana == 0)
            return "Don't use any mana in a run";
        return "Use at least [" + requiredMana + "] mana in a run";
    }
    public override string GetTitle() {
        return "Mana Usage " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "mana_" + id; }
}
