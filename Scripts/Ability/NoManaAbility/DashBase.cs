using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBase : BaseAbility
{
    //il dash scala pure sulla velocità base del giocatore
    //[SerializeField] private float dashSpeed;
    [SerializeField] private float duration;
    [SerializeField] private const float addInvulnerability = 0.1f;
    [SerializeField] private float spdMultiplier = 2f;
    private const float addSpdFlatModifier = 3f;
    private TrailRenderer trailRenderer;
    protected override void Start() {
        base.Start();
        trailRenderer = GetComponent<TrailRenderer>();
    }
    protected override void UseSkill() {
        MenuManager.instance.ShowDashIndicatorFill(cooldown);
        StartCoroutine(DashSkillR());
        //player.currMp -= manaCost;
        //GameManager.instance.OnManapointChange();
        AudioManager.instance.PlayPooledSFX("Dash", transform, 1f);
    }
    private IEnumerator DashSkillR() {
        DashEffect(true);
        player.Push((player.stats.moveSpeed + addSpdFlatModifier) * spdMultiplier); 
        player.ChangeSpriteToFlash();

        yield return new WaitForSeconds(duration);
        
        player.StopMoving();

        yield return new WaitForSeconds(addInvulnerability);

        player.RevertSpriteToNormal();
        DashEffect(false);
    }
    private void DashEffect(bool flag) {
        player.stopMoving = flag;
        player.isUnhittable = flag;
        trailRenderer.enabled = flag;
    }
    public void UpgradeDash(float cdPercRed, float duration) {
        cooldown -= cooldown * cdPercRed;
        this.duration += duration;
    }
}
