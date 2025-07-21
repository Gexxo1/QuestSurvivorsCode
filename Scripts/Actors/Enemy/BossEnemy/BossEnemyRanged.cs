using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossEnemyRanged : RangedEnemy {
    [Header("Boss Enemy Ranged")]
    //[SerializeField] private Enemy minion;
    [HideInInspector] public bool isFinalBoss;
    //protected void SpawnMinion(Vector3 pos) { Instantiate(minion,pos,Quaternion.identity); }
    [SerializeField] private int abilityCd = 0;    
    protected override void OnEnable() {
        base.OnEnable();
        InitAbilityAttributes();
    }
    protected int minCd, maxCd;
    private int attackCounter = 0;
    protected virtual void InitAbilityAttributes() {
        minCd = (int)(abilityCd * 0.5f);
        if(minCd <= 0)
            minCd = 1;
        maxCd = (int)(abilityCd * 1.5f);
        randomInterval = 1;
        attackCounter = 0;
    }
    
    protected override void Attack() {
        if(attackCounter % randomInterval == 0 && attackCounter != 0) 
            BossAbility();
        else {
            attackCounter++;
            rangedWeapon.Shoot();
        }
    }
    protected int randomInterval = 1;
    protected int GenerateRandomInterval() { return Random.Range(minCd, maxCd); }

    protected virtual void BossAbility() {
        attackCounter = 0;
        randomInterval = GenerateRandomInterval();
    }
}
