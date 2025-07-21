using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Collectable
{
    [SerializeField] int healValue;
    [SerializeField] int manaValue;
    protected override void Start() {
        base.Start();
    }
    public override void OnPickup()
    {
        if(collected)
            return;
        OnCollect();
        base.OnPickup();
    }
    public override void OnCollect() {
        if(healValue != 0)
            GameManager.instance.HealPlayer(healValue);
        if(manaValue != 0)
            GameManager.instance.RestorePlayerMana(manaValue);
        AudioManager.instance.PlayPooledSFX("Heal",transform,1f);
    }
    public override string GetDescription()
    {
        string s = "";
        bool h = healValue != 0;
        bool m = manaValue != 0;
        if(h || m)
            s += "Restore ";
        if(h)
            s += healValue + " health";
        if(h && m)
            s += " and ";
        if(m)
            s += manaValue + " mana ";
        return s;
    }

}
