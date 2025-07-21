using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : TieredItem
{
    [Header("Powerup")]
    public bool isVisible = false;
    protected SpriteRenderer spriteRenderer;
    protected Player player;
    public bool isMythic = false;
    protected virtual void Awake() {
        player = GameManager.instance.player;
    }
    protected virtual void OnEnable() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = isVisible;
    }
    //protected virtual void Start() { Setup(); }
    public virtual void Effect() { }

    public void tryUpgradePowerup() {
//        Debug.Log("Trying upgrade " + title);
        if(isMaxedOut) {
            Debug.LogWarning("Unexpected behaviour: Trying to upgrade a maxed out item");
            return;
        }
        if(TryIncreaseTier()) {
            onUpgrade();
            UpdateUI();
        }
        else 
            Debug.Log("powerup is maxed out");
        
        if(isMaxedOut) {
            if(!isMythic)
                PowerupTableManager.instance.RemoveAvailability(title);    
            else
                PowerupTableManager.instance.RemoveAvailabilityMythic(title);  
        }
        
    }
    
    public virtual void Setup(int newTier) { //setup nelle classi ereditate va messo dopo
//        Debug.Log("Setup called - MUST BE CALLED ONCE " + title);
        isMaxedOut = false;
        isReset = false;
        if(!isMythic)
            maxTier = 5;
        else
            maxTier = 3;
        tier = 0;
        //SetTier(newTier);
        UpdateUI();
//        Debug.Log("setup called");
    }
    
    public abstract void onUpgrade();
    private void UpdateUI() {
        if(!isMythic) {
            MenuManager.instance.UpdateItemHUD();
            MenuManager.instance.UpdateItemTierHUD();
        }
        else {
            MenuManager.instance.UpdateMythicHUD();
            MenuManager.instance.UpdateMythicTierHUD();
        }
    }
    /*
    public virtual string GetDetailedDescription() {
        return GetDescription();
    }
    public virtual string GetDetailedUpDesc() {
        return GetUpgrDesc();
    }
    */
    protected bool isReset;
    protected virtual void ResetItem() {
        tier = 0;
        isMaxedOut = false;
        isReset = true;
//        Debug.Log("Resetting item " + title);
    }
    protected void OnDisable() {
        if(gameObject != null)
            ResetItem();
    }
}
