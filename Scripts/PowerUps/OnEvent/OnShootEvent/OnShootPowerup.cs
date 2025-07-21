using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnShootPowerup : PowerUp
{
    protected override void Awake() {
        base.Awake();
        PowerupManager.OnPlayerShoot += OnPlayerShoot;
    }
    void OnDestroy() {
        PowerupManager.OnPlayerShoot -= OnPlayerShoot;
    }
    public abstract void OnPlayerShoot(int shootCounter);
}
