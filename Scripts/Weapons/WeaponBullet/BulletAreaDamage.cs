using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAreaDamage : Bullet
{
    [SerializeField] private bool hitOnlyOnce = false;
    //[SerializeField] private bool triggerOnHitEnemySpawn = false; //for kingslime
    protected override void OnEnable() {
        shotPos = transform.position;  
        deactivateBulletAfterTimeCoroutine = StartCoroutine(DeactivateBulletAfterTime());
        //OnDisable();
        //transform.localScale = baseScale;
        transform.localScale = modScale;
    }

    public override void HandleCollision(Collider2D coll) {
        if(coll == null) return;
        if(coll.TryGetComponent(out IHittable hitObj)) 
            OnHit(hitObj, coll);
    } 
    protected override void OnHit(IHittable hitObj, Collider2D coll) {
        Damage dmg = GameManager.instance.DamageCalculation(wielder, wielder.stats, shotPos, 
            coll.GetComponent<Fighter>(), weaponStats, coll.ClosestPoint(transform.position), 
            DamageType.areaDamage, false, weaponStats.statusEffects, this);
        //GameManager.instance.ShowText("x", coll.transform.position);
        float atkCd = hitOnlyOnce ? 0 : weaponStats.atkCd;
        hitObj.getHit(dmg, atkCd);
    }

    protected override void OnDisable() {
        if(disabled) return;
        disabled = true;
        
        if(animator != null) {
            if(!hitOnlyOnce)
                animator.SetTrigger("explode");
            else
                animator.SetTrigger("explode2");
        }
        else
            ReleaseFromPool();
    }

}
