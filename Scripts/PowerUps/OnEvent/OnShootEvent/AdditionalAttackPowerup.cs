using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalAttackPowerup : OnShootPowerup {
    [SerializeField] private int[] bulletThreshold = new int[5];
    [SerializeField] private int[] additionalBulletsNo = {1,1,1,1,1};
    public override void OnPlayerShoot(int shootCounter) {
//        Debug.Log(shootCounter + " % " + bulletThreshold[tier] + " = " + shootCounter % bulletThreshold[tier]);
        if(shootCounter % bulletThreshold[tier] == 0 && shootCounter != 0) {
            player.inventory.getCurrWeapon().SetAdditionalBullets(additionalBulletsNo[tier]);
            //Debug.Log("Additional bullets: " + additionalBulletsNo[tier]);
        }
        
    }

    public override void onUpgrade() {
        //non è necessario riempire questo metodo perchè il tier viene già incrementato in "tryincreaseTier"
    }
    public override string GetDescription() {
        string s = additionalBulletsNo[tier] >= 2 ? "bullet" : "bullets";
        return "Every " + bulletThreshold[tier] + " main weapon attacks, your next attack will shoot " + additionalBulletsNo[tier] + " additional " + s + "\n";
    }

    public override string GetUpgrDesc() {
        string s = "";
        if(additionalBulletsNo[tier] != additionalBulletsNo[tier+1])
            s = "Additional bullets number: " + additionalBulletsNo[tier+1] + "\n";
        return "Now additional bullet requires " + bulletThreshold[tier+1] + " attacks \n" + s; 
    }
}
