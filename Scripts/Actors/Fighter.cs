using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;
public abstract class Fighter : Mover, IHittable, IEffectable
{
    [Header("Fighter")]
    //General Parameters
    [SerializeField] protected Stats originalStat;
    [HideInInspector] public Stats stats;
    [HideInInspector] public int currHP = 69; 
    [HideInInspector] public int currMp;
    protected float knockbackRecoverySpeed = 0.2f; 
    protected float lastImmune = -999;
    public bool isInvulnerable = false;
    public bool knockImmunity = false;
    public bool isAttackBlocked = false;
    public bool isUnhittable = false;
    //Flash parameters
    [SerializeField] private Material flash;
    private Material currMaterial;
    private Color currColor;
    protected float flashDuration = 0.1f;
    [HideInInspector] public SpriteRenderer sprite;
    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] protected List<StatusEffectData> activeStatuses; //DO NOT REMOVE SERIALIZE FIELD

    protected override void Awake() {
        base.Awake();
        sprite = GetComponent<SpriteRenderer>();
        currMaterial = sprite.material;
        currColor = sprite.color;
    }
    protected virtual void Update() {
        HandleStatus();
    }
    protected virtual void OnEnable() {
        lastImmune = -999;
    }
    private readonly Color32 critColor = new(255,69,0,255);
    protected bool IsUnhittable() {
        return isUnhittable || !gameObject.activeInHierarchy;
    }
    protected bool CooldownCheck(float cooldown) {
//        Debug.Log("Fighter [" + gameObject + "]: " + Time.time + " - " + lastImmune + " > " + cooldown);
        return Time.time - lastImmune >= cooldown;
    }
    protected abstract void OnPostHit(); //used by inherited classes to handle post hit effects
    public virtual void getHit(Damage dmg, float hitCooldown)
    {
       // Debug.Log("Fighter [" + gameObject + "]: " + Time.time + " - " + lastImmune + " > " + hitCooldown);
        if(IsUnhittable()) return;
        if(CooldownCheck(hitCooldown)) {
//            Debug.Log(gameObject + " getting hit by " + dmg.hitSource + " --> " + dmg.amount + " damage");
            if(dmg.type != DamageType.status)
                lastImmune = Time.time;
            
            if(dmg.amount < 0) {
                Heal(-dmg.amount);
                return;
            }

            if(isInvulnerable)
                dmg.amount = 0;
            else
                ApplyDamage(dmg.amount);
            
            if(!knockImmunity) 
                StartCoroutine(KnockbackDelay((transform.position - dmg.origin).normalized, dmg.knockback));
            
            //General Parameters (Text,Sound)
                //Initialize
            int numberSize = 25;
            Color textColor = Color.red;
            if(!gameObject.CompareTag("Player")) //se chi sta subendo il danno non è il player
                if(!dmg.isCrit)
                    textColor = Color.white;
                else
                    textColor = critColor;
            if(dmg.type != DamageType.normal && dmg.type != DamageType.areaDamage) 
                textColor = dmg.textColor;
            
            //Various powerups checks
            if(dmg.hitter != null) {
                //Lifesteal check
                if(dmg.sourceHeal > 0) {
                    dmg.hitter.Heal(dmg.sourceHeal);
                }
                //Thorns check
                if(stats.thornsPercent > 0 && dmg.type != DamageType.thorns) { //nel caso nel quale il danno che si sta subendo è già da thorns
                    //target = player --> hitter = enemy
                    //Debug.Log("hitted " + this + "thorns target: " + dmg.hitter.gameObject.name + "damage taken: " + dmg.dmgAmount  + " thorns damage: " + stats.GetThornsDamage(dmg.dmgAmount));
                    Damage dmgS = GameManager.instance.RawDamageCalculation(this, dmg.hitter.transform.position, stats.GetThornsDamage(dmg.originalAmount), DamageType.thorns, Color.gray);
                    dmg.hitter.getHit(dmgS, 0);
                }
                //Manapoint regen
                if(dmg.type != DamageType.thorns) //senza questa condizione crasha perchè nel rawdamage non viene inizializzato il source item
                    if(dmg.hitter.stats.manapointGain > 0 && dmg.hasManaGain) {
                        GameManager.instance.player.currMp += dmg.hitter.stats.manapointGain;
                        GameManager.instance.OnManapointChange();
                    }
                //DoT check
                //Debug.Log(DamageType.status + " damage taken: " + dmg.dmgAmount);
                //if(dmg.type == DamageType.status) 
                ApplyMultipleStatus(dmg.statusEffects);
            }
            //Various Visual/Audio Effects
            //Flash effect
            if(gameObject.activeInHierarchy)
                StartCoroutine(FlashEffect());

            //Bleed particles effect
            if(hitParticle != null)
                ObjectPoolManager.SpawnObject(hitParticle.gameObject, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystem);

            //Sound effect
            /*
            if(AudioManager.instance != null && playSound) {
                if(this is Player)
                    AudioManager.instance.PlaySound("PlayerHit",false);
                else
                    AudioManager.instance.PlaySound("Hit",false);
            }
            */

            Vector2 min = dmg.hitPoint * 0.9f;
            Vector2 max = dmg.hitPoint * 1.1f;
            Vector2 textPos = new(UnityEngine.Random.Range(min.x,max.x),UnityEngine.Random.Range(min.y,max.y));
            //Show Text
            GameManager.instance.ShowText(dmg.amount.ToString(),numberSize,textColor,textPos,Vector3.up * 50,1.0f);
            //End
            //Handle death
            if(currHP <= 0) {
                //currHP = 0;
                Death(dmg.hitSource);
            }

            OnPostHit();
        }
    }
    protected int dmgTakenFixed;
    protected virtual void ApplyDamage(int damageReceived) {
        int newDamageReceived = ArmorCalculation(damageReceived);
        if(currHP - newDamageReceived <= 0) {
            dmgTakenFixed = currHP;
//            Debug.Log("Damage taken capped: " + dmgTakenCapped + " - Current HP: " + currHP + " - Damage Received: " + newDamageReceived);
        }
        else
            dmgTakenFixed = newDamageReceived;
        if(newDamageReceived > 0)
            currHP -= newDamageReceived;
        if(currHP <= 0) 
            currHP = 0;
    }

    protected abstract int ArmorCalculation(int damageReceived);

    protected abstract void Death(HitSource deathSource);
    
    public virtual IEnumerator FlashEffect() {
        ChangeSpriteToFlash();
        yield return new WaitForSeconds(flashDuration);
        RevertSpriteToNormal();
        yield return null;
    }
    public void ChangeSpriteToFlash() { sprite.material = flash; }
    public void RevertSpriteToNormal() { sprite.material = currMaterial; }

    public virtual void ChangeSpriteColor(Color32 color) { sprite.color = color; }
    public virtual void RevertSpriteColor() { sprite.color = currColor; }
    public void setAttackBlocked(bool b) {
        isAttackBlocked = b;
    }

    float slowSum = 0; Color32 colorAvg;
    private void MixStatuses() {
        slowSum = Utility.GetSumFromList(activeStatuses.Select(x => x.slowAmountPercent).ToList());
        if (slowSum > 100)
            slowSum = 100;
        colorAvg = Utility.GetAverageColorFromList(activeStatuses.Select(x => (Color32)x.color).ToList());
//        Debug.Log("Mixing Statuses: color avg [" + colorAvg + "] - slow sum [" + slowSum + "] - status count [" + activeStatuses.Count + "]");
    }
    private void AddStatusEffects(List<StatusEffectData> statuses) { //not very optimized but... it works
        foreach(StatusEffectData se in statuses) {
            StatusEffectData newSe = Instantiate(se);
            newSe.ApplyToTarget(this);
            activeStatuses.Add(newSe);
        }
        MixStatuses();
    }
    private void AddStatusEffect(StatusEffectData se) { 
        StatusEffectData newSe = Instantiate(se);
        newSe.ApplyToTarget(this);
        activeStatuses.Add(newSe);
        MixStatuses();
    }
    public void ApplyMultipleStatus(List<StatusEffectData> statusEffects) {
        //foreach(StatusEffectData se in statusEffects) Debug.Log("Applying status: " + se.statusName + " to " + gameObject.name);
        
        if(statusEffects == null || statusEffects.Count <= 0)
            return;
        foreach (StatusEffectData statusEffect in statusEffects)
            activeStatuses.RemoveAll(e => e.statusName == statusEffect.statusName);
        AddStatusEffects(statusEffects);
        ChangeSpriteColor(colorAvg);
        foreach(StatusEffectData se in activeStatuses) {
            movementPenalty = 1 - slowSum / 100;
            if(se.damageOverTime > 0)
                se.StatusEffectDamage();
        }
    }
    public void ApplyStatus(StatusEffectData statusEffect) {
        activeStatuses.RemoveAll(e => e.statusName == statusEffect.statusName);
        AddStatusEffect(statusEffect);
        ChangeSpriteColor(colorAvg);
        foreach(StatusEffectData se in activeStatuses) {
            movementPenalty = 1 - slowSum / 100;
            if(se.damageOverTime > 0)
                se.StatusEffectDamage();
        }
    }

    public void RemoveStatus(StatusEffectData se) {
        activeStatuses.Remove(se);

        movementPenalty = 1;
        RevertSpriteColor();
        se.OnRemove();
        if(activeStatuses.Count > 0) {
            MixStatuses();
            ChangeSpriteColor(colorAvg);
        }
    }
    /*
        public void RemoveStatus(StatusEffectData se) {
            MixStatuses();
            ChangeSpriteColor(colorAvg);
            if(activeStatuses.Count == 0) {
                movementPenalty = 1;
                RevertSpriteColor();
            }
            se.OnRemove();
        }
    */
    
    public void HandleStatus() { //this may have some performance issues when too much statuses are applied
        if(activeStatuses.Count == 0) return;
        //if(activeStatuses.Count == 1) colorAvg = activeStatuses[0].color;
        
        for (int i = activeStatuses.Count - 1; i >= 0; i--) {
            StatusEffectData se = activeStatuses[i];
            se.Update();
            if (se.IsFinished) {
                //activeStatuses.RemoveAt(i);
                RemoveStatus(se);
            }
        }
    }
    /*
    private void StatusEffectDamage(int damage, Color textColor) {
        if(damage != 0) 
            getHit(GameManager.istance.RawDamageCalculation(null,this,damage,DamageType.status,textColor),0);
    }
    */

    public virtual void Heal(int amount) {
        currHP += amount;
        if(currHP > stats.healthpoint)
            currHP = stats.healthpoint;
        //Debug.Log("Healed: " + amount + " - Current HP: " + currHP);
    }

    public virtual void RestoreMana(int amount) {
        currMp += amount;
        if(currMp > stats.manapoint)
            currMp = stats.manapoint;
    }
    
    public virtual void InstantiateStats(Stats stat) {
        stats = Instantiate(stat);
        currHP = stats.healthpoint;
        currMp = stats.manapoint;
        AdjustAnimatorMovSpd();
    }

    protected void AdjustAnimatorMovSpd() {
        float speedMultiplier = stats.moveSpeed / 2;
        speedMultiplier = Mathf.Clamp(speedMultiplier, 1f, 2f);
        if(animator != null) 
            animator.SetFloat("MoveSpeed", speedMultiplier);
    }
}   
