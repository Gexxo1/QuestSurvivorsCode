using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "ScriptableObjects/WeaponStats/ProjectileWeaponStats", order = 1)]
public class WeaponStats : BaseWeaponStats
{
    //le stat default sono esattamente quelle della golden sword
    [Header("WeaponProjectile Stats")]
    public float projectileSpeed = 0f; //10f
    public int projectilePierce = 0; 
    public int projectileNumber = 0; 
    public int projectileBounce = 0; 
    [HideInInspector] public List<StatusEffectData> statusEffects; //instantiated status effects
    [SerializeField] private List<StatusEffectData> originalStatusEffects; //stats effects initialized in inspector
    [SerializeField] public StatusEffectData statusEffectUpgrade;
    public float projectileHoming = 0;
    public bool isHoming { get { return projectileHoming > 0; } }
    public string GetStatusEffectsString() {
        string s = "";
        s += "Status Effects: \n";
        foreach(StatusEffectData se in originalStatusEffects) 
            s += se.statusName + " ";
        if(originalStatusEffects.Count == 0)
            s += "No status effects\n";
        
        s += "Status Effect Upgrade: \n";
        if(statusEffectUpgrade != null)
            s += statusEffectUpgrade;
        else
            s += "No status effect upgrade\n";
        
        return s;
    }

    public string GetStatsString() {
        return GetMainStatsString() + "\n" + GetSpecialStatsString() + "\n" + GetStatusEffectsString();
    }
    public string GetMainStatsString() { return "Damage: " + baseDamage + "\nKnockback: " + knockback + "\nAttack Speed: " + AttackSpeed + "\nProjectile Speed: " + projectileSpeed; }
    public string GetSpecialStatsString() {
        string h = isHoming ? "Homing\n" : "";
        return "Size: " + weaponSize + "\nPierce: " + projectilePierce + "\nNumber: " + projectileNumber + "\nBounce: " + projectileBounce + "\n" + h;
    }

    public void AddToStats(WeaponStats add) {
        base.AddToStats(add);
        projectileSpeed += add.projectileSpeed;
        //GameManager.istance.player.inventory.AddWeaponSize(add.weaponSize*Vector3.one);
        projectilePierce += add.projectilePierce;
        projectileNumber += add.projectileNumber;
        projectileBounce += add.projectileBounce;
        projectileHoming += add.projectileHoming;
        HandleStatusEffectUpgrade(add);
    }

    private void HandleStatusEffectUpgrade(WeaponStats add) {
        if (add.statusEffectUpgrade == null && add.originalStatusEffects == null) return;
        //case 1. it adds an upgrade of status effects
        if(add.statusEffectUpgrade != null) {
            //case 1.1. i don't have a status effect upgrade
            if(statusEffectUpgrade == null)
                statusEffectUpgrade = Instantiate(add.statusEffectUpgrade);
            else //case 1.2. i already have a status effect upgrade
                statusEffectUpgrade.addToStats(add.statusEffectUpgrade);
        }
        //create a new statuseffects list if it is null
        statusEffects ??= new List<StatusEffectData>();
        //Debug.Log("Status Effects: " + add.originalStatusEffects);
        //case 2. it adds a status effect
        
        foreach(StatusEffectData addse in add.originalStatusEffects) {
            bool isNew = true;
            foreach(StatusEffectData se in statusEffects) {
                //case 2.1. isnotnew: i already have the status effect in my stats with the same name
                if(se.statusName == addse.statusName) {
//                    Debug.Log("Adding status effect: " + addse.statusName + " --> " + se.statusName);
                    se.addToStats(Instantiate(addse));  
                    isNew = false;
                }
                //if(add.statusEffectUpgrade != null) se.addToStats(Instantiate(add.statusEffectUpgrade));
            }
            //case 2.2. isnew: i don't have the status effect in my stats
            if(isNew && addse != null) { 
                StatusEffectData newse = Instantiate(addse);
                //if the new status previously wasn't in my stats (is new) 
                //  --> it didn't had an upgrade 
                //  --> so i need to add my dotupgrade i currently have in inventory
                if(statusEffectUpgrade != null) 
                    newse.addToStats(Instantiate(statusEffectUpgrade));
                statusEffects.Add(newse); 
            }
        }
        
        foreach(StatusEffectData se in statusEffects) {
            //if i already have statuses effect in my stats and i'm adding a new one
//            Debug.Log(add.statusEffectUpgrade);
            if(add.statusEffectUpgrade != null)  //if i already have dot in my stats and i add a dotupgrade
                se.addToStats(Instantiate(add.statusEffectUpgrade));
            
        }
    }

    public void SubToStats(WeaponStats sub) {
        base.SubToStats(sub);
        projectileSpeed -= sub.projectileSpeed;
        //GameManager.istance.player.inventory.AddWeaponSize(add.weaponSize*Vector3.one);
        projectilePierce -= sub.projectilePierce;
        projectileNumber -= sub.projectileNumber;
        projectileBounce -= sub.projectileBounce;        
        //doesn't sub status effects // need to implement it later
    }
    
    public override string GetAutoDescription() {
        string s = base.GetAutoDescription();
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (FieldInfo field in fields) {
            object value = field.GetValue(this);
            string name = field.Name;
            if(value is List<StatusEffectData> gs) 
                foreach(StatusEffectData g in gs) 
                    s += g.GetAutoDescription();
        } 
        return s;
    }

    public override string GetDescription(bool dontShowInfo = false) {
        string s = base.GetDescription();
        s += GetProjHomingDesc(true) 
        + GetProjSpdDesc(dontShowInfo)
        + GetProjPierceDesc(dontShowInfo)
        + GetProjNumDesc(dontShowInfo)
        + GetProjBounceDesc(dontShowInfo)
        ;
        
        s += GetStatusEffectsDescription();
        if(statusEffectUpgrade != null)
            s += statusEffectUpgrade.GetDescription(true);
        return s;
    }
    //all stats desc
    protected virtual string GetProjSpdDesc(bool dontShowInfo = false) {
        if(projectileSpeed == 0) return "";
        return "Projectile Speed " + IncreaseOrDecrease(projectileSpeed)
        + ShowDetailsCheck(GetSignedValue(projectileSpeed), dontShowInfo) + ".\n";
    }
    protected virtual string GetProjPierceDesc(bool dontShowInfo = false) {
        if(projectilePierce == 0) return "";
        return "Projectile Pierce " + IncreaseOrDecrease(projectilePierce)
        + ShowDetailsCheck(GetSignedValue(projectilePierce), dontShowInfo) + ".\n";
    }
    protected virtual string GetProjNumDesc(bool dontShowInfo = false) {
        if(projectileNumber == 0) return "";
        return "Projectile Number " + IncreaseOrDecrease(projectileNumber)
        + ShowDetailsCheck(GetSignedValue(projectileNumber), dontShowInfo) + ".\n";
    }
    protected virtual string GetProjBounceDesc(bool dontShowInfo = false) {
        if(projectileBounce == 0) return "";
        return "Projectile Bounce " + IncreaseOrDecrease(projectileBounce)
        + ShowDetailsCheck(GetSignedValue(projectileBounce), dontShowInfo) + ".\n";
    }
    protected virtual string GetProjHomingDesc(bool dontShowInfo = false) {
        if(!isHoming) return "";
        if(!dontShowInfo)
            return "Homing Effect Area Increased by " + Utility.GetPercentStrFromDecimalValue(projectileHoming) + "\n";
        return "Homing Effect Added\n";
    }
    public string GetStatusEffectsDescription() {
        string s = "";
        if(originalStatusEffects == null)
            return s;
        foreach(StatusEffectData se in originalStatusEffects) 
            s += se.GetDescription();
        return s;
    }
    public virtual string GetUpgradeDescription(bool dontShowInfo = false) {
        string s = base.GetDescription();
        s += GetProjHomingDesc(false) 
        + GetProjSpdDesc(dontShowInfo)
        + GetProjPierceDesc(dontShowInfo)
        + GetProjNumDesc(dontShowInfo)
        + GetProjBounceDesc(dontShowInfo);
        foreach(StatusEffectData se in originalStatusEffects) 
            s += se.GetUpgradeDescription();
        if(statusEffectUpgrade != null)
            s += statusEffectUpgrade.GetDescription(true);
        return s;
    }

    public string GetWeaponBaseStats(bool showApproxStats = true) {
        string s = "";
        string knockbackStr = ""; string wpnSizeStr = ""; string projSpdStr = "";
        string atkSpdStr = GetAtkSpdStr(showApproxStats); 
        /*  this needs to be done separately because it can be overridden by classes 
            like charged wpn that may have a different concept of attack speed */
        if(showApproxStats) {
            if(knockback == 3)
                knockbackStr = "Knockback: Normal\n";
            else if(knockback < 3)
                knockbackStr = "Knockback: Weak\n";
            else if(knockback > 3)
                knockbackStr = "Knockback: Strong\n";
        }
        else
            knockbackStr = "Knockback: " + this.knockback + "\n";

        if(showApproxStats) {
            if(weaponSize == 1)
                wpnSizeStr = "Projectile Size: Normal\n";
            else if(weaponSize < 1)
                wpnSizeStr = "Projectile Size: Small\n";
            else if(weaponSize > 1)
                wpnSizeStr = "Projectile Size: Big\n";
        }
        else
            wpnSizeStr = "Projectile Size: " + this.weaponSize + "\n";
        string dmgStr = "Base Damage: " + baseDamage + "\n";

        if(showApproxStats)
            if(projectileSpeed >= 7.5 && projectileSpeed <= 12.5)
                projSpdStr = "Projectile Speed: Normal\n";
            else if(projectileSpeed < 5)
                projSpdStr = "Projectile Speed: Very Slow\n";
            else if(projectileSpeed < 7.5)
                projSpdStr = "Projectile Speed: Slow\n";
            else if(projectileSpeed > 15)
                projSpdStr = "Projectile Speed: Very Fast\n";
            else if(projectileSpeed > 12.5)
                projSpdStr = "Projectile Speed: Fast\n";
        else
            projSpdStr = "Projectile Speed: " + projectileSpeed + "\n";
        //bool showAdditionalTraits = false;

        string pierceStr = ""; string projNumStr = "";
        if(projectilePierce != 0) {
            pierceStr = "Projectile Pierce +" + projectilePierce + "\n";
            //showAdditionalTraits = true;
        }
        if(projectileNumber != 1) {
            projNumStr = "Projectile Number +" + (projectileNumber-1) + "\n";
            //showAdditionalTraits = true;
        }
        string hStr = isHoming ? "Homing Effect Added\n" : "";
        s += dmgStr + knockbackStr + atkSpdStr + wpnSizeStr + projSpdStr + hStr;
        //if(showAdditionalTraits)
            s += 
            //"Additional Traits:\n" +
            projNumStr + pierceStr;
        return s;
    }

    protected virtual string GetAtkSpdStr(bool showApproxStats = true) {
        string atkSpdStr;
        if (showApproxStats) {
            if(atkCd < 0.75) //case 0.5
                atkSpdStr = "Attack Speed: Fast\n";
            else if(atkCd >= 0.75 && atkCd < 1) //case 0.75
                atkSpdStr = "Attack Speed: Normal\n";
            else //if(atkCd >= 1) //case 1
                atkSpdStr = "Attack Speed: Slow\n";
        }
        else
            atkSpdStr = "Attack Speed: " + atkCd + "\n";
        return atkSpdStr;
    }
    public WeaponStats InstantiateWeaponStats() {
        statusEffects = new List<StatusEffectData>();
        foreach(StatusEffectData se in originalStatusEffects) 
            statusEffects.Add(Instantiate(se));
        return Instantiate(this);
    }

    #region minor-utilities
    //DON'T USE THIS
    public WeaponStats GetStatsDiff (WeaponStats stats) {
        WeaponStats ws = CreateInstance<WeaponStats>().Copy(stats);
        ws.baseDamage = baseDamage - stats.baseDamage;
        ws.knockback = knockback - stats.knockback;
        ws.atkCd = atkCd + stats.atkCd;
        ws.weaponSize = weaponSize - stats.weaponSize;
        ws.projectileSpeed = projectileSpeed - stats.projectileSpeed;
        ws.projectilePierce = projectilePierce - stats.projectilePierce;
        ws.projectileNumber = projectileNumber - stats.projectileNumber;
        ws.projectileBounce = projectileBounce - stats.projectileBounce;
        /* TODO: fix this
        foreach(StatusEffectData addse in originalStatusEffects) 
            foreach(StatusEffectData se in stats.originalStatusEffects) 
                if(se.statusName == addse.statusName || addse.statusName == "dotupgrade") 
                    ws.statusEffects.Add(se.GetStatsDiff(addse));
        */
        return ws;
    }
    private WeaponStats Copy(WeaponStats s) {
        baseDamage = s.baseDamage;
        knockback = s.knockback;
        atkCd = s.atkCd;
        weaponSize = s.weaponSize;

        projectileSpeed = s.projectileSpeed;
        projectilePierce = s.projectilePierce;
        projectileNumber = s.projectileNumber;
        projectileBounce = s.projectileBounce;
        projectileHoming = s.projectileHoming;
        return s;
    }
    public WeaponStats() {
        baseDamage = 0;
        knockback = 0;
        atkCd = 0f;
        weaponSize = 0;

        projectileSpeed = 0;
        projectilePierce = 0;
        projectileNumber = 0;
        projectileBounce = 0;
        projectileHoming = 0;
    }
    public void SetWeaponStats(BaseWeaponStats s) {
        baseDamage = s.baseDamage;
        knockback = s.knockback;
        atkCd = s.atkCd;
        weaponSize = s.weaponSize;
        projectileSpeed = 0;
        projectilePierce = 0;
        projectileNumber = 0;
        projectileBounce = 0;
        statusEffects = null;
        projectileHoming = 0;
    }
    public void SetWeaponStats(int baseDamage, float knockback, float atkCd, float projectileSpeed, float weaponSize, int projectilePierce, int projectileNumber, int projectileBounce, List<StatusEffectData> statusEffects, float homing = 0) {
        this.baseDamage = baseDamage;
        this.knockback = knockback;
        this.atkCd = atkCd;
        this.projectileSpeed = projectileSpeed;
        this.weaponSize = weaponSize;
        this.projectilePierce = projectilePierce;
        this.projectileNumber = projectileNumber;
        this.projectileBounce = projectileBounce;
        this.statusEffects = statusEffects;
        this.projectileHoming = homing;
    }
    public WeaponStats CreateWeaponStats(int baseDamage, float knockback, float atkCd, float projectileSpeed, float weaponSize, int projectilePierce, int projectileNumber, int projectileBounce, List<StatusEffectData> statusEffects, float homing = 0) {
        this.baseDamage = baseDamage;
        this.knockback = knockback;
        this.atkCd = atkCd;
        this.projectileSpeed = projectileSpeed;
        this.weaponSize = weaponSize;
        this.projectilePierce = projectilePierce;
        this.projectileNumber = projectileNumber;
        this.projectileBounce = projectileBounce;
        this.statusEffects = statusEffects;
        this.projectileHoming = homing;
        return this;
    }
    #endregion
}
