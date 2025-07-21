using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour {
    public virtual void OnSpawn(int maxHP, string enemyName) {
        BossManager.instance.OnBossSpawn(maxHP, enemyName);
    }
    public virtual void OnHit(int currHP, int maxHP, int dmg) {
        BossManager.instance.UpdateBossHP(currHP, maxHP, dmg);
//        Debug.Log("On Hit");
    }
    public void OnDeath() {
        //if(currHP > 0) BossManager.instance.UpdateBossHP(currHP, maxHP, currHP);
        BossManager.instance.OnBossDeath();
//        Debug.Log("On Death");
    }
}
