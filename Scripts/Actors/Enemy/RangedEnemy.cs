using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Enemy")]
    [SerializeField] protected EnemyProjectileWeapon rangedWeapon;
    //public Vector3 shootRange;
    protected override void Aggro() {
        if(!rangedWeapon.InEnemyRange()) 
            Movement((playerTransform.position - transform.position).normalized,stats.moveSpeed);
        else 
            StopMoving();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if(rangedWeapon.CanShoot()) 
            Attack();
    }

    protected virtual void Attack() {
        rangedWeapon.Shoot();
    }
}
