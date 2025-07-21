using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Text;
using System;

public abstract class GenericStats : ScriptableObject
{
    public virtual string GetAutoDescription() {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance); //note: BindingFlags.NonPublic removed --> now finds only public fields
        //Utility.PrintArray(fields);
        string s = "";
        //int index = 0;
        foreach (FieldInfo field in fields) {
            object value = field.GetValue(this);
            string name = field.Name;
            if(!(value is int || value is double || value is float))
                continue;

            string percent = "";
            if(name.Contains("Percent")) //if it's a percent value
                percent = "%";
            if(name == "moveSpeed" || name == "weaponSize") { //if it's visual only percent value
                percent = "%";
                value = ((float)value) * 100;
            }

            name = GetStatNameReplacement(name);

            if (value is int vi) {
                if(vi == 0) continue;
                string sign = vi > 0 ? "increased" : "decreased";
                vi = vi < 0 ? -vi : vi;
                s += name + " " + sign + " by " + vi + percent + ".\n";
            }
            else if (value is float vf) {
                if(vf == 0f) continue;
                string sign = vf > 0 ? "increased" : "decreased";
                vf = vf < 0 ? -vf : vf;
                s += name + " " + sign + " by " + vf + percent + ".\n";
            }
            else if (value is double vd) {
                if(vd == 0.0) continue;
                string sign = vd > 0 ? "increased" : "decreased";
                vd = vd < 0 ? -vd : vd;
                s += name + " " + sign + " by " + vd + ".\n";
            }
        }
        return s;
    } 
    //only manual thing
    private string GetStatNameReplacement(string s) {
        return s switch
        {
            //player stats
            "healthpoint" => "Max Health",
            "moveSpeed" => "Move Speed",
            "armor" => "Armor", //or "Damage Block"?
            "lifestealPercent" => "Lifesteal Rate",
            "poisonDuration" => "Poison Duration",
            "manapoint" => "Max Mana",
            "bonusDmgPercent" => "Damage",
            "critRatePercent" => "Crit Chance",
            "critDamagePercent" => "Crit Damage Percent",
            "luckPercent" => "Coins Drop Rate",
            "addXpGainPercent" => "Experience Gain",
            "manapointGain" => "Mana Gain",
            "thornsPercent" => "Damage Reflection Percent",
            "resCount" => "Resurrection Count",
            "rerollCount" => "Reroll Count",
            //weapon stats
            "baseDamage" => "Base Damage",
            "knockback" => "Knockback",
            "atkCd" => "Attack Cooldown", "AttackSpeed" => "Attack Speed",
            "projectileSpeed" => "Projectile Speed",
            "weaponSize" => "Weapon Size",
            "projectilePierce" => "Projectile Pierce",
            "projectileBounce" => "Projectile Bounce",
            "projectileNumber" => "Projectile Number",
            //status effect data
            "statusName" => "Status Name",
            "duration" => "Duration",
            "damageOverTime" => "Damage Over Time",
            "slowAmountPercent" => "Slow Amount",
            "color" => "Color",
            _ => s,
        };
    }
    //WARNING: 'this' must be instantiated first, as it will be overwritten
    public GenericStats AddToStatsGeneric(GenericStats addStats) {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        FieldInfo[] addFields = addStats.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

        //Debug.Log("Initial stats: " + this.ToString());
        //Debug.Log("Add stats: " + addStats.ToString());
        foreach (FieldInfo field in fields) {
            object value = field.GetValue(this);
            string name = field.Name;
            foreach (FieldInfo addField in addFields) {
                object addValue = addField.GetValue(addStats);
                string addName = addField.Name;
                if (name == addName && addValue != null) {
                    if (value is int vi && addValue is int vi2) 
                        if (vi2 != 0) 
                            field.SetValue(this, vi + vi2);
                    else if (value is float vf && addValue is float vf2)  
                        if (vf2 != 0f)
                            field.SetValue(this, vf + vf2);
                    else if (value is double vd && addValue is double vd2)
                        if (vd2 != 0.0)
                            field.SetValue(this, vd + vd2);
                }
            }
        }
        return this;
    }
    
    public void NullAttributesWarning() {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (FieldInfo field in fields) 
            if (field.GetValue(this) == null && field.FieldType.GetGenericTypeDefinition() != typeof(List<>))
                Debug.LogWarning("Field [" + field.Name + "] is null");
    }
    
    public override string ToString() {
        string s = "";
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        foreach (FieldInfo field in fields) {
            object value = field.GetValue(this);
            if(value == null) continue; //if value null --> skip
            if(value is int vi && vi == 0) continue; //if value is 0 --> skip
            if(value is float vf && vf == 0f) continue; //if value is 0f --> skip
            if(value is double vd && vd == 0.0) continue; //if value is 0.0 --> skip

            if (value is IList list) { //if it's a list
                s += "List { ";
                foreach (object o in list) 
                    s += o.ToString() + " ";
                s += "}\n";
                continue;
            }
            string name = field.Name;
            s += "[" + value + "] " + name + "\n";
        }
        if (s.EndsWith("\n")) s = s.Remove(s.Length - 1); //togli a capo finale
//        Debug.Log(s);
        return s;
    } 

}
