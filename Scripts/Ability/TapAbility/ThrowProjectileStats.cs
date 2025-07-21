using UnityEngine;

public class ThrowProjectileStats : ThrowProjectile
{
    [Header("Projectile Stats")]
    public WeaponStats increasedProjectileStats;
    protected override void InstantiateBullet() {
        WeaponStats stats = Instantiate(wpn.stats);
        stats.AddToStats(increasedProjectileStats);

        ObjectPoolManager.SpawnBullet
        (projectile, wpn.GetFirstEndpointPosition(), wpn.transform.parent.rotation, stats, false, ObjectPoolManager.PoolType.PlayerBullet);
    }
    /*
        ObjectPoolManager.SpawnBullet
        (projectile, wpn.GetFirstEndpointPosition(), wpn.transform.parent.rotation, stats, false, ObjectPoolManager.PoolType.PlayerBullet);
    */
    /*
    protected override float GetCooldown() {
        if(increasedProjectileStats.atkCd != 0) 
            return increasedProjectileStats.atkCd;
        return base.GetCooldown();
        
    }
    */
}
