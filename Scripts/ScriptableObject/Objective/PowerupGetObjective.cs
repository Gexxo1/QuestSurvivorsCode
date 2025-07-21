using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerupObjective", menuName = "ScriptableObjects/Objectives/PowerupObjective")]
public class PowerupGetObjective : Objective
{
    public List<PowerUp> neededPowerups; //powerup needed to accomplish the objective
    public override bool IsConditionTrue(int value) { //the true condition is handled not there but in the ObjectiveManager
        return value >= neededPowerups.Count;
    }
    public override string GetReqDesc() {
        string puListString = "";
        foreach (PowerUp powerup in neededPowerups)
            puListString +=  "[" + powerup.title + "] ";
        return "Get all of the following powerups in a run: " + puListString;
    }
    public override string GetTitle() {
        return "Build Master " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "pu_" + id; }
}
