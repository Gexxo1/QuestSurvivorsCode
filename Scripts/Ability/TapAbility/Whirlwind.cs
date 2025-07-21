using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : AnimatorAbility
{
    [Header("Whirlwind")]
    private MeleeWeapon wpn;
    [SerializeField] private int projNumber = 8;
    [SerializeField] private List<GameObject> endpoints;
    [SerializeField] private StatsModifier statMod;
    protected override void Start()
    {
        base.Start();
        wpn = player.inventory.getCurrWeapon().GetComponent<MeleeWeapon>();
    }
    protected override void UseSkill()
    {
        base.UseSkill();
        DrainMana();
        if(wpn.IsFirstSwing()) {
            animator.SetTrigger("Ability1");
            animator.ResetTrigger("Ability2");
        }
        else {
            animator.SetTrigger("Ability2");
            animator.ResetTrigger("Ability1");
        }
        InstantiateBullets();
    }

    private void InstantiateBullets() {
        for(int i=0; i < projNumber && i < endpoints.Count; i++) 
            wpn.InstantiateBullet(endpoints[i].transform, statMod.hasManaGain, ObjectPoolManager.PoolType.PlayerBullet, GetFixedAttributes());
    }

    protected WeaponStats GetFixedAttributes() {
        WeaponStats weaponStats = ScriptableObject.CreateInstance<WeaponStats>();
        int dmg, pierce; float knock, bullspd;
        //dmg = (int)(wpn.stats.baseDamage * fixedModfier[shootStrength]);
        dmg = (int)(wpn.stats.baseDamage * statMod.dmgMul);
        knock = wpn.stats.knockback * statMod.knockbackMul;
        bullspd = wpn.stats.projectileSpeed * statMod.spdMul;
        //pierce = wpn.stats.projectilePierce + shootStrength + 1;
        pierce = wpn.stats.projectilePierce * statMod.pierceMul + statMod.pierceAdd;


        weaponStats.SetWeaponStats(dmg, knock, wpn.stats.atkCd, bullspd, 
            wpn.stats.weaponSize, pierce, statMod.knockbackMul, 
            wpn.stats.projectileBounce, wpn.stats.statusEffects, wpn.stats.projectileHoming);
        return weaponStats;
    }
}

[Serializable]
public class StatsModifier {
    public float dmgMul = 1f;
    public int knockbackMul = 1;
    public int pierceMul = 1;
    public int pierceAdd = 0;
    public float spdMul = 1f;
    public bool hasManaGain = false;
}
