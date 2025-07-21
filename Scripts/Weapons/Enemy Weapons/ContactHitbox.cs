using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ContactHitbox : HitSource
{
    [Header("Enemy Hitbox")]
    private ContactWeapon weapon;
    private Collider2D coll;
    private void Start() {
        weapon = GetComponentInParent<ContactWeapon>();
        weaponStats = weapon.stats;
        wielder = weapon.wielder;
        //Debug.Log("WeaponStats: " + weaponStats);
    }
    private void OnEnable() {
        coll = GetComponent<Collider2D>();
    }
    private float lastHit = 0;
    private void OnTriggerStay2D(Collider2D coll) {
//        Debug.Log(coll.gameObject.name);
        HandleCollision(coll);
    }

    public override void HandleCollision(Collider2D coll) {
        if(coll == null) { Debug.LogWarning("coll [" + coll + "] is null"); return; }
        if(coll.TryGetComponent(out IHittable hitObj)) {
            if(coll.TryGetComponent(out Player p) || !wielder.isAttackBlocked) 
                OnHit(hitObj, coll);
        }
    }

    protected override void OnHit(IHittable hitObj, Collider2D coll) {
        Fighter hitFighter = coll.GetComponent<Fighter>();
        if(Time.time - lastHit < weaponStats.atkCd) 
            return;
        lastHit = Time.time;
//        Debug.Log("Wielder: " + wielder + " " + this);
        Damage dmg = GameManager.instance.DamageCalculation(wielder,  weapon.wielder.stats, wielder.transform.position, 
            hitFighter, weaponStats, coll.ClosestPoint(transform.position),
            DamageType.normal,false,weaponStats.statusEffects, this);
        
        hitObj.getHit(dmg, 0);
    }

    public void SetCollider(Collider2D sourceCollider) {
        if (sourceCollider == null) {
            Debug.LogWarning("sourceCollider is null");
            return;
        }

//        Debug.Log(coll + " - " + sourceCollider);
        // Copy the properties of the source collider to the current collider
        coll.offset = sourceCollider.offset;

        if (coll is BoxCollider2D boxCollider && sourceCollider is BoxCollider2D sourceBoxCollider) 
            boxCollider.size = sourceBoxCollider.size;
        else if (coll is CircleCollider2D circleCollider && sourceCollider is CircleCollider2D sourceCircleCollider) 
            circleCollider.radius = sourceCircleCollider.radius;
        else if (coll is PolygonCollider2D polygonCollider && sourceCollider is PolygonCollider2D sourcePolygonCollider) 
            polygonCollider.points = sourcePolygonCollider.points;
        else if (coll is CapsuleCollider2D capsuleCollider && sourceCollider is CapsuleCollider2D sourceCapsuleCollider) {
            capsuleCollider.size = sourceCapsuleCollider.size;
            capsuleCollider.direction = sourceCapsuleCollider.direction;
        } else 
            Debug.LogWarning("Collider type not supported for copying");
        
    }
}
