using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MagnetCollectable
{
    public int goldValue = 1;
    public override void OnPickup()
    {
        if(collected)
            return;
        OnCollect();
        base.OnPickup();
    }
    public override void OnCollect() {
        AudioManager.instance.PlayPooledSFX("Coin",transform);
        GameManager.instance.OnCoinGet(goldValue,true,transform.position);
    }
    public override string GetDescription()
    {
        return "+" + goldValue + " gold";
    }
}
