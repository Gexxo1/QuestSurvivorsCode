using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TieredItem : Item
{
    [Header("Tiered Item")]
    [HideInInspector] public int tier;
    protected int maxTier = 3; //in powerup questo valore viene sovrascritto da setup
    [HideInInspector] public bool isMaxedOut;
    public bool TryIncreaseTier() {
        //Debug.Log(tier + " < " + (maxTier-1));
        if(tier < maxTier-1) {
            tier++;    
//            Debug.Log("tier = " + tier);
            if(tier == maxTier-1)
                isMaxedOut = true;
            return true;
        }
        return false;
    }
    public virtual void SetTier(int value) {
//        Debug.Log("set tier base " + title);
        tier = value;
    }
    /*
    private void OnDisable() {
        if(gameObject != null)
            ResetItem();
    }
    */
    public virtual string GetUpgrDesc() {
        return GetDescription();
    }
    public int GetVisualTier() {
        //Debug.Log(title + " Visual tier: " + (tier+1));
        return tier + 1;
    }

    public string GetTierRoman(int add) {
        return Utility.ToRoman(GetVisualTier()+add);
    }

    public string GetTierRoman() {
        return Utility.ToRoman(GetVisualTier());
    }

    public override string ToString() {
        return base.ToString() + "Tier [" + tier + "] MaxTier [" + maxTier + "] isMaxedOut [" + isMaxedOut + "]";
    }
}
