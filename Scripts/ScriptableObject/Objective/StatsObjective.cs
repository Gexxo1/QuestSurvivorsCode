using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsObjective", menuName = "ScriptableObjects/Objectives2/StatsObjective")]
public class StatsObjective : Objective
{
    public BaseStats statTreshold;
    public override bool IsConditionTrue(int value) {
        return true; //dont use this
    }
    public override string GetReqDesc() {
        return "Achieve " + statTreshold + " in a run";
    }
    public override string GetTitle() {
        return "Stat Master " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "statsreq_" + id; }
    //public override string GetId() { return base.GetId() + "goldcollect_" + goldTreshold; }
}
