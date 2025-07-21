using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KillsObjective", menuName = "ScriptableObjects/Objectives/KillsObjective")]
public class KillsObjective : Objective
{
    public int killsTreshold;

    public override bool IsConditionTrue(int value) {
        return value >= killsTreshold;
    }
    public override string GetReqDesc() {
        return "Kill [" + killsTreshold + "] enemies in a run";
    }
    public override string GetTitle() {
        return "Per-Run Total Kills " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "kills_" + id; }
}
