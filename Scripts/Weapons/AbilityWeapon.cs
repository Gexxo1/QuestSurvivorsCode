using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWeapon : ProjectileWeapon {
    public override void InstantiateBullet(Transform endpoint, bool hasManaGain = false, ObjectPoolManager.PoolType poolType = ObjectPoolManager.PoolType.PlayerBullet, WeaponStats newStats = null, Bullet newBullet = null) {
        base.InstantiateBullet(endpoint, false, poolType, newStats, newBullet);
    }
}
