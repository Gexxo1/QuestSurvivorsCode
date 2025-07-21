using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : Enemy
{
    [Header("King Slime")]
    [SerializeField] private Minion minion;
    protected void SpawnMinion(Vector3 pos) {
        //Instantiate(minion,pos,Quaternion.identity);
        ObjectPoolManager.SpawnObject(minion.gameObject,pos,Quaternion.identity,ObjectPoolManager.PoolType.Enemy);
    }
    
    public override void getHit(Damage dmg, float hitCooldown)
    {
        if(Time.time - lastImmune >= hitCooldown && DamageType.areaDamage != dmg.type) {
            Vector3 pos = transform.position + 
                new Vector3(Random.Range(-(transform.localScale.x/4),transform.localScale.x/4), 
                            Random.Range(-(transform.localScale.y/4),transform.localScale.y/4));
            SpawnMinion(pos);
        }
        base.getHit(dmg, hitCooldown);
    }

    
}
