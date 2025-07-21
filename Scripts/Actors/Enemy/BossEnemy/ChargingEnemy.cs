using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : AbilityEnemy {
    [Header("Charging Enemy")]
    [SerializeField] GameObject exclamationMark;
    [SerializeField] GameObject directionArrow;
    //private GameObject telegraphInstance;
    [SerializeField] protected float notificationDelay = 2f;
    [SerializeField] float chargeDuration = 0.5f;
    [SerializeField] float chargeSpeed = 5f;
    private Vector2 chargeDirection;

    protected override void UseAbility() {
        abilityInProcess = true;
        //canMove = false;
        NotifyAbility();
        StartCoroutine(PerformAbilityAfterDelay(notificationDelay));
    }

    private void NotifyAbility() {
        exclamationMark.SetActive(true);
        //directionArrow.SetActive(true);
                
    }
    /*
    protected override void Update() {
        base.Update();
        ChangeArrowDirection();
    }

    private void ChangeArrowDirection() {
        float angle = Mathf.Atan2(chargeDirection.y, chargeDirection.x) * Mathf.Rad2Deg;
        // Imposta la rotazione della freccia
        directionArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //orbit around the enemy
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * directionArrow.transform.position.x;
        directionArrow.transform.position = transform.position + offset;
    }
    */
    
    private IEnumerator PerformAbilityAfterDelay(float delay) {
        
        yield return new WaitForSeconds(delay);
        PerformAbility();
    }

    private void PerformAbility() {
        exclamationMark.SetActive(false);
        //directionArrow.SetActive(false);
        //telegraphInstance.SetActive(false);
        StartCoroutine(Dash());
    }
    private IEnumerator Dash() {
        DashEffect(true);
        //isDashing = false;
        
        Push(chargeDirection,stats.moveSpeed * chargeSpeed); 
        //ChangeSpriteToFlash();

        yield return new WaitForSeconds(chargeDuration);

        //RevertSpriteToNormal();
        DashEffect(false);

        abilityInProcess = false;
        //canMove = true;
    }
    private void DashEffect(bool flag) {
        stopMoving = flag;
        knockbackImmunity = flag;
        //trailRenderer.enabled = flag;
    }

    protected override bool CanUseAbility() {
//        Debug.Log("CanUseAbility " + timeCounter + " >= " + randAbilityCd + " && " + abilityInProcess);
        return base.CanUseAbility() && !abilityInProcess;
    }

    protected override bool EnemyMoveCond() {
        return base.EnemyMoveCond() && !abilityInProcess;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        chargeDirection = (GameManager.instance.player.transform.position - transform.position).normalized;
    }
}
