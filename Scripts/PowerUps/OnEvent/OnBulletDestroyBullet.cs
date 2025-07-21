using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//on bullet destroy summon another bullet
public class OnBulletDestroyBullet : OnBulletDestroyPowerup {
    [Header("OnBulletDestroyBullet")]
    [SerializeField] private Bullet prefab;
    [SerializeField] private int[] numberOfProjectiles = {1, 1, 1, 1, 1};
    [SerializeField] private bool hasManaGain = false;
    public override void OnBulletDestroy(HitSource hitsource) {
        Quaternion rotation = Quaternion.identity;
        if(prefab is not BulletAreaDamage) 
            rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        //Debug.Log("OnBulletDestroyBullet --> " + rotation);
        ObjectPoolManager.SpawnBullet(prefab, hitsource.transform.position, rotation, stats, hasManaGain, ObjectPoolManager.PoolType.PlayerBullet);
    }

    public override string GetUpgrDesc() {
        if(numberOfProjectiles[tier] != numberOfProjectiles[tier+1])
            return base.GetUpgrDesc() + "Now spawns " + numberOfProjectiles[tier+1] + " bullets";
        return base.GetUpgrDesc();
    }
}
