using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class OnBulletDestroyPowerup : WeaponPowerup {
    protected new WeaponStats stats => (WeaponStats)base.stats;
    protected new WeaponStats statsUpgrade => (WeaponStats)base.statsUpgrade;
    protected override void Awake() {
        base.Awake();
        PowerupManager.OnBulletDestroyed += OnBulletDestroy;
    }
    void OnDestroy() {
        PowerupManager.OnBulletDestroyed -= OnBulletDestroy;
    }
    public abstract void OnBulletDestroy(HitSource hitsource);
    protected override void AddGlobalWpnStatsToStats() {
//        Debug.Log(stats.projectileHoming + " + " + GameManager.instance.player.globalWpnStats.projectileHoming);
        stats.AddToStats(GameManager.instance.player.globalWpnStats);
    }
    /*
    protected override void InstantiateStatsIfNull() {
        if(originalStats != null && stats == null) {
            stats = Instantiate(originalStats);
            stats.AddToStats(GameManager.instance.player.globalWpnStats);
        }
    }
    */
}
