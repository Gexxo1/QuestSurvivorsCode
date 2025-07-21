using System.Collections;
using UnityEngine;

public class Enemy : Fighter
{
    [Header("Enemy")]
    [SerializeField] protected string enemyName = "Enemy Name Not Set";
    [SerializeField] private bool isDummy = false;
    [SerializeField] protected DropList drops;
    [SerializeField] protected Canvas hpCanvas;
    protected RectTransform hpBar;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool collidingWithPlayer;
    protected Player playerRef;
    protected Transform playerTransform;
    protected BossBehaviour bossBehavior;
    protected override void Awake() {
        base.Awake();
        if(originalStat == null) {
            Debug.LogWarning("original Stat is null");
            return;
        }
            
        InstantiateStats(originalStat);

        if(TryGetComponent(out BossBehaviour b)) 
            bossBehavior = b;
            
        if(hpCanvas != null)
            hpBar = hpCanvas.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>();
    }
    protected void Start() {
        playerRef = GameManager.instance.player;
    }
    protected override void OnEnable() {
        base.OnEnable();
        isDead = false;
        collidingWithPlayer = false;
        isKnocked = false;
        currHP = stats.healthpoint;
        activeStatuses.Clear();
        RevertSpriteToNormal();
        RevertSpriteColor();

        //aim related
        weaponBase = transform.GetChild(0).gameObject;
        aimTransform = weaponBase.transform;

        //boss related
        if(bossBehavior != null) 
            StartCoroutine(WaitForMenuManagerInstance());
            //bossBehavior.OnSpawn(stats.healthpoint, GetEnemyName());
        //bossBehavior?.OnSpawn();
        UpdateHpBar();
    }
    private IEnumerator WaitForMenuManagerInstance() {
        // Aspetta fino a quando MenuManager.instance non è inizializzato
        while (MenuManager.instance == null) {
            yield return null;
        }
        Debug.Log("Ecchime");
        bossBehavior.OnSpawn(stats.healthpoint, GetEnemyName());
    }
    public override void getHit(Damage dmg, float hitCooldown) {
        
        //if(dmg.hitSource == null) Debug.LogWarning("Hit source is null: " + dmg.hitter);
        if(!CooldownCheck(hitCooldown) && IsUnhittable())
            return;
        //Debug.Log("Enemy getHit: " + dmg.hitSource);
        if(dmg.playSound) 
            AudioManager.instance.PlayPooledSFX("Hit2",transform,0.4f);
            //AudioManager.instance.PlayPooledSFX("Hit",transform,0.5f);
        base.getHit(dmg, hitCooldown);

        //int fixedDamage = dmg.amount;
        PowerupManager.instance.EnemyHitEvent(this, dmg.hitSource);
    }

    protected override void ApplyDamage(int damageReceived) {
        base.ApplyDamage(damageReceived);
        bossBehavior?.OnHit(currHP, stats.healthpoint, dmgTakenFixed);
    }

    //aggro
    private int animatorHash = Animator.StringToHash("EnemyMoving");
    protected bool isMoving;
    protected override void FixedUpdate() {
        
        if(animator != null) 
            animator.SetBool(animatorHash, isMoving);
        
        if(!isDummy) {
            AimToPlayer();
            Aggro();
        }
            
    }
    protected virtual void Aggro() {
        if(stopMoving) return;
        if(EnemyMoveCond()) 
            Movement((playerTransform.position - transform.position).normalized,stats.moveSpeed);
        else 
            StopMoving();
        
    }

    public override void Movement(Vector3 direction, float modifier) {
        base.Movement(direction,modifier);
        isMoving = true;
    }
    public override void StopMoving() {
        base.StopMoving();
        isMoving = false;
    }

    protected virtual bool EnemyMoveCond() {
        return !collidingWithPlayer;
    }
    /*
    private void OnTriggerEnter2D(Collider2D coll) {
        if(coll.TryGetComponent(out Player p)) 
            collidingWithPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D coll) {
        if(coll.TryGetComponent(out Player p)) 
            collidingWithPlayer = false;
    }
    */
    protected override void Death(HitSource deathSource)
    {
        if(!isDead) {
            if(drops != null)
                GameManager.instance.dropItem(drops.getDropList(),transform.position,transform.localScale/2);
            PowerupManager.instance.EnemyKilledEvent(this);
            if(deathSource != null)
                PowerupManager.instance.EnemyExecutedEvent(deathSource);
            if(activeStatuses.Count > 0) { //check if has any active 'on death status effects' like crimson curse (crimson mage ability)
                foreach(StatusEffectData se in activeStatuses) 
                    if(se is HealSEData healStatus) 
                        healStatus.HealPlayer();
            }
//            Debug.Log(name + " is dead with " + currHP + " hp left");
            bossBehavior?.OnDeath(); //this applies only to bosses (in fact it checks if has boss behavior component)
            DestroyEnemy();
        }
    }

    protected void DestroyEnemy(int killValue = 1, bool waveEnemy = true) {
        if(isDead) return;
        isDead = true;
        GameManager.instance.UpdateKills(killValue, gameObject, waveEnemy);
        bool flag = ObjectPoolManager.TryReturnObjectToPool(gameObject);
        if(!flag) {
            Debug.LogWarning("Enemy " + name + " not found in pool, destroying it manually.");
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    //Aim Code

    private Transform aimTransform;
    protected Vector3 aimDirection;
    private GameObject weaponBase;
    private float angle;
    protected virtual void AimToPlayer() {
        playerTransform = playerRef.transform;
        aimDirection = (playerTransform.position - weaponBase.transform.position).normalized;  
        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        aimTransform.eulerAngles = new Vector3(0,0,angle); //valore non corretto
        //Gira lo sprite quando l'enemy è a destra o a sinistra del player
        Vector3 aimLocalScale = Vector3.one;
        if(angle > 90 || angle < -90) {
            aimLocalScale.y = -1f;
            sprite.flipX = true;
        }
        else {
            aimLocalScale.y = +1f;
            sprite.flipX = false;
        }
        aimTransform.localScale = aimLocalScale;
    }
    public Vector3 GetAimDirection() { return aimDirection; }
    public void SetDropsList(DropList d) { drops = d; }
    
    protected override void OnPostHit() {
        UpdateHpBar();
    }

    protected void UpdateHpBar() { 
        if(hpCanvas == null || !hpCanvas.isActiveAndEnabled) return;
        
        float hpRatio = currHP / (float)stats.healthpoint;
        hpBar.localScale = new Vector3(hpRatio,1,1);
    }

    public void SetHpBarActive(bool flag) {
        if(hpCanvas != null)
            hpCanvas.gameObject.SetActive(flag);
    }

    public string GetEnemyName() { return enemyName; }

    protected override int ArmorCalculation(int damageReceived) {
        /*
        damageReceived -= stats.armor;
        if(damageReceived <= 0)
            damageReceived = 1; //il danno non può andare sotto lo 0
        return damageReceived;
        */
        return Mathf.Max(damageReceived - stats.armor, 1);
    }
}   
