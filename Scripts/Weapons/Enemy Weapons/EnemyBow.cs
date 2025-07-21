
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBow : EnemyProjectileWeapon
{
    [Header("Enemy Bow")]
    public bool isCharging = false;
    [SerializeField] Renderer[] chargeIndicators;
    [SerializeField] private GameObject[] visualArrow;
    protected override void Update() {
        if(CanShoot())
            Charge();
        if(chargeFinal && isCharging)
            Shoot();
    }
    [SerializeField] private bool chargeFinal = false;
    //eseguiti dall'animator
    public void setCharge1() { 
        //wielder.setAttackBlocked(true);
        chargeIndicators[0].gameObject.SetActive(true);
    }
    public void setCharge2() { 
        chargeIndicators[1].gameObject.SetActive(true);
    }
    public void setCharge3() { 
        chargeIndicators[2].gameObject.SetActive(true);
    }
    public void setChargeFinal() { 
        chargeFinal = true;
    }
    private void Charge() {
        isCharging = true;
        anim.SetBool("isCharging", isCharging);
        SetLastShot();
    }

    public override void Shoot() {
        isCharging = false;
        ResetShot();
        InstantiateBullets();
    }

    public void ResetShot() {
        chargeFinal = false;
        foreach(Renderer chargeIndicator in chargeIndicators)
            chargeIndicator.gameObject.SetActive(false);
        anim.SetBool("isCharging", false);
    }
    
    public bool IsFullyCharged() { return chargeFinal; }
}
