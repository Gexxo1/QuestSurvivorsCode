using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : ProjectileWeapon
{
    //Attack 1 or 2 --> es.: spada va avanti o dietro
    [HideInInspector] bool isFirstSwing = true;
    protected override void Start() {
        base.Start();
        //useWeaponParentRotation = true;
    }
    public void invertSwing() {
        isFirstSwing  = !isFirstSwing;
    }
    public void SetSwing(bool isFirstSwing) {
        this.isFirstSwing = isFirstSwing;
    }
    public bool IsFirstSwing() { return isFirstSwing; }
    public override void Shoot() {
        SetLastShot();
        anim.SetTrigger(animTriggerName);
        InstantiateSequentialBullets();     
        
        //if(isFirstSwing) PlaySFX("Swing1",0.75f);  
        //else PlaySFX("Swing2",0.75f);   
        PowerupManager.instance.PlayerShootEvent(wielder);   
    }
    /*
    public override void InstantiateBullet(Transform endpoint, bool hasManaGain = false) {
        //ObjectPoolManager.SpawnObject(bulletPrefab.gameObject, endpoint.position, endpoint.rotation, ObjectPoolManager.PoolType.PlayerBullet);
        ObjectPoolManager.SpawnBullet(bulletPrefab, endpoint.position, endpoint.rotation, this, hasManaGain, ObjectPoolManager.PoolType.PlayerBullet);
    }
    */

    public override string GetWeaponType() {
        return "Melee";
    }

    public void StartSwingAnimation(string animName) {
        anim.SetTrigger(animName);
        //anim.Play(animName, -1, 1f);
    }
}
