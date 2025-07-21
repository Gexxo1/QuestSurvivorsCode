using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyProjectileWeapon : ProjectileWeapon 
{
    [Header("Enemy Projectile Weapon")]
    [SerializeField] private EnemyRange enemyRange;
    public override bool CanShoot() { 
        return InEnemyRange() && AttackCooldown(); 
    }
    protected override void Update() {
        //intentionally blank to overwrite the base class
    }

    public bool InEnemyRange() { return enemyRange.isInRange; }
}
