using UnityEngine;
public class Bow : ProjectileWeapon
{
    //Shoot attributes
    public bool isCharging = false;
    //Charge indicators
    [SerializeField] private GameObject[] visualArrow;
    private TripleShot ability;
    //given by stats
    private Inventory invRef;
    protected override void Start()
    {
        base.Start();
        invRef = GameManager.instance.player.inventory;
        invRef.InstantiateIndicators(3);
            
        if(GameManager.instance.player.inventory.GetAbility().TryGetComponent(out TripleShot ca))
            ability = ca;
    }
    /*
    protected override void Update() {
        Charge();
        if(CanShoot())
            Shoot();
    }
    */
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
        invRef.SetIndicatorActive(0,true);
    }
    public void setCharge2() { 
        invRef.SetIndicatorActive(1,true);
    }
    public void setCharge3() { 
        invRef.SetIndicatorActive(2,true);
    }
    public void setChargeFinal() { 
        chargeFinal = true;
    }
    private void Charge() {
        isCharging = true;

        if(additionalBullets == 1) SetVisualArrowActive(true, 1);
        if(additionalBullets == 2) SetVisualArrowActive(true, 2);

        anim.SetBool("isCharging", isCharging);
        SetLastShot();
        if(ability != null) ability.SetLastUsed();
    }

    public override void Shoot() {
        isCharging = false;
        ResetShot();
        InstantiateBullets(true);
        //wielder.setAttackBlocked(false);
        PlaySFX("Bow",0.5f);
        PowerupManager.instance.PlayerShootEvent(wielder); 
    }

    public void ResetShot() {
        chargeFinal = false;

        invRef.DisableAllIndicators();

        anim.SetBool("isCharging", false);

        if(additionalBullets == 2) SetVisualArrowActive(false, 1);
        if(additionalBullets == 3) SetVisualArrowActive(false, 2);
    }
    
    public void IncreaseVisualArrowsSize(Vector3 sizeIncrease) {
        foreach(GameObject arrow in visualArrow) 
            arrow.transform.localScale += sizeIncrease;
    }
    public void SetVisualArrowsSize(Vector3 sizeIncrease) {
        foreach(GameObject arrow in visualArrow) 
            arrow.transform.localScale = sizeIncrease;
    }
    public void SetVisualArrowActive(bool active, int index) {
        visualArrow[index].SetActive(active);
    }
    protected override bool PlayerInput() {
        if(ability == null) return base.PlayerInput();
        return base.PlayerInput() && !ability.isCharging;
    }

    public bool IsFullyCharged() { return chargeFinal; }

    public override string GetWeaponType() {
        return "Charged";
    }
}
