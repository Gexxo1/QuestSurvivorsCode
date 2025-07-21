using UnityEngine;

[CreateAssetMenu(fileName = "HitObjective", menuName = "ScriptableObjects/Objectives/HitObjective")]
public class HitObjective : Objective
{
    public int damageThreshold;
    public override bool IsConditionTrue(int totalNumberOfDamage) {
        return totalNumberOfDamage >= damageThreshold;
    }
    public override string GetReqDesc() {
        if(damageThreshold == 0)
            return "Take no damage in a run";
        return "Take no less than [" + damageThreshold + "] damage in a run";
    }
    public override string GetTitle() {
        return "Damage Taken " + base.GetTitle();
    }
    public override string GetId() { return base.GetId() + "hit_" + id; }
    //public override string GetId() { return base.GetId() + "hit_" + damageThreshold; }
}
