using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : Enemy {
    [Header("Kamikaze Enemy")]
    [SerializeField] private float timeNeededToExplode = 1f;
    [SerializeField] private BulletAreaDamage explosionPrefab;
    [SerializeField] private BaseWeaponStats explosionStats;
    [SerializeField] private EnemyRange rangeTrigger;
    //[SerializeField] private EnemyColliderTrigger pTrigger;
    private float timeLeftToExplode;
    private bool isCountingDown = false;
    protected override void OnEnable() {
        base.OnEnable();
        ResetCountdown();
    }
    protected override void Update() {
        base.Update();
        //Debug.Log(collidingWithPlayer);
        if(rangeTrigger.isInRange) {
            timeLeftToExplode -= Time.deltaTime;
            if(timeLeftToExplode <= 0) 
                Explode();
        }
        else if(timeLeftToExplode < timeNeededToExplode) {
            timeLeftToExplode += Time.deltaTime;
        }
        UpdateSpriteColor();
    }
    public void BeginOrStopCountdown(bool flag) { //triggered by collider ontriggerenter or ontriggerexit
        isCountingDown = flag;
    }
    public void ResetCountdown() {
        sprite.color = Color.white;
        rangeTrigger.isInRange = false;
        timeLeftToExplode = timeNeededToExplode;
    }
    private void Explode() {
        ObjectPoolManager.SpawnBullet(explosionPrefab, transform.position, Quaternion.identity, 
        explosionStats, false, this, ObjectPoolManager.PoolType.EnemyBullet);
        bossBehavior?.OnHit(currHP, stats.healthpoint, currHP);
        bossBehavior?.OnDeath();
        DestroyEnemy(0); //if enemy succesfully explodes, then it shouldn't be counted as a kill
    }
    private void UpdateSpriteColor() {
        float t = 1 - (timeLeftToExplode / timeNeededToExplode); // Calcola la proporzione del tempo trascorso
        sprite.color = Color.Lerp(Color.white, Color.red, t); // Interpola tra bianco e rosso
    }

    //intentionally blank because the color changed in updatespritecolor() conflicts with other color changes caused by statuses
    public override void ChangeSpriteColor(Color32 color) { }
    public override void RevertSpriteColor() { }

    //coroutine shiet, unused
    /*
    protected Coroutine explosionCoroutine;
    public void BeginOrStopCountdown(bool flag) {
        if(flag) {
            Debug.Log("Starting explosion countdown: ");
            if(explosionCoroutine != null) {
                Debug.Log("Coroutine already running, stopping it");
                StopCoroutine(explosionCoroutine);
                explosionCoroutine = null;
            } else Debug.Log("Starting new routine");
            explosionCoroutine = StartCoroutine(ExplosionCountdown());
        }
        else {
            //modo 1
            //StopCoroutine(ExplosionCountdown());
            //explosionCoroutine = null;

            //modo 2
            if(explosionCoroutine != null) {
                StopCoroutine(explosionCoroutine);
                explosionCoroutine = null;
            }
            else
                Debug.LogWarning("ExplosionCoroutine is null: trying to stop a coroutine that is not running");
        }
    }
    private IEnumerator ExplosionCountdown() {
        yield return new WaitForSeconds(timeToExplode);
        Explode();
    }
    */
}   
