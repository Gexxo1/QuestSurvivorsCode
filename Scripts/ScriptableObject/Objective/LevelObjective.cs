using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelObjective", menuName = "ScriptableObjects/Objectives/LevelObjective")]
public class LevelObjective : Objective
{
    public int levelTreshold;
    public override bool IsConditionTrue(int value) {
//        Debug.Log("LevelObjective: " + value + " >= " + levelTreshold);
        return value >= levelTreshold;
    }
    public override string GetReqDesc()
    {
        return "Reach level [" + levelTreshold + "] in a run";
    }
    public override string GetTitle() {
        return "Per-Run Level Reached " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "lvl_" + id; }
    //public override string GetId() { return base.GetId() + "lvl_" + levelTreshold; }
}
