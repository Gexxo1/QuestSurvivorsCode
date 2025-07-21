using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightningStaffBoss : BossEnemyRanged {
    [Header("Lightning Staff Enemy")]
    private ProjectileWeapon staffRef;

    [SerializeField] private GameObject warningAreaPrefab;
    [SerializeField] private BulletAreaDamage damageAreaPrefab;
    [SerializeField] private float warningDuration = 2.0f;
    protected override void Awake() {
        base.Awake();
        if(rangedWeapon.TryGetComponent(out ProjectileWeapon s))
            staffRef = s;
    }
    protected override void BossAbility() {
        base.BossAbility();
        staffRef.SetLastShot();
        
        SpawnWarningAndDamageArea();
    }

    private void SpawnWarningAndDamageArea() {
        StartCoroutine(WarningAndDamageCoroutine());
    }
    GameObject warningArea;
    private IEnumerator WarningAndDamageCoroutine() {
        // Spawna l'area di avviso
        Vector2 warningPos = GameManager.instance.player.transform.position;
        warningArea = Instantiate(warningAreaPrefab, warningPos, Quaternion.identity); //not pooled
        //if (damageAreaPrefab.TryGetComponent(out CircleCollider2D damageCollider)) {
            // Imposta la scala dell'area di danno in base al raggio del collider
            //float radius = damageCollider.radius;
        warningArea.transform.localScale += Vector3.one - damageAreaPrefab.baseScale;
        //} 
        //else Debug.LogWarning("LightningStaffBoss: damageAreaPrefab does not have a CircleCollider2D component");

        // Attende per la durata dell'avviso
        yield return new WaitForSeconds(warningDuration);
        // Distrugge l'area di avviso
        Destroy(warningArea);
        warningArea = null;
        // Spawna l'area di danno
        //GameObject damageArea = 
        
        //Instantiate(damageAreaPrefab, warningPos, Quaternion.identity);
        ObjectPoolManager.SpawnBullet(damageAreaPrefab, warningPos, Quaternion.identity, staffRef, false, ObjectPoolManager.PoolType.EnemyBullet);
    }

    private void OnDisable() {
        if(warningArea != null) {
            Destroy(warningArea);
            warningArea = null;
        }
    }
}
