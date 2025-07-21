using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Enemy
{
    protected override void Death(HitSource deathSource) {
        if(!isDead) {
            if(drops != null)
                GameManager.instance.dropItem(drops.getDropList(),transform.position,transform.localScale/2);
            //PowerupManager.instance.EnemyKilled(transform.position); to add
            DestroyEnemy(1,false);
        }
    }
}
