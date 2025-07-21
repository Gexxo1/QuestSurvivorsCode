using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    /*
    protected override void Death(HitSource deathSource) {
        if(!isDead) {
            PowerupManager.instance.EnemyKilledEvent(this);
            if(drops != null && !isFinalBoss)
                GameManager.instance.dropItem(drops.getDropList(),transform.position,transform.localScale/2);
            
            DestroyEnemy();
        }
    }
    */
    /*
    protected override void Death()
    {
        if(!isDead) {
            base.Death();
            if(!isFinalBoss)
                MenuManager.instance.ShowLevelUpMenu(true);
        }
    }
    */
}
