using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupAbility : PowerUp
{
    protected Ability ability;
    protected virtual void Start() {
        ability = player.inventory.GetAbility();
//        Debug.Log("Ability: " + ability);
    }
    public override void onUpgrade() {
        
    }
    
}
