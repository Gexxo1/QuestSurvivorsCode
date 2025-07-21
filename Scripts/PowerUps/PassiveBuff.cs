using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBuff : PowerUp
{
    [SerializeField] private List<StatusEffectData> buffs;
    private List<StatusEffectData> buffsInstances;
    private void Start() {
        buffsInstances = new List<StatusEffectData>();
        foreach(StatusEffectData buff in buffs) {
            StatusEffectData newBuff = Instantiate(buff);
            buffsInstances.Add(newBuff);
            GameManager.instance.player.ApplyStatus(newBuff);
        }
    }
    public override void onUpgrade() { 
        //First remove all the buffs
        foreach(StatusEffectData buff in buffs)
            GameManager.instance.player.RemoveStatus(buff);
        //Then re-apply the buffs
        for(int i=0; i < buffs.Count && i < buffsInstances.Count; i++) {
            StatusEffectData upgr = buffsInstances[i];
            upgr.addToStats(buffs[i]);
            GameManager.instance.player.ApplyStatus(upgr);
        }
    }
    public override string GetDescription() {
        string s = "Self-";
        foreach(StatusEffectData se in buffs) 
            s += se.GetDescription();
        return s;
    }
    public override string GetUpgrDesc() {
        string s = "";
        foreach(StatusEffectData se in buffs) 
            s += se.GetUpgradeDescription();
        return s;
    }
}
