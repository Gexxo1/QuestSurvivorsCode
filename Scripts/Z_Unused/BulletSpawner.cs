using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
/*
[RequireComponent(typeof(ProjectileWeapon))]
//BulletSpawner usage: attach it to a projectile weapon
public class BulletSpawner : MonoBehaviour
{
    public ObjectPool<Bullet> bulletPool;
    //main weapon reference
    private ProjectileWeapon wpn;
    private Transform wpnEndpoint;
    private Bullet wpnBullet;
    //private GameObject parent;
    //used only IF player
    private PlayerAim playerAim;
    //used only IF enemy
    private EnemyProjectile enemyAim;
    private void Start() {
        if(TryGetComponent(out ProjectileWeapon pw)) {
            Debug.Log("Wielder " + pw.wielder);
            wpn = pw;
            wpnEndpoint = wpn.GetEndpoint();
            wpnBullet = wpn.GetBulletPrefab();
            //todo: merge player and enemy aim system into one (inherit from a common class?)
            if(wpn.wielder is Player p) playerAim = p.GetComponent<PlayerAim>();
            else if(wpn.wielder is EnemyProjectile e) enemyAim = e;  
            else Debug.LogWarning("BulletSpawner: no Player or Enemy found");
        }
        else Debug.Log("BulletSpawner: no ProjectileWeapon found");
        //parent = new GameObject(wpn.name + "_BulletPool");

        bulletPool = new ObjectPool<Bullet>
            (CreateBullet, OnTakeBulletFromPool, OnReturnBulletToPool, OnDestroyBullet, true, 100, 10000);
    }
    int i = 0;
    private Bullet CreateBullet() {
        Bullet bullet = Instantiate(wpnBullet, wpnEndpoint.position, Quaternion.identity);
        bullet.name = "Bullet_" + i++;
        bullet.SetPool(bulletPool);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void OnTakeBulletFromPool(Bullet bullet) {
        Vector3 direction;
        if(playerAim != null)
            direction = playerAim.GetAimDirection();
        else if(enemyAim != null)
            direction = enemyAim.GetAimDirection();
        else { Debug.LogWarning("BulletSpawner: no Player or Enemy found"); return; }

        Debug.Log("BulletSpawner: " + direction);
        //bullet.Setup(wpn.stats, wpn.GetEndpoint().position, direction, wpn.GetWielder(), wpn, wpn.GetWielder() is Player, wpnBullet.transform.rotation.z);
        //bullet.transform.SetPositionAndRotation(wpnEndpoint.position, Quaternion.identity);
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnBulletToPool(Bullet bullet) {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet) {
        Destroy(bullet.gameObject);
    }
}
*/