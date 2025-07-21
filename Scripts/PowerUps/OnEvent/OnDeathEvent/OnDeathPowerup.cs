
public abstract class OnDeathPowerup : WeaponPowerup
{ 
    protected override void Awake() {
        base.Awake();
        PowerupManager.OnEnemyKilled += OnDeath;
    }
    void OnDestroy() {
        PowerupManager.OnEnemyKilled -= OnDeath;
    }
    public abstract void OnDeath(Fighter fighter);
}
