using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingBoss : ChargingEnemy {
    [Header("Charging Boss")]
    [SerializeField] List<Increase> increases;
    [Serializable] //note: the successive element of the list should have a lower thresold and an higher reduction than previous element
    public struct Increase { 
        [SerializeField] [Range(0,100)] public int hpThresholdPercent;
        [SerializeField] [Range(0,100)] public int reductionPercentage;
    }
    
    private int currentIndex = 0;
    float hpThreshold;
    public override void getHit(Damage dmg, float hitCooldown) {
        base.getHit(dmg, hitCooldown);
        if(currHP <= 0 && currentIndex > increases.Count) return;
        
        do {
            if(currentIndex >= increases.Count) break;
            //Debug.Log("Cycle " + currentIndex + " > " + increases.Count);

            hpThreshold = stats.healthpoint * increases[currentIndex].hpThresholdPercent / 100f;
            //Debug.Log("Condition --> " + currHP + " >= " + hpThreshold);

            if(currHP >= hpThreshold) break;

            //if(prevCd == -1) prevCd = randAbilityCd;
            //else randAbilityCd = prevCd;
            
            baseAbilityCd *= 1 - increases[currentIndex].reductionPercentage / 100f;
            notificationDelay *= 1 - increases[currentIndex].reductionPercentage / 100f;
            if(notificationDelay < 0.5f) notificationDelay = 0.5f;
            Debug.Log("Charging Boss: New Cd " + baseAbilityCd);
            ExtractRandomCooldown();
            //Debug.Log("Cd reduction " + prevCd + " -> " + randAbilityCd);
            currentIndex++;

        } while(true);
    }

}

