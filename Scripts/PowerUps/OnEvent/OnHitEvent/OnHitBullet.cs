using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitBullet : OnHitPowerup
{ 
    [Header("OnHitBullet")]
    [SerializeField] private Bullet prefab;
    [Range(0,100)] [SerializeField] private int[] chance = new int[5];
    public override void OnHit(Fighter fighter, HitSource hitsource) { //WARNING, sourcebullet = 'the bullet that triggered the event' != 'the bullet that will be spawned'
        //check if the bullet that triggered the event is the same as the bullet that will be spawned
        if(hitsource == null) {
            Debug.LogWarning("HitSource is null, fighter [" + fighter + "]");
            return;
        }
        if(Random.Range(0,101) > chance[tier] || Utility.AreGameObjectsSameIgnoringClone(prefab.gameObject,hitsource.gameObject)) return; 
        Quaternion rotation = Quaternion.identity;
        if(prefab is not BulletAreaDamage) 
            rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        ObjectPoolManager.SpawnBullet(prefab, fighter.transform.position, rotation, stats, true, ObjectPoolManager.PoolType.PlayerBullet);
    }

    public override string GetDescription() {
        return base.GetDescription() + "\n" + "Base chance: " + chance[tier] + "%";
    }
    public override string GetUpgrDesc() {
        return "Chance is now: " + chance[tier+1] + "%\n" + base.GetUpgrDesc();
    }
}
