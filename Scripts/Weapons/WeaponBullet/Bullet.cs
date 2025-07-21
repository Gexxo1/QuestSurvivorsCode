using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : HitSource
{
    [Header("Main Bullet Attributes")]
    [SerializeField] protected float timeToDestroy = 60f;
    [SerializeField] protected bool affectedByOnBulletDestroy = true;
    [Header("Scale / Rotation")]
    [SerializeField] public Vector3 baseScale = Vector3.one;
    [SerializeField] protected bool unaffectedByScale = false;
    [SerializeField] protected Vector3 baseRotation;
    [SerializeField] protected bool unaffectedByRotation = false;
    [Header("Important Refs - MUST be set")]
    [SerializeField] GameObject spriteHitbox;
    //[Header("Main Refs - MUST be set")]
    //[SerializeField] protected GameObject bulletCollider;
    [Header("Additional Refs - Can be null if not needed")]
    [SerializeField] protected Bullet additionalBullet; //can be null
    [SerializeField] GameObject homingAura; //to do: serialize every projectile homing aura in prefab, instead of transform.find
    [SerializeField] HomingMechanic homingMechanic;
    [SerializeField] private bool affectedByPooling = true;
    //internal attributes
    protected Vector2 direction;
    protected bool isDestroyed = false;
    protected Item sourceItem;
    private bool hasManaGain = false;
    protected Vector3 shotPos; //posizione in cui è stato sparato il proiettile
    private Stats fighterStats; //stat di chi ha sparato il proiettile (non confondere con quelle dell'arma)
    protected Coroutine deactivateBulletAfterTimeCoroutine;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector3 modScale;
    private int hitCount = 0;
    private int bounceCount = 0;
    
    protected virtual void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        homingMechanic = GetComponentInChildren<HomingMechanic>(true);
//        Debug.Log("Ecchitelo " + homingMechanic);
    }
    //VERY IMPORTANT METHOD FOR POOLING
    //in there you should put ALL of the internal attributes, if not done so, the instantiated pool bullet will have previous attributes
    //IMPORTANT NOTE: "OnEnable" is called AFTER the "Setup" method
    protected virtual void OnEnable() {
        shotPos = transform.position;  
        direction = transform.right;
        UpdateVelocity();
        deactivateBulletAfterTimeCoroutine = StartCoroutine(DeactivateBulletAfterTime());
        //transform.localScale = baseScale;
        SetBaseScaleAndRotation();
        ResetRotation();
        hitCount = 0;
        bounceCount = 0;
        if(homingMechanic != null) homingMechanic.SetHomingAura(1);
//        else Debug.LogWarning("HomingMechanic is null");
        //currentlyHomingToTarget = false;
        //SpawnPosCheck();
    }

    private void FixedUpdate() {
        DebugShowDirection();
        /*
        if(currentlyHomingToTarget) {
            Debug.Log("currently homing to target");
            float rotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = rotateAmount * 10000;
            rb.velocity = transform.right * weaponStats.projectileSpeed;
        }
        */
    }
    //note: i should set base scale or rotation through "spriteHitbox", not this.transform, "this" applies to directional rotations
    private void SetBaseScaleAndRotation() { 
        if(!unaffectedByScale)
            spriteHitbox.transform.localScale = modScale;
        else
            spriteHitbox.transform.localScale = baseScale;
        UpdateRotation(baseRotation);
    }

    private void SpawnPosCheck() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(shotPos, 0.1f);
        foreach(Collider2D collider in colliders) {
            //Debug.Log(collider.TryGetComponent(out ICollidable collObj));
            /*
            if(collider.TryGetComponent(out ICollidable collObj)) {
                Debug.Log("Invalid projectile spawn point: " + collider.gameObject.name);
                OnDisable();
                return false;
            }
            */
            HandleStartingCollision(collider);
        }
    }
    //main weapon setup
    public Bullet Setup(WeaponStats ws, ProjectileWeapon wpn, bool hasManaGain) {
//        Debug.Log("setup bullet v3.1 " + name);
        SetupStats(ws);
        SetupWielder(wpn);
        SetupVarious(weaponStats.weaponSize * Vector3.one, hasManaGain, weaponStats.projectileHoming);
        return this;
    }
    public virtual Bullet Setup(BaseWeaponStats ws, bool hasManaGain, Fighter wielder = null) {
//        Debug.Log("setup bullet v3.3 " + name);
        SetupStats(ws);
        SetupWielder(wielder);
        SetupVarious(weaponStats.weaponSize * Vector3.one, hasManaGain, weaponStats.projectileHoming);
        return this;
    }
    protected void SetupStats(BaseWeaponStats stats) {
        weaponStats = ScriptableObject.CreateInstance<WeaponStats>();
        weaponStats.SetWeaponStats(stats);
        //AddGlobalStats();
    }
    protected void SetupStats(WeaponStats stats) {
        weaponStats = Instantiate(stats);
        //AddGlobalStats();
    }
    private void AddGlobalStats() { if(wielder is Player p) weaponStats.AddToStats(p.globalWpnStats); }
    //new setup
    private void SetupVarious(Vector3 scale, bool mg, float homing = 0) {
        //Debug.Log("Setup " + (baseScale + scale - Vector3.one));
        //transform.localScale += scale - Vector3.one;
        modScale = baseScale + scale - Vector3.one;
        fighterStats = wielder.stats;
        hasManaGain = mg;
        disabled = false;
        SetupHoming(homing);
    } 
    private void SetupWielder(ProjectileWeapon wpn) { 
        if(wpn != null) { //used for main weapons
            sourceItem = wpn;
            wielder = wpn.GetWielder();
        }
        else //used for powerups
            wielder = GameManager.instance.player;
    }
    private void SetupWielder(Fighter wldr) { 
        if(wldr != null)  //used for main weapons
            wielder = wldr;
        else //used for powerups
            wielder = GameManager.instance.player;
    }
   // private bool currentlyHomingToTarget = false;
    public void SetHomingDirection(Vector3 dir) {
        if(homingMechanic == null) return;
        Debug.Log("SetHomingDirection: " + dir);
        AssignDirection(dir);
        //currentlyHomingToTarget = true;
    }
    private void SetupHoming(float homing) {
        if (homingMechanic == null) 
            return;
        bool isHoming = homing > 0;
//        Debug.Log("Bullet " + this + " " + homingAura.activeInHierarchy + " | " + homingMechanic.gameObject.activeInHierarchy + " | " + isHoming);
        if(isHoming)
            homingMechanic.SetHomingAura(homing);
        homingMechanic.gameObject.SetActive(isHoming);
    }

    #region various rotations
    protected void ResetRotation() {
        if(unaffectedByRotation) transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    protected void SetRotation(Vector3 rotation) {
        if(unaffectedByRotation) return;
        transform.rotation = Quaternion.Euler(rotation);
    }
    protected void UpdateRotation(Vector3 rotation) {
        if(unaffectedByRotation) return;
        transform.Rotate(rotation);
//        Debug.Log("UpdateRotation: " + rotation);
    }
    protected void UpdateRotation(float angle) { 
        if(unaffectedByRotation) return;
        transform.rotation = Quaternion.Euler(0, 0, angle + baseRotation.z);
    }
    #endregion
    protected virtual void UpdateVelocity() {
        //direction = transform.right;
//        Debug.Log("UpdateVelocity: velocity = " + weaponStats.projectileSpeed + " * " + speedModifier + " * " + direction);
        rb.velocity = weaponStats.projectileSpeed * direction;
    }

    public override void HandleCollision(Collider2D coll) {
        if(coll == null) { Debug.LogWarning("coll [" + coll + "] is null"); return; }
//        Debug.Log("HandleCollision: " + coll.name + " " + coll.gameObject.layer.ToString());
        if(coll.gameObject.TryGetComponent(out ICollidable collObj)) {
            bool disable = true; bool pierce = false;
            if(coll.gameObject.TryGetComponent(out IHittable hitObj)) {
                OnHit(hitObj, coll);
                if(hitCount >= weaponStats.projectilePierce) {
                    disable = true; 
                    //SetupHoming(0);
                }
                else pierce = true;
                hitCount++;
                //foreach (ContactPoint2D contact in coll.contacts) Debug.DrawRay(contact.point, contact.normal, Color.green, 2.0f);
            }
//            Debug.Log("Pierce: " + pierce + " | " + bounceCount + " < " + weaponStats.projectileBounce);
            if(bounceCount < weaponStats.projectileBounce && !pierce) {
                Bounce(coll);
                bounceCount++;
                disable = false;
            }
//            Debug.Log("Disable: " + disable);
            if(disable && !pierce) OnDisable();
        }
    }
    private void HandleStartingCollision(Collider2D coll) {
        if(coll.gameObject.TryGetComponent(out ICollidable collObj)) {
            bool disable = true;
            if(coll.gameObject.TryGetComponent(out IHittable hitObj)) {
                OnHit(hitObj, coll);
                if(hitCount == weaponStats.projectilePierce) 
                    disable = true;
                hitCount++;
            }
            if(disable) OnDisable();
        }
    }
    
    private void DebugShowDirection() {
        Debug.DrawLine(transform.position, transform.position + (Vector3)direction * 2, Color.red);
    }
    private void DebugShowCollisionPoint(Vector2 point, Vector2 normal) {
        Debug.DrawRay(transform.position, normal, Color.green, 2.0f);
    }
    //note: this bounce is not accurate, but it is the only choice as long it is used with ontriggerenter
    private void Bounce(Collider2D coll) {
        Vector2 collisionPoint = coll.ClosestPoint(transform.position);
        Vector2 normal = (transform.position - (Vector3)collisionPoint).normalized; //direzione opposta
        Vector2 newDirection = Vector2.Reflect(direction, normal); //direzione riflessa
        //Debug.Log("Bounce " + coll.name + " --> old dir: [" + direction + "] new dir: [" + newDirection + "]");
        ChangeDirection(newDirection);
        DebugShowCollisionPoint(collisionPoint, normal);
    }

    
    protected override void OnHit(IHittable hitObj, Collider2D coll) {
        //Debug.Log("Hit " + hitFighter);
        //List<StatusEffectData> statusEffects;
        //if(wielder is Player) statusEffects = GameManager.instance.player.globalWpnStats.statusEffects;
        //else statusEffects = weaponStats.statusEffects;
//        Debug.Log(homingMechanic.gameObject.activeSelf);
        //string s = disabled ? "x" : "y";
        if(disabled) return;
        //GameManager.instance.ShowText(s, coll.transform.position);
        Fighter hitFighter = coll.gameObject.GetComponent<Fighter>();
        Damage dmg = GameManager.instance.DamageCalculation(wielder, fighterStats, shotPos, hitFighter, weaponStats, 
            coll.ClosestPoint(transform.position),DamageType.normal,hasManaGain,weaponStats.statusEffects,this);
        hitObj.getHit(dmg, 0f);
        //OnDestroy();
        
    }
    /* onhit con Collision2D  (unused)
    protected virtual void OnHit(IHittable hitObj, Collision2D coll) {
        Fighter hitFighter = coll.gameObject.GetComponent<Fighter>();
        Damage dmg = GameManager.instance.DamageCalculation(wielder, fighterStats, shotPos, hitFighter, weaponStats, 
            coll.contacts[0].normal,DamageType.normal,hasManaGain,weaponStats.statusEffects,this);
        hitObj.getHit(dmg, 0f);
        //OnDestroy();
    }
    */

    protected IEnumerator DeactivateBulletAfterTime() {
        if(timeToDestroy <= 0) yield break; //e.g. orbitals

        float elapsedTime = 0;
        while(elapsedTime < timeToDestroy) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        OnDisable();
    }
    protected bool disabled = false;
    protected virtual void OnDisable() {
        if(disabled) return;
        //currentlyHomingToTarget = false;
        disabled = true;
        hitCount = 0;
        bounceCount = 0;
        SetupHoming(0);
        if(animator != null) {
            if(rb != null)
                rb.velocity = Vector2.zero;
            else
                Debug.LogWarning(gameObject.name + ": Rigidbody2D is null");
            
            animator.SetTrigger("explode");
        }
        else
            ReleaseFromPool();
        if(affectedByOnBulletDestroy)
            PowerupManager.instance.BulletDestroyedEvent(this);
        if(additionalBullet != null && transform != null && transform.parent.gameObject.activeInHierarchy)
            ObjectPoolManager.SpawnBullet(additionalBullet, transform.position, Quaternion.identity, 
                weaponStats, false, ObjectPoolManager.PoolType.PlayerBullet);
    }
    //this is also called in animation when it ends
    public void ReleaseFromPool() { 
        if(!affectedByPooling) return;
        ObjectPoolManager.ReturnObjectToPool(this); 
    }

    public Sprite GetSprite() { return spriteHitbox.GetComponent<SpriteRenderer>().sprite; }

    public void ChangeDirection(Vector3 dir) {
        AssignDirection(dir);
        UpdateRotation(Utility.GetAngleFromVectorFloat(direction)); //applica la rotazione al proiettile, altrimenti il proiettile rimbalza senza girarsi
        UpdateVelocity(); //aggiorna la velocità del proiettile, altrimenti il proiettile non cambia direzione
    }
    /*
    Vector2 tempVelocity;
    public void ChangeDirectionTemporarily(Vector3 dir) {
        tempVelocity = rb.velocity;
        ChangeDirection(dir);
    }
    public void RevertDirection() {
        if(tempVelocity == Vector2.zero) return;
        rb.velocity = tempVelocity;
        tempVelocity = Vector2.zero;
    }
    */
    public void AssignDirection(Vector3 dir) { direction = dir; }
    public WeaponStats GetWeaponStats() { return weaponStats; }
}
