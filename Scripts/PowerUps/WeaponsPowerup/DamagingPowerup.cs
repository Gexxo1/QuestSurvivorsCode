using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagingPowerup : WeaponPowerup
{
    /*
    protected virtual void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.name == "Player") 
            return;
        if(coll.TryGetComponent(out IHittable hitObj)) 
            OnHit(coll,hitObj);
    }

    protected virtual void OnHit(Collider2D coll, IHittable hitObj)
    {
        //Attenzione: il powerup ha un suo danno, decidere se è influenzato o no dalle stats
        Debug.LogWarning("you're trying to call 'OnHit' method on class 'DamagingPowerup', call this method from inherited members instead");
    }
    */

}