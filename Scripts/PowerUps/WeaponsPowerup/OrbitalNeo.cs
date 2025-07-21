using System.Collections.Generic;
using UnityEngine;

public class OrbitalNeo : WeaponPowerup
{
    [SerializeField] private OrbitalChildNeo orbitalPrefab;
    [SerializeField] List<OrbitalChildNeo> orbitals;
    private new OrbitalStats stats => (OrbitalStats)base.stats;
    private new OrbitalStats statsUpgrade => (OrbitalStats)base.statsUpgrade;
    [SerializeField] private float baseOrbitRadiusAdd;
    //[SerializeField] private float orbitSpeedIncrease;
    [SerializeField] private float orbitRadiusMultiplier = 1f;
    protected override void Awake() {
        base.Awake();
        if(originalStats is not OrbitalStats)
            Debug.LogError("Serialize Error --> OrbitalNeo: originalStats is not an OrbitalStats");
        
        Transform parent = transform.GetChild(0);
        for(int i=0; i < 5; i++) {
            OrbitalChildNeo orbital = Instantiate(orbitalPrefab,parent);
            orbital.name = "Orbital " + i;
            orbitals.Add(orbital);
        }
        
        foreach (OrbitalChildNeo o in orbitals)
            o.gameObject.SetActive(false);
        UpdateOrbitalsNumber();
    }
    private const int degreeMultiplier = 180;
    
    private void Update() {
        Orbit();
    }
    private void Orbit() {
        for (int i = 0; i < stats.projectileNumber; i++) {
            float anglePerOrbital = 2 * Mathf.PI / stats.projectileNumber; // angle in radians
            float startingAngle = i * anglePerOrbital + anglePerOrbital / 2;
            float angle = Time.time * stats.projectileSpeed + startingAngle;
            //Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * stats.projectileNumber * orbitRadiusMultiplier;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * (baseOrbitRadiusAdd + stats.projectileNumber * orbitRadiusMultiplier);
            //orbit around the player
            orbitals[i].transform.position = (Vector2)player.transform.position + offset;
            //rotate 360 degrees per second
            orbitals[i].transform.Rotate(Vector3.forward, degreeMultiplier * stats.projectileSpeed * Time.deltaTime);
        }
    }
    public override void onUpgrade() {
        //note: i can't call base.onupgrade because it would execute the "basestats" addstats
        stats.AddToStats(Instantiate(statsUpgrade));
        UpdateOrbitalsNumber();
    }

    private void UpdateOrbitalsNumber() {
        for (int i = 0; i < stats.projectileNumber; i++) {
            orbitals[i].UpdateStats(stats, this);
            orbitals[i].gameObject.SetActive(true);
        }
    }
    public override string GetUpgrDesc() {
        return statsUpgrade.GetDescription();
    }
    public override void AddToStats(WeaponStats statToAdd) {
        base.AddToStats(statToAdd);
        UpdateOrbitalsNumber();
    }
}