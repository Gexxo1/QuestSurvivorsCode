using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotsObjective", menuName = "ScriptableObjects/Objectives/PotsObjective")]
public class PotObjective : Objective
{
    public int potBreakTreshold;
    public override bool IsConditionTrue(int value) { //il parametro c'è ma non deve essere inizializzato
        return value >= potBreakTreshold;
    }
    public override string GetReqDesc() {
        return "Break [" + potBreakTreshold + "] pots in a run";
    }
    public override string GetTitle() {
        return "Per-Run Broken Pots " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "pot_" + id; }
}
