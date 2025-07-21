using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : TapAbility
{
    private SpriteRenderer visualIndication;
    [SerializeField] private float duration = 2f;
    protected override void Start()
    {
        base.Start();
        visualIndication = GetComponent<SpriteRenderer>();
        visualIndication.enabled = false;
    }

    protected override void UseSkill() {
        base.UseSkill();
        DrainMana();
        StartCoroutine(BlockRoutine(duration));
    }


    private IEnumerator BlockRoutine(float time) {
        ActivateShield(true);
        
        yield return new WaitForSeconds(time);

        ActivateShield(false);

        yield return null;
    }

    private void ActivateShield(bool isActive) {
        visualIndication.enabled = isActive;
        player.isInvulnerable = isActive;
    }
}
