using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAbility : TapAbility
{
    //[Header("AnimatorAbility")]
    protected Animator animator;
    protected override void Start()
    {
        base.Start();
        animator = GameManager.instance.player.inventory.getCurrRootWeapon().GetComponent<Animator>();
    }

}
