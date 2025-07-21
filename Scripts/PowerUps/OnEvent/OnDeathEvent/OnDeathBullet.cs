using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathBullet : OnDeathPowerup
{ 
    [SerializeField] private BulletAreaDamage prefab;
    protected override void Awake() {
        base.Awake();
    }
    public override void OnDeath(Fighter fighter) {
        //Instantiate(prefab, pos, Quaternion.identity).Setup(stats,this);
        ObjectPoolManager.SpawnBullet(prefab, fighter.transform.position, Quaternion.identity, stats, true, ObjectPoolManager.PoolType.PlayerBullet);
    }

}
