using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldObjective", menuName = "ScriptableObjects/Objectives/GoldObjective")]
public class GoldObjective : Objective
{
    public int goldTreshold;
    public override bool IsConditionTrue(int value) {
        return value >= goldTreshold;
    }
    public override string GetReqDesc() {
        return "Obtain [" + goldTreshold + "] gold in a run";
    }
    public override string GetTitle() {
        return "Per-Run Gold Collection " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "goldcollect_" + id; }
    //public override string GetId() { return base.GetId() + "goldcollect_" + goldTreshold; }
}
