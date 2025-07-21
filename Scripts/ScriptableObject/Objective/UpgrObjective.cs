using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeObjective", menuName = "ScriptableObjects/Objectives/UpgradeObjective")]
public class UpgrObjective : Objective
{
    public int upgrTreshold;
    public override bool IsConditionTrue(int value) {
        return value >= upgrTreshold;
    }
    public override string GetReqDesc() {
        return "Upgrade character at " + upgrTreshold;
    }

    public override string GetId() { return base.GetId() + "upgrchar_" + upgrTreshold; }
}
