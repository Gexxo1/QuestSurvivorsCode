using UnityEngine;

[CreateAssetMenu(fileName = "TotalKillsObjective", menuName = "ScriptableObjects/Objectives/TotalKills")]
public class TotalKillsObjective : Objective
{
    public int totalKillsThreshold;
    public override bool IsConditionTrue(int value) {
        return value >= totalKillsThreshold;
    }
    public override string GetReqDesc() {
        return "Slay a total of [" + totalKillsThreshold + "] enemies";
    }
    public override string GetTitle() {
        return "Total Kills " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "goldcollect_" + id; }
    //public override string GetId() { return base.GetId() + "goldcollect_" + goldTreshold; }
}
