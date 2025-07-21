using UnityEngine;

[CreateAssetMenu(fileName = "TotalPlaytimeObjective", menuName = "ScriptableObjects/Objectives/TotalPlaytime")]
public class TotalPlaytimeObjective : Objective
{
    public int playtimeThreshold;
    public override bool IsConditionTrue(int value) {
        return value >= playtimeThreshold;
    }
    public override string GetReqDesc() {
        return "Accumulate a total [" + playtimeThreshold + "] of playtime";
    }
    public override string GetTitle() {
        return "Playtime " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "goldcollect_" + id; }
    //public override string GetId() { return base.GetId() + "goldcollect_" + goldTreshold; }
}
