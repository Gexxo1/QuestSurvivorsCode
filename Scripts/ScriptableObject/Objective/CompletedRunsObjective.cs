using UnityEngine;

[CreateAssetMenu(fileName = "CompletedRunsObjective", menuName = "ScriptableObjects/Objectives/CompletedRuns")]
public class CompletedRunsObjective : Objective
{
    public int runsThreshold;
    public override bool IsConditionTrue(int value) {
        return value >= runsThreshold;
    }
    public override string GetReqDesc() {
        return "Complete [" + runsThreshold + "] runs succesfully";
    }
    public override string GetTitle() {
        return "Run Completion " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "runcomplete_" + id; }
    //public override string GetId() { return base.GetId() + "goldcollect_" + goldTreshold; }
}
