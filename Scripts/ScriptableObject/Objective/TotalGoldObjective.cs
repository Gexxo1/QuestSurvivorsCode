using UnityEngine;

[CreateAssetMenu(fileName = "ToatalGoldObjective", menuName = "ScriptableObjects/Objectives/TotalGold")]
public class TotalGoldObjective : Objective
{
    public int goldTreshold;
    public override bool IsConditionTrue(int value) {
        return value >= goldTreshold;
    }
    public override string GetReqDesc() {
        return "Obtain a total of [" + goldTreshold + "] gold (spending gold does not affect this objective)";
    }
    public override string GetTitle() {
        return "Total Gold Collection " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "totalgoldcollect_" + id; }
    //public override string GetId() { return base.GetId() + "goldcollect_" + goldTreshold; }
}
