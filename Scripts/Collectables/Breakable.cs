using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
public class Breakable : Collectable, IHittable
{
    //On Collect
    private int goldAmount;
    //public int rangeGoldMax = 5;
    [SerializeField] private DropList drops;
    //Receive dmg 
    public int durability = 1;
    protected override void OnEnable() {
        base.OnEnable();
        durability = 1;
    }
    protected override void OnTriggerStay2D(Collider2D coll)
    {
        //intentionally blank to overwrite base class
    }
    public override void OnPickup()
    {
        if(collected)
            return;
        GameManager.instance.dropItem(drops.getDropList(),transform.position,transform.localScale/2);
        base.OnPickup();
    }

    public void getHit(Damage dmg, float immuneTime)
    {
        durability -= dmg.amount;

        //Break
        if(durability <= 0) {
            durability = 0;
            OnPickup();
            if(AudioManager.instance != null)
                AudioManager.instance.PlayPooledSFX("Break",transform,0.5f);
            GameManager.instance.IncreasePotBreakCounter(1);
        }
        //}
    }
}
