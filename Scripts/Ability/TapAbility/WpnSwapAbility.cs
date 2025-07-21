using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpnSwapAbility : ActivationAbility {
    
    [SerializeField] WeaponTopHierarchy weapon;
    protected override void Start() {
        base.Start();
        player.inventory.AddWeaponToInventory(weapon);
    }
    protected override void AbilityActivation() {
        SwapWeapon();
    }
    protected override void AbilityDeactivation() {
        SwapWeapon();
    }
    private void SwapWeapon() {
        player.inventory.SwapWeapon();
    }
}
