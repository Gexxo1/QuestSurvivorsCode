using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.Pool;
using System;

public class BulletNoDmg : Bullet {
    public override void HandleCollision(Collider2D coll) {
        if(coll == null) { Debug.Log("coll [" + coll + "] is null"); return; }

        if(coll.gameObject.TryGetComponent(out ICollidable _)) 
            OnDisable();
    }
    protected override void OnHit(IHittable hitObj, Collider2D coll) { } //intentionally blank
    protected override void OnDisable() {
        if(disabled) return;
        disabled = true;
        
        if(rb != null) rb.velocity = Vector2.zero;
        ReleaseFromPool();
        //warning: instantiating on disable NEEDS to be done before checking if the parent is active (happens if scene is changed)
        if(additionalBullet != null && transform != null && transform.parent.gameObject.activeInHierarchy) 
            ObjectPoolManager.SpawnBullet(additionalBullet, transform.position, Quaternion.identity, 
                weaponStats, false, ObjectPoolManager.PoolType.PlayerBullet);
    }

    protected override void UpdateVelocity() {
        direction = transform.right;
        rb.velocity = weaponStats.projectileSpeed * direction;
    }
}
