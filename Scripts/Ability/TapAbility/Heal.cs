using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : TapAbility
{
    [SerializeField] private float healPercent = 10;
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void UseSkill() {
        base.UseSkill();
        DrainMana();
        int healValue = (int)(healPercent / 100 * player.stats.healthpoint);
        GameManager.instance.HealPlayer(healValue);
    }
}
