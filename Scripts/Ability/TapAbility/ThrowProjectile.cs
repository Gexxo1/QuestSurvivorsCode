using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ThrowProjectile : TapHoldAbility
{
    public Bullet projectile;
    [Header("Damage Ability Stats")]
    protected ProjectileWeapon wpn;
    protected override void Awake() {
        base.Awake();
        wpn = GameObject.Find("Weapons").GetComponentInChildren<ProjectileWeapon>();
    }

    protected override void UseSkill() {
        base.UseSkill();
        DrainMana();
        InstantiateBullet();
    }

    protected virtual void InstantiateBullet() {
        ObjectPoolManager.SpawnBullet
        (projectile, wpn.GetFirstEndpointPosition(), wpn.transform.parent.rotation, wpn, false, ObjectPoolManager.PoolType.PlayerBullet);
    }
}
