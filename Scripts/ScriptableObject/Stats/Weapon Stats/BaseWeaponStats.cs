using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "ScriptableObjects/WeaponStats/WeaponBaseStats", order = 0)]
public class BaseWeaponStats : GenericStats
{
    [Header("Main Stats")]
    public int baseDamage = 0; //10
    public float knockback = 0f; //3
    public float atkCd = 0f; //0.5f //aka: attack cooldown
    public float weaponSize = 0f; //1f
    public float AttackSpeed { 
        get { 
            if(atkCd == 0)
                return 0;
            return 1 / atkCd; 
        } 
        set
        {
            atkCd = value;
            atkCd = 1f / AttackSpeed;
        }
    }
        
    public void AddToStats(BaseWeaponStats add) {
//        Debug.Log("Adding stats: " + add.name + " --> " + name);
        baseDamage += add.baseDamage;
        knockback += add.knockback;
        weaponSize += add.weaponSize;
        if(add.atkCd == 0) return;
        float addAtkSpd = atkCd * (add.AttackSpeed / 100f);
        //Debug.Log("atkCd: " + atkCd + " + " + addAtkSpd);
        atkCd += addAtkSpd; // Aggiunge AttackSpeed come percentuale di atkCd;
        //Debug.Log("newAtkSpd: " + AttackSpeed);
        if(atkCd < 0f) {
            atkCd = 0f;
            Debug.Log("Warning: atk cd can't be negative");
        }
        //WARNING --> weapon size currently doesn't work for enemies but only for player (do not add weapon size stats to enemies)
    }
    public void SubToStats(BaseWeaponStats sub) {
        baseDamage -= sub.baseDamage;
        knockback -= sub.knockback;
        weaponSize -= sub.weaponSize;
        if(sub.atkCd == 0) return;
        float addAtkSpd = atkCd * (sub.AttackSpeed / 100f);
        atkCd -= addAtkSpd;
        if(atkCd < 0f) {
            atkCd = 0f;
            Debug.Log("Warning: atk cd can't be negative");
        }
    }

    public BaseWeaponStats() {
        baseDamage = 10;
        knockback = 3f;
        atkCd = 0.5f;
        weaponSize = 1;
    }

    public BaseWeaponStats(int baseDamage, float knockback, float atkCd, float weaponSize) {
        this.baseDamage = baseDamage;
        this.knockback = knockback;
        this.atkCd = atkCd;
        this.weaponSize = weaponSize;
    }

    public BaseWeaponStats GetStatsDiff (BaseWeaponStats stats) {
        BaseWeaponStats bws = CreateInstance<BaseWeaponStats>().Copy(stats);
        bws.baseDamage = baseDamage - stats.baseDamage;
        bws.knockback = knockback - stats.knockback;
        bws.atkCd = atkCd + stats.atkCd;
        bws.weaponSize = weaponSize - stats.weaponSize;
        return bws;
    }

    private BaseWeaponStats Copy(BaseWeaponStats s) {
        baseDamage = s.baseDamage;
        knockback = s.knockback;
        atkCd = s.atkCd;
        weaponSize = s.weaponSize;
        return s;
    }

    public virtual string GetDescription(bool dontShowInfo = false) {
        string s = "";
        /*
        if(baseDamage != 0) {
            s += "Base damage " + IncreaseOrDecrease(baseDamage) 
            + ShowDetailsCheck(GetSignedValue(baseDamage),dontShowInfo) + ".\n";
        }
        if(knockback != 0) {
            string details = dontShowInfo ? " " : " by " + GetSignedValuePercent(knockback);
            s += "Knockback " + IncreaseOrDecrease(knockback) + details + ".\n";
        }
        if(AttackSpeed != 0) {
            string sign = AttackSpeed > 0 ? "decreased" : "increased";
            float atkspd = AttackSpeed > 0 ? AttackSpeed : -AttackSpeed;
            string details = dontShowInfo ? " " : " by " + atkspd + "%";
            s += "Attack speed " + sign + details + ".\n";
        }
        if(weaponSize != 0) {
            string sign = weaponSize > 0 ? "increased" : "decreased";
            float wpnsize = weaponSize > 0 ? weaponSize : -weaponSize;
            string details = dontShowInfo ? " " : " by " + wpnsize * 100 + "%";
            s += "Weapon size " + sign + details + ".\n";
        }
        */
        s += GetBaseDmgDesc(dontShowInfo);
        s += GetKnockbackDesc(dontShowInfo);
        s += GetAtkSpdDesc(dontShowInfo);
        s += GetWeaponSizeDesc(dontShowInfo);
        return s;
    }
    protected virtual string IncreaseOrDecrease(float value, bool isReversed = false) {
        if(!isReversed) 
            return value > 0 ? "increased" : "decreased";
        else
            return value > 0 ? "decreased" : "increased";
    }
    protected virtual string GetSignedValue(float value) {
        return value > 0 ? value.ToString() : (-value).ToString();
    }
    protected virtual string GetSignedValuePercent(float value, bool isMultipliedBy100 = true) {
        float newValue = isMultipliedBy100 ? (int)(value * 100) : (int)value;
        string ret = newValue > 0 ? newValue.ToString() : (-newValue).ToString();
        return ret + "%";
    }
    protected virtual string ShowDetailsCheck(string valueStr, bool dontShowInfo) {
        return dontShowInfo ? " " : " by " + valueStr;
    }
    //all stats desc
    protected virtual string GetBaseDmgDesc(bool dontShowInfo = false) {
        if(baseDamage == 0) return "";
        return "Base damage " + IncreaseOrDecrease(baseDamage)
        + ShowDetailsCheck(GetSignedValue(baseDamage), dontShowInfo) + ".\n";
    }
    protected virtual string GetKnockbackDesc(bool dontShowInfo = false) {
        if(knockback == 0) return "";
        return "Knockback " + IncreaseOrDecrease(knockback)
        + ShowDetailsCheck(GetSignedValuePercent(knockback), dontShowInfo) + ".\n";
    }
    protected virtual string GetAtkSpdDesc(bool dontShowInfo = false) {
        if(AttackSpeed == 0) return "";
        return "Attack speed " + IncreaseOrDecrease(AttackSpeed, true)
        + ShowDetailsCheck(GetSignedValuePercent(AttackSpeed, false), dontShowInfo) + ".\n";
    }
    protected virtual string GetWeaponSizeDesc(bool dontShowInfo = false) {
        if(weaponSize == 0) return "";
        return "Weapon size " + IncreaseOrDecrease(weaponSize) 
        + ShowDetailsCheck(GetSignedValuePercent(weaponSize), dontShowInfo) + ".\n";
    }
    public void SetWeaponStats(int baseDamage, float knockback, float atkCd, float weaponSize) {
        this.baseDamage = baseDamage;
        this.knockback = knockback;
        this.atkCd = atkCd;
        this.weaponSize = weaponSize;
    }
}
