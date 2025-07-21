using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRay : HitSource
{
    //Setup parameters
    //private BaseWeaponStats weaponStats;
    private Transform endpoint;
    public override void HandleCollision(Collider2D coll) {
        if(coll.TryGetComponent(out IHittable hitObj)) 
            OnHit(hitObj, coll);
    }

    protected override void OnHit(IHittable hitObj, Collider2D coll) {
        Fighter target = coll.GetComponent<Fighter>();
        Player p = GameManager.instance.player;
        //if(coll.TryGetComponent(out IEffectable e)) e.ApplyMultipleStatus(p.globalWpnStats.statusEffects);
        Vector2 hitpoint = coll.ClosestPoint(transform.position);
        Damage dmg = GameManager.instance.DamageCalculation(wielder, wielder.stats, endpoint.position, target, 
            weaponStats, hitpoint, DamageType.normal, default, p.globalWpnStats.statusEffects, this);
        hitObj.getHit(dmg, weaponStats.atkCd);
    }
    private void OnTriggerStay2D(Collider2D coll) {
        HandleCollision(coll);
    }
    /*
    public DamageRay Setup(Fighter whoIsShooting, int damage, float knockback, float cd, Item item) {
        wielder = whoIsShooting;
        this.damage = damage;
        this.knockback = knockback;
        hitCd = cd;
        sourceItem = item;
        return this;
    }
    */
    public DamageRay Setup(Fighter whoIsShooting, WeaponStats baseWeaponStats) {
        wielder = whoIsShooting;
        weaponStats = Instantiate(baseWeaponStats);
        endpoint = transform.parent;
        return this;
    }

    public void getDestroyed() {
        Destroy(gameObject);
    }

    public void getDestroyed(float sec) {
        Destroy(gameObject, sec);
    }
}
