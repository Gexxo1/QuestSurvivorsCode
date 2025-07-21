using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
/*
//Used by wavemanger
public class EnemySpawner : MonoBehaviour
{
    public ObjectPool<Enemy> enemyPool;
    public ObjectPool<Bullet> bulletPool;
    int currentEnemyIndex = 0;
    GameObject enemyParentPool;
    GameObject bulletParentPool;
    //reference to enemies
    List<Enemy> enemyTypeList;
    EnemyProjectile rangedEnemy;
    //reference to the "EnemyProjectile"'s projectile weapon
    private ProjectileWeapon wpn;
    private Transform wpnEndpoint;
    private Bullet wpnBullet;
    private EnemyProjectile enemyAim;
    private void Start() {
        enemyTypeList = WaveManager.instance.GetEnemiesList();
        rangedEnemy = WaveManager.instance.GetRangedEnemy();
        if(rangedEnemy != null) {
            wpn = rangedEnemy.GetProjectileWeapon();
            if(wpn != null) {
                wpnEndpoint = wpn.GetEndpoint();
                wpnBullet = wpn.GetBulletPrefab();
                if(wpn.wielder is EnemyProjectile e) enemyAim = e;  
                else Debug.LogWarning("BulletSpawner: no Player or Enemy found");
            }
            else Debug.Log("BulletSpawner: no ProjectileWeapon found");
        }
        else Debug.LogWarning("BulletSpawner: no ranged enemy found");
        enemyParentPool = new GameObject("EnemyPool");
        bulletParentPool = new GameObject("BulletPool");
        
        bulletPool = new ObjectPool<Bullet>
            (CreateBullet, OnTakeBulletFromPool, OnReturnBulletToPool, OnDestroyBullet, true, 100, 10000);
        //enemyPool = new ObjectPool<Enemy> (CreateBullet, OnTakeBulletFromPool, OnReturnBulletToPool, OnDestroyBullet, true, 100, 10000);
    }
    //bullet section
    int bi = 0;
    private Bullet CreateBullet() {
        Bullet bullet = Instantiate(wpnBullet, wpnEndpoint.position, Quaternion.identity);
        bullet.name = "Bullet_" + bi++;
        bullet.SetPool(bulletPool);
        bullet.gameObject.SetActive(false);
        return bullet;
    }
    private void OnTakeBulletFromPool(Bullet bullet) {
        Vector3 direction;
        if(enemyAim != null)
            direction = enemyAim.GetAimDirection();
        else { Debug.LogWarning("BulletSpawner: Enemy not found"); return; }

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

    //enemy section
    int ei = 0;
    private Enemy CreateEnemy() {
        Enemy enemy = Instantiate(enemyTypeList[currentEnemyIndex], wpnEndpoint.position, Quaternion.identity);
        enemy.name = "Enemy_" + ei++;
        //bullet.SetPool(bulletPool);
        enemy.gameObject.SetActive(false);
        return enemy;
    }
    private void OnTakeEnemyFromPool(Enemy enemy) {
        //Enemy enemy = FindEnemy(targetEnemy);
        enemy.gameObject.SetActive(true);
    }
    private Enemy FindEnemy(Enemy targetEnemy) {
        Enemy enemy = enemyTypeList.Find(e => e == targetEnemy);
        return enemy;
    }
    private void OnReturnEnemyToPool(Bullet bullet) {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyEnemy(Enemy bullet) {
        Destroy(bullet.gameObject);
    }
}
*/