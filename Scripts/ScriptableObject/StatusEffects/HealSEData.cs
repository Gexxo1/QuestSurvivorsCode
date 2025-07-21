using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "ScriptableObjects/StatusEffects/HealStatusEffectOnKill")]
public class HealSEData : StatusEffectData {
    //private const int healPercentage = 10;
    private const int healAmount = 1;
    public HealSEData(string name, float duration, int maxStacks, float tickRate, Color32 color) : base(name, duration, maxStacks, tickRate, color) { }
    public void HealPlayer() {
        Player p = GameManager.instance.player;
        //int healAmount = (int)((float)p.stats.healthpoint * healPercentage / 100);
        p.Heal(healAmount);
    }
}
