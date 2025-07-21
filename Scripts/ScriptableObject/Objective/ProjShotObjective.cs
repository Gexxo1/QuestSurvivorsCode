using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ProjShotObjective", menuName = "ScriptableObjects/Objectives/ProjsShotObjective")]
public class ProjShotObjective : Objective
{
    [SerializeField] int shotsNeeded; //shots needed to complete objective
    public override bool IsConditionTrue(int value) { //the true condition is handled not there but in the ObjectiveManager
        return value >= shotsNeeded;
    }
    public override string GetReqDesc() {
        return "Shoot [" + shotsNeeded + "] projectiles in a run";
    }
    public override string GetTitle() {
        return "Bullet Hell " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "projshot_" + id; }
}
