using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "OffGameUpgrade", menuName = "ScriptableObjects/OffGameUpgrade")]
public class OffGameUpgrades : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private Sprite img;
    [SerializeField] private StatsUp statUpgrade;
    [SerializeField] private int[] cost;
    private int tier = 0;
    public string GetTitle() { return title; }
    public Stats GetStat() { return statUpgrade; }
    public string GetName() { return title; }
    public int GetCost() { return cost[tier]; }
    public int GetCostSum() {
        int sum = 0;
        for(int i=0; i < tier; i++)
            sum += cost[i];
        return sum;
    }
    public Sprite GetIcon() { return img; }
    public int GetTier() { return tier; }
    public bool IsMaxTier() { return tier == GetMaxTier(); }
    public int GetMaxTier() { return cost.Length; }
    public void Upgrade() { tier++; }
    public void SetTier(int t) { tier = t; } 
    public string GetUpgradeDescription() { return statUpgrade.GetAutoDescription(); }
    public StatsUp GetStatUpgrade() { return statUpgrade; }
}
