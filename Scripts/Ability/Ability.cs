using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : Item
{
    [Header("Ability")]
    [SerializeField] protected int manaCost = 1; //this should not be called, it should be called via GetManaCost(), or else will be not affected by mana reduction
    [SerializeField] protected float cooldown = 5;
    protected float lastUsed = -999;
    protected Player player;
    protected Inventory inventory;
    protected bool freeCast = false;
    private float manaCostReduction = 0f;
    virtual protected void Awake() {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    virtual protected void Start() {
        player = GameManager.instance.player;
        inventory = player.inventory;
    }

    virtual protected void Update() { }
    //bool nextCastIsFree = false;
    protected bool HasManaToCast { get { return player.currMp >= GetManaCost(); } }
    protected bool ManaRequirements() {
        //if(nextCastIsFree) return true;
        //bool hasManaToCast = player.currMp >= manaCost;
//        Debug.Log("ManaRequirements [" + hasManaToCast + "] NextCastIsFree[" + nextCastIsFree + "]");
        /*
        if(!hasManaToCast && !nextCastIsFree) {
            if() {
                //nextCastIsFree = true;
                return true;
            }
        }
        */
        return HasManaToCast || PowerupManager.instance.IsAbilityCastEventAvailable(player, GetManaCost());
    }
    protected void DrainMana() {
        //Debug.Log("Draining mana");
        /*
        if(nextCastIsFree) {
            nextCastIsFree = false;
            //Debug.Log("Ability free cast");
            return;
        }
        */
        if(freeCast) return;
        if(!HasManaToCast) {
            PowerupManager.instance.ConsumeAbilityCastEvent(player, GetManaCost());
            return;
        }
        player.currMp -= GetManaCost();
        GameManager.instance.OnManapointChange();
        GameManager.instance.OnManaUsed(GetManaCost());
    }
    abstract protected void UseSkill();
    protected virtual bool CooldownCondition() { return Time.time - lastUsed > GetCooldown(); }
    public virtual void SetLastUsed() { lastUsed = Time.time; }
    protected virtual float GetCooldown() { return cooldown; }
    protected abstract bool StartCondition();
    public abstract string GetAbilityType();
    public override string ToString() {
        return base.ToString() + " Type[" + GetAbilityType() + "]";
    }
    public override string GetDescription() {
        return base.GetDescription() + "\n" + GetRequirements();
    }
    protected virtual string GetRequirements() {
        return GetManaReq() + " Cooldown <color=#C82828>" + cooldown + "</color>s";
    }
    protected virtual string GetManaReq() {
        return "Mana cost <color=#2C47C0>" + GetManaCost() + "</color>";
    }
    public override string GetTitle() {
        string title = base.GetTitle() + " ";
        //string tier = Utility.ToRoman(GetVisualTier());
        //string type = " [" + GetAbilityType() + "]";
        return title 
        //+ type
        ;
    }

    public virtual SpriteRenderer GetSpriteRenderer() {
        return GetComponent<SpriteRenderer>();
    }
    public void SetFreeCast(bool flag) {
        freeCast = flag;
    }

    public int GetManaCost() {
        //Debug.Log("Mana cost: " + manaCost + " - " + (int)(manaCost * manaCostReduction));
        return manaCost - (int)(manaCost * manaCostReduction);
    }
    public int GetTrueManaCost() {
        return manaCost;
    }
    public void SetManaReduction(float reduction) {
        manaCostReduction = reduction;
//        Debug.Log("Mana cost reduction: " + reduction);
    }

    protected virtual bool TryDrainMana() {
        if(ManaRequirements()) {
            DrainMana();
            return true;
        }
        return false;
    }
}
