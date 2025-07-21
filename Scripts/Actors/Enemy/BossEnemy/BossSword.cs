using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSword : BossEnemyRanged
{
    [Header("Boss Sword")]
    private EnemyMeleeWeapon meleeRef;
    [SerializeField] private List<GameObject> endps;
    protected override void Awake() {
        base.Awake();
        if(rangedWeapon.TryGetComponent(out EnemyMeleeWeapon mw))
            meleeRef = mw;
    }

    
    protected override void BossAbility() {
        base.BossAbility();
        meleeRef.SetLastShot();
        if(meleeRef.IsFirstSwing()) {
            meleeRef.anim.SetTrigger("Ability1");
            meleeRef.anim.ResetTrigger("Ability2");
        }
        else {
            meleeRef.anim.SetTrigger("Ability2");
            meleeRef.anim.ResetTrigger("Ability1");
        }
        for(int i=0; i < endps.Count; i++) 
            meleeRef.InstantiateBullet(endps[i].transform, false, ObjectPoolManager.PoolType.EnemyBullet);
    }
}
