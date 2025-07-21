using UnityEngine;

public abstract class WeaponHoldAbility : HoldAbility 
{
    protected WeaponTopHierarchy topWpn;
    protected ProjectileWeapon wpn;
    protected Animator anim;
    protected GameObject wpnOrigin;
    protected bool isAnimationPlaying = false;
    protected override void Start() {
        base.Start();
        topWpn = GameObject.Find("Weapons").GetComponentInChildren<WeaponTopHierarchy>();
        wpn = topWpn.getWeaponCore();
        anim = wpn.anim;
        wpnOrigin = topWpn.getWpnOrigin();
    }
    
    protected override void DeactivateSkill() {
        base.DeactivateSkill(); 
        SetMainWeaponLastShot();
    }
    
    //Used in animations - DO NOT MODIFY NAME OR DELETE
    public void BlockPlayerInput() {
        isAnimationPlaying = true;
        player.setAttackBlocked(true);
    }
    public void RestorePlayerInput() {
        isAnimationPlaying = false;
        player.setAttackBlocked(false);
    }

    protected override bool StartCondition() {
        return base.StartCondition() && !isAnimationPlaying;
    }

    public void SetMainWeaponLastShot() {
        wpn.SetLastShot();
    }

    protected override bool StartInput() {
        return base.StartInput() && !Input.GetKey(KeyCode.Mouse0);
    }

}
