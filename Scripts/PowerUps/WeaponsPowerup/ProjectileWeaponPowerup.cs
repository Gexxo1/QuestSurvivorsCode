using UnityEngine;
public abstract class ProjectileWeaponPowerup : WeaponPowerup
{
    //[Header("Projectile Weapon Based Powerup")]
    protected new WeaponStats stats => (WeaponStats)base.stats;
    protected new WeaponStats statsUpgrade => (WeaponStats)base.statsUpgrade;
    public new WeaponStats GetWeaponStats() { return stats; }
    [SerializeField] private Bullet projectile;
    protected float lastShot = 0;
    [SerializeField] protected GameObject endpoint;    
    protected virtual void Update() { lastShot += Time.deltaTime; }
    protected bool CanShoot() { return lastShot >= stats.atkCd; }
    protected void Shoot(Vector3 dir) {
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        ObjectPoolManager.SpawnBullet(projectile, endpoint.transform.position, rotation, stats, true, ObjectPoolManager.PoolType.PlayerBullet);
        lastShot = 0;
    }
    
    protected override void AddToStats() {
        if(stats != null) 
            stats.AddToStats(statsUpgrade);
        else
            Debug.LogWarning("Stats are null, add to stats failed");
    }

    public override void AddToStats(WeaponStats statToAdd) {
        if(stats != null && statToAdd != null) 
            stats.AddToStats(statToAdd);
        else
            Debug.LogWarning("Stats or parameters are null, add to stats failed");
    }

    protected abstract void Aim(Enemy target);
}
