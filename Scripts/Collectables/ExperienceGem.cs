using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : MagnetCollectable
{
    [SerializeField] int xpValue;
    public override void OnPickup()
    {
        if(collected)
            return;
        //AudioManager.instance.PlayPooledSFX("Gem",transform,1f);
        GameManager.instance.GrantXp(xpValue);
        base.OnPickup();
    }
}
