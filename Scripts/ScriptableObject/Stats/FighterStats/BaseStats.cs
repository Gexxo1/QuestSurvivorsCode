using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats/BaseStats")]
public class BaseStats : GenericStats
{
    [Header("Primary Stats")]
    public int healthpoint = 0;
    public float moveSpeed = 0; 
    public int armor = 0;
    public float GetStatsValueByNames(string name) {
        FieldInfo field = GetType().GetField(name);
        Debug.Log("field: " + field + " value: " + (float)field.GetValue(this));
        return (float)field.GetValue(this);
    }
    public bool AreStatsGreaterOrEqual(BaseStats confront) {
        if (healthpoint < confront.healthpoint) return false;
        if (moveSpeed < confront.moveSpeed) return false;
        if (armor < confront.armor) return false;
        return true;
    }
    protected float FixedPercent(int percent) {
        return ((float)percent + 100) / 100; //si aggiunge 100 perchè si assume che è una percentuale aggiunta
    }

    public virtual string GetUpgradeDescription(bool showInfo = true) {
        string s = "";
        if(healthpoint != 0) {
            string sign = healthpoint > 0 ? "increased" : "decreased";
            int h = healthpoint > 0 ? healthpoint : -healthpoint;
            string details = showInfo ? " by " + h : "";
            s += "Max health " + sign + details + ".\n";
        }
        if(moveSpeed != 0) {
            string sign = moveSpeed > 0 ? "increased" : "decreased";
            float ms = moveSpeed > 0 ? moveSpeed * 100 : -moveSpeed * 100;
            string details = showInfo ? " by " + ms + "%" : "";
            s += "Move speed " + sign + details + ".\n";
        }
        if(armor != 0) {
            string sign = armor > 0 ? "increased" : "decreased";
            int a = armor > 0 ? armor : -armor;
            string details = showInfo ? " by " + a : "";
            s += "Armor " + sign + details + ".\n";
        }
        return s;
    }

    public BaseStats AddToStats(BaseStats add) {
        healthpoint += add.healthpoint;
        moveSpeed += add.moveSpeed;
        armor += add.armor;
        return this;
    }

    public BaseStats SubToStats(BaseStats sub) {
        healthpoint -= sub.healthpoint;
        moveSpeed -= sub.moveSpeed;
        armor -= sub.armor;
        return this;
    }
    /*
    public override string ToString() {
        string s = "BaseStats:\n";
        s += "Health Points: " + healthpoint + "\n";
        s += "Move Speed: " + moveSpeed + "\n";
        s += "Armor: " + armor + "\n";
        return s;
    }
    */
}
