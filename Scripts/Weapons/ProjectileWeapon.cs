using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ProjectileWeapon : Weapon
{
    [Header("Projectile Weapon")]
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] protected Transform[] gunEndPoints;
    //[HideInInspector] public Vector3 extraProjScale;
    [SerializeField] private float multipleProjectileDelay = 0.05f;
    [SerializeField] private string sfxName = "none";
    [SerializeField] protected Vector3 baseRotation;
    protected float lastShoot;
    protected bool useWeaponParentRotation = false; //this should be set true to weapons that have a different rotation from the parent (e.g. bow, staff)
    protected override void Awake() {
        base.Awake();
        if(baseRotation != Vector3.zero)
            useWeaponParentRotation = true;
//        Debug.Log(useWeaponParentRotation + " " + baseRotation);
    }
    protected override void Start() {
        base.Start();
        //transform.localScale = stats.weaponSize*Vector3.one; removed 05/08/2024 because the base size should remain untouched (should set only in prefab inspector)
        AdjustAnimatorAtkSpd();
    }
    protected virtual void Update() {
        if(CanShoot()) 
            Shoot();
    }
    public virtual bool CanShoot() { 
        return PlayerInput() && AttackCooldown(); 
    }
    protected virtual bool PlayerInput() {
        //return (Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0)) && !Input.GetKey(KeyCode.Space);
        return Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0);
    }
    protected bool AttackCooldown() {
        //Debug.Log("Time.time - lastShoot: [" + (Time.time - lastShoot));
        //Debug.Log("] stats.atkCd: [" + stats.atkCd);
        //Debug.Log("] wielder.isAttackBlocked: [" + wielder.isAttackBlocked + "]");
        bool isAtkBlock = false;
        if(wielder != null) isAtkBlock = wielder.isAttackBlocked;
        return Time.time - lastShoot > stats.atkCd && !isAtkBlock;
    }
    public virtual void Shoot() {
        SetLastShot();
        PlaySFX(sfxName);
        PlayAnimation();
        //InstantiateBullets();   
        InstantiateSequentialBullets();      
        PowerupManager.instance.PlayerShootEvent(wielder);       
    }
    protected void PlayAnimation() {
        if(animTriggerName != "none")
            anim.SetTrigger(animTriggerName);
    }
    protected virtual void PlaySFX(string name, float volume = 1f) {
        if(name != "none")
            AudioManager.instance.PlayPooledSFX(name, transform, volume);
    }
    public void SetLastShot() { 
        lastShoot = Time.time; 
        if(wielder is Player && stats.atkCd > 0.1f)
            MenuManager.instance.ShowWeaponIndicatorFill(stats.atkCd);
    }
    public float GetLastShot() { return lastShoot; }
    public virtual void InstantiateBullets(bool hasManaGain = false, WeaponStats newStats = null, Bullet newBullet = null) {
        bool mg = hasManaGain && wielder is Player;
        for(int i=0; i < gunEndPoints.Length && i < GetProjectileNumber(newStats) + additionalBullets; i++) 
            InstantiateBullet(gunEndPoints[i], mg, GetPoolType(), newStats, newBullet);
    }
    protected void InstantiateSequentialBullets(WeaponStats newStats = null, Bullet newBullet = null) {
        //Debug.Log("sequential bullets");
        StartCoroutine(InstantiateSequentialBullets(GetProjectileNumber(newStats), newStats, newBullet));
    }
    public IEnumerator InstantiateSequentialBullets(int numberOfBullets, WeaponStats newStats = null, Bullet newBullet = null) {
        for(int i=0; i < numberOfBullets + additionalBullets; i++) {
            InstantiateBullet(gunEndPoints[0], wielder is Player, GetPoolType(), newStats, newBullet);
            yield return new WaitForSeconds(multipleProjectileDelay);
        }
        SetAdditionalBullets(0);
        yield return null;
    }
    protected int additionalBullets = 0;
    public void SetAdditionalBullets(int n) { additionalBullets = n; }
    private ObjectPoolManager.PoolType GetPoolType() {
        return wielder is Player ? ObjectPoolManager.PoolType.PlayerBullet : ObjectPoolManager.PoolType.EnemyBullet;
    }
    //Without Pooling
    /*
    public void InstantiateBullet(Vector3 dir, Transform endpoint, bool hasManaGain = false, int rotModifier = 0) {
        Bullet b = Instantiate(bulletPrefab,endpoint.position,endpoint.rotation);
        b.gameObject.SetActive(false);
        b.Setup(stats, endpoint.transform.position, dir, wielder, this, hasManaGain, rotModifier);
        b.gameObject.SetActive(true);
    }
    */
    //With Pooling
    public virtual void InstantiateBullet(Transform endpoint, bool hasManaGain = false, ObjectPoolManager.PoolType poolType = ObjectPoolManager.PoolType.PlayerBullet, 
        WeaponStats newStats = null, Bullet newBullet = null) {

        if (endpoint == null) {
            Debug.LogError("gunEndPoint is null");
            return;
        }
        if(bulletPrefab == null && newBullet == null) {
            Debug.LogError("bulletPrefab & bullet parameter are null");
            return;
        }
        Bullet bulletToInstantiate = newBullet == null ? bulletPrefab : newBullet;
        //Debug.Log("Setting Auto Follow to: " + isAutoFollow);
        //bulletToInstantiate.SetAutoFollow(isAutoFollow);
        Quaternion rotation = useWeaponParentRotation ? transform.parent.rotation : endpoint.rotation;
        if(manaGainDisabled) hasManaGain = false;
        if(newStats != null) 
            ObjectPoolManager.SpawnBullet(bulletToInstantiate, endpoint.position, rotation, newStats, hasManaGain, ObjectPoolManager.PoolType.PlayerBullet);
        else
            ObjectPoolManager.SpawnBullet(bulletToInstantiate, endpoint.position, rotation, this, hasManaGain, poolType);
    }
    protected int GetProjectileNumber(WeaponStats newStats = null) { 
        return newStats == null ? stats.projectileNumber : newStats.projectileNumber; 
    }

    public void setWeaponSkin(Sprite skin, Bullet bull) {
        gameObject.GetComponent<SpriteRenderer>().sprite = skin;
        bulletPrefab = bull;
    }

    public Fighter GetWielder() { return wielder; }

    public Transform GetFirstEndpoint() {
        return gunEndPoints[0];
    }

    public Vector3 GetFirstEndpointPosition() {
        return gunEndPoints[0].position;
    }

    public virtual void UpdateWeaponStats(WeaponStats add, bool addStats = true) {
        if(addStats) 
            stats.AddToStats(add);
        else 
            stats.SubToStats(add);
        
        AdjustAnimatorAtkSpd();    
    }
    public virtual void UpdateWeaponStats(BaseWeaponStats add, bool addStats = true) {
        if(addStats) 
            stats.AddToStats(add);
        else 
            stats.SubToStats(add);
        
        AdjustAnimatorAtkSpd();
    }
    protected virtual void AdjustAnimatorAtkSpd() {
        if (anim != null && stats != null) {
            float speedMultiplier = stats.AttackSpeed / 2;
            speedMultiplier = Mathf.Clamp(speedMultiplier, 1f, 2f);
//            Debug.Log("Animator AtkSpdMul: " + speedMultiplier);
            anim.SetFloat("AtkSpdMul", speedMultiplier);
        }
    }

    public override string GetWeaponBaseStatsDetails(bool showApproxStats = true) {
        return "Type: " + GetWeaponType() + "\n" + base.GetWeaponBaseStatsDetails(showApproxStats);
    }
    
    public override string GetWeaponType() {
        return "Ranged";
    }
    public Bullet GetBulletPrefab() {
        return bulletPrefab;
    }
    public Sprite GetWeaponProjectileSprite() {
        return bulletPrefab.GetSprite();
    }
    /*
    public void SetAutoFollowProjectile(bool af) {
        isAutoFollow = af;
    }
    public bool IsAutoFollowProjectile() {
        return isAutoFollow;
    }
    */
}
