using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats/AdvancedStats")]
public class Stats : BaseStats
{
    //IMPORTANTE!!! --> Quando si aggiunge una nuova stat dobbiamo aggiornare "stats add" e "getIncreasedValues"
    
    [Header("Player only & Powerups")]
    public int manapoint = 0;
    public int bonusDmgPercent = 0;
    [Range(0, 100)] public int critRatePercent = 0;
    public int critDamagePercent = 0; 
    public int luckPercent = 0;
    public int addXpGainPercent = 0;
    public int manapointGain = 0;
    public int thornsPercent = 0; //is not a chance --> but a percentage of reflection of the original damage
    [Range(0, 100)] public int lifestealPercent = 0;
    public int resCount = 0;
    public int rerollCount = 0;
    public int armorPen = 0;
    public int armorRegenPercentReduction = 0;
    public float GetArmorRegenReduction(float baseArmorRegen) {
        return baseArmorRegen - baseArmorRegen * (1 - FixedPercent(armorRegenPercentReduction));
    }
    public float GetFixedDamagePercent() {
        return FixedPercent(bonusDmgPercent);
    }
    public float getFixedCritDamagePercent() {
        return FixedPercent(critDamagePercent);
    }
    public float getFixedXpPercent() { 
        return FixedPercent(addXpGainPercent);
    }
    public int GetThornsDamage(int damage) {
        return (int)(damage * ((float)thornsPercent / 100));
    }
    public override string GetUpgradeDescription(bool showInfo = true) {
        string s = base.GetUpgradeDescription(showInfo);
        if(manapoint != 0) {
            string sign = manapoint > 0 ? "increased" : "decreased";
            int m = manapoint > 0 ? manapoint : -manapoint;
            string details = showInfo ? " by " + m : "";
            s += "Mana " + sign + details + ".\n";
        }
        if(bonusDmgPercent != 0) {
            string sign = bonusDmgPercent > 0 ? "increased" : "decreased";
            int dmgp = bonusDmgPercent > 0 ? bonusDmgPercent : -bonusDmgPercent;
            string details = showInfo ? " by " + dmgp + "%" : "";
            s += "Damage " + sign + details + ".\n";
        }
        if(critRatePercent != 0) {
            string sign = critRatePercent > 0 ? "increased" : "decreased";
            int crate = critRatePercent > 0 ? critRatePercent : -critRatePercent;
            string details = showInfo ? " by " + crate : "";
            s += "Crit chance " + sign + details + "%.\n";
        }
        if(critDamagePercent != 0) {
            string sign = critDamagePercent > 0 ? "increased" : "decreased";
            int cdmg = critDamagePercent > 0 ? critDamagePercent : -critDamagePercent;
            string details = showInfo ? " by " + cdmg + "%" : "";
            s += "Crit damage " + sign + details + ".\n";
        }
        if(luckPercent != 0) {
            string sign = luckPercent > 0 ? "increased" : "decreased";
            int lck = luckPercent > 0 ? luckPercent : -luckPercent;
            string details = showInfo ? " by " + lck + "%" : "";
            s += "Coins drop chance " + sign + details + ".\n";
        }
        if(addXpGainPercent != 0) {
            string sign = addXpGainPercent > 0 ? "increased" : "decreased";
            int xpgain = addXpGainPercent > 0 ? addXpGainPercent : -addXpGainPercent;
            string details = showInfo ? " by " + xpgain + "%" : "";
            s += "Xp gain " + sign + details + ".\n";
        }
        if(manapointGain != 0) {
            string sign = manapointGain > 0 ? "increased" : "decreased";
            int mg = manapointGain > 0 ? manapointGain : -manapointGain;
            string details = showInfo ? " by " + mg : "";
            s += "Mana gain " + sign + details + ".\n";
        }
        if(thornsPercent != 0) {
            string sign = thornsPercent > 0 ? "increased" : "decreased";
            int tp = thornsPercent > 0 ? thornsPercent : -thornsPercent;
            string details = showInfo ? " by " + tp : "";
            s += "Damage Reflection Percent " + sign + details + "%.\n";
            //s += "Enemies now take " + thornsPercent + "% " + sign + " of the damage they deal to you.\n";
        }
        if(lifestealPercent != 0) {
            string sign = lifestealPercent > 0 ? "increased" : "decreased";
            int lp = lifestealPercent > 0 ? lifestealPercent : -lifestealPercent;
            string details = showInfo ? " by " + lp : "";
            s += "Lifesteal chance " + sign + details + "%.\n";
        }
        if(resCount != 0) {
            string sign = resCount > 0 ? "increased" : "decreased";
            int resc = resCount > 0 ? resCount : -resCount;
            string details = showInfo ? " by " + resc : "";
            s += "Resurrections number " + sign + details + ".\n";
        }
        if(rerollCount != 0) {
            string sign = rerollCount > 0 ? "increased" : "decreased";
            int rerollc = rerollCount > 0 ? rerollCount : -rerollCount;
            string details = showInfo ? " by " + rerollc : "";
            s += "Available rerolls " + sign + details + ".\n";
        }
        if(armorPen != 0) {
            string sign = armorPen > 0 ? "increased" : "decreased";
            int signArmPen = armorPen > 0 ? armorPen : -armorPen;
            string details = showInfo ? " by " + signArmPen + " points" : "";
            s += "Armor penetration " + sign + details + ".\n";
        }
        if(armorRegenPercentReduction != 0) {
            string sign = armorRegenPercentReduction > 0 ? "increased" : "decreased";
            int armReg = armorRegenPercentReduction > 0 ? armorRegenPercentReduction : -armorRegenPercentReduction;
            string details = showInfo ? " by " + armReg + "%" : "";
            s += "Armor regen " + sign + details + ".\n";
        }
        return s;
    }

    public Stats AddToStats(Stats add) {
        base.AddToStats(add);
        manapoint += add.manapoint;
        bonusDmgPercent += add.bonusDmgPercent;
        critRatePercent += add.critRatePercent;
        critDamagePercent += add.critDamagePercent;
        luckPercent += add.luckPercent;
        addXpGainPercent += add.addXpGainPercent;
        manapointGain += add.manapointGain;
        thornsPercent += add.thornsPercent;
        lifestealPercent += add.lifestealPercent;
        resCount += add.resCount;
        rerollCount += add.rerollCount;
        armorPen += add.armorPen;
        armorRegenPercentReduction += add.armorRegenPercentReduction;
        return this;
    }
    public Stats SubToStats(Stats sub) {
        base.SubToStats(sub);
        manapoint -= sub.manapoint;
        bonusDmgPercent -= sub.bonusDmgPercent;
        critRatePercent -= sub.critRatePercent;
        critDamagePercent -= sub.critDamagePercent;
        luckPercent -= sub.luckPercent;
        addXpGainPercent -= sub.addXpGainPercent;
        manapointGain -= sub.manapointGain;
        thornsPercent -= sub.thornsPercent;
        lifestealPercent -= sub.lifestealPercent;
        resCount -= sub.resCount;
        rerollCount -= sub.rerollCount;
        armorPen -= sub.armorPen;
        armorRegenPercentReduction -= sub.armorRegenPercentReduction;
        return this;
    }

    public List<double> GetStatsList(int tier) {
        List<double> list = new();
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        //int index = 0;
        foreach (FieldInfo field in fields) {
            object value = field.GetValue(this);
            //if (value is int vi && vi == 0) continue;
            //if (value is float vf && vf == 0f) continue;
            //if (value is double vd && vd == 0.0) continue;
            
            if (value is int vi) 
                if(vi == 0) continue;
                else list.Add(vi * tier);
            if (value is float vf) {
                if(field.Name == "moveSpeed")
                    vf *= 100;
                if(vf == 0f) continue;
                else list.Add(vf * tier);
            }
            if (value is double vd)
                if(vd == 0.0) continue;
                else list.Add(vd * tier);
            

//            Debug.Log("name: " + field.Name + " value:" + field.GetValue(this));
            //index++;
        }
        return list;
    } 

    public string GetStatsListValuesToString(int tier) {
        string s = "";
        List<double> list = GetStatsList(tier+1);
        foreach(double d in list) 
            s += d + " ";
        return s;
    }
    public List<string> GetStatsStringList() {
        List<string> list = new();
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (FieldInfo field in fields)         
            list.Add(field.Name);
        
        return list;
    } 
}
