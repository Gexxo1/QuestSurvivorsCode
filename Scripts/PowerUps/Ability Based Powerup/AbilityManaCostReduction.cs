using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManaCostReduction : PowerupAbility
{
    [SerializeField] [Range(0,100)] private int[] manaCostReductionPercent = new int[5];
    
    protected override void Awake() {
        base.Awake();
        
    }
    protected override void Start() {
        base.Start();
        SetManaReduction();
    }
    public override void onUpgrade() {
        base.onUpgrade();
        SetManaReduction();
    }

    private void SetManaReduction() {
        if(ability == null) {
            Debug.LogWarning("Ability not found, mana cost reduction not applied");
            return;
        }
        ability.SetManaReduction((float)manaCostReductionPercent[tier]/100);
        MenuManager.instance.UpdateAbilityManaCost();
    }

    public override string GetDescription() {
        string s = "";
        int index;
        if(ability == null && GameManager.instance != null) {
            ability = GameManager.instance.player.inventory.GetAbility();
            index = tier;
            Debug.LogWarning("Ability not found, mana cost reduction not applied");
        }
        else if(isReset)
            index = 0;
        else
            index = tier+1;
//        Debug.Log("Ability: " + (float)manaCostReductionPercent[index]/100);
        if(ability != null)
            s = "<i>Next actual mana cost: " + (ability.GetTrueManaCost() - (int)(ability.GetTrueManaCost() * (float)manaCostReductionPercent[index]/100)) + "</i>";

        return "Ability now costs " + GetTotalManaCostReduction(tier) + "% less mana\n" + s;
    }

    private int GetTotalManaCostReduction(int t) {
        int total = 0;
        for(int i = 0; i <= t; i++)
            total += manaCostReductionPercent[i];
        return total;
    }
}
