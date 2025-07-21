using UnityEngine;
public abstract class WeaponPowerup : PowerUp
{
    [Header("Weapon Based Powerup")]
    [SerializeField] protected BaseWeaponStats originalStats; //stat upgrade WHEN tier 1
    protected BaseWeaponStats stats;
    [SerializeField] protected BaseWeaponStats statsUpgrade; //stat upgrade AFTER tier 1
    protected override void OnEnable() { 
        base.OnEnable(); 
        InstantiateStatsIfNull(); 
    }
    protected override void Awake() {
        base.Awake();
        InstantiateStatsIfNull();
    }
    public override void onUpgrade() {
        InstantiateStatsIfNull();
        AddToStats();
    }
    public override string GetUpgrDesc() {
        return  statsUpgrade.GetDescription();
    }

    public BaseWeaponStats GetWeaponStats() {
        InstantiateStatsIfNull();
        return stats;
    }

    protected virtual void InstantiateStatsIfNull() {
        //Debug.Log("InstantiateStatsIfNull --> [" + originalStats + "] [" + stats + "]");
        if(originalStats != null && stats == null) {
            stats = Instantiate(originalStats);
            AddGlobalWpnStatsToStats();
//            Debug.Log(stats);
        }
    }
    protected virtual void AddGlobalWpnStatsToStats() {
        stats.AddToStats(GameManager.instance.player.globalWpnStats);
    }
    protected virtual void AddToStats() {
        if(stats != null) 
            stats.AddToStats(statsUpgrade);
        else
            Debug.LogWarning("Stats are null, add to stats failed");
    }
    public virtual void AddToStats(WeaponStats statToAdd) {
        if(stats != null && statToAdd != null) 
            stats.AddToStats(statToAdd);
        else
            Debug.LogWarning("Stats or parameters are null, add to stats failed");
    }
}
