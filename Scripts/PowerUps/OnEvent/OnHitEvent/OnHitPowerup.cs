using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnHitPowerup : WeaponPowerup
{
    protected override void Awake() {
        base.Awake();
        PowerupManager.OnEnemyHit += OnHit;
    }
    void OnDestroy() {
        PowerupManager.OnEnemyHit -= OnHit;
    }
    public abstract void OnHit(Fighter fighter, HitSource sourceBullet);
}
