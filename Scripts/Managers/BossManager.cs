using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance { get; private set; }
    [SerializeField] string bossName = "not set"; //serialization ovverrides base enemy name
    float currHP, maxHP; 
    bool isMultipleBoss; //se un boss sono più nemici
    int bossNumber; //conta i boss 
    int bossesRemaining;
    private void Awake() {
        SingletonCheck();
    }
    private void Start() {
        isMultipleBoss = WaveManager.instance.GetWaveData().isBossWave;
        bossNumber = WaveManager.instance.GetWaveData().numberBossWaveEnemies;
        bossesRemaining = bossNumber;
    }
    bool init = false;
    public void OnBossSpawn(float mHP, string name) {
        if(init) return;
        if(bossName == "not set") bossName = name;

        Debug.Log("Boss spawned: " + bossName + " | " + mHP);

        if(!isMultipleBoss) maxHP = mHP;
        else maxHP = mHP * bossNumber;

        currHP = maxHP;
        bossName = name;

        MenuManager.instance.ActivateBossHpBar(true, maxHP, bossName);
//        Debug.Log("Boss spawned: currhp --> " + currHP);
        init = true;
    }
    public void UpdateBossHP(int personalCurrHP, int personalMaxHP, int damage) {
        if(!isMultipleBoss) {
            currHP = personalCurrHP;
            maxHP = personalMaxHP;
        }
        else {
            //float damage = personalMaxHP - personalCurrHP;
            //Debug.Log("Boss HP: " + currHP + " | Total Max Hp: " + maxHP + " | Damage: " + damage + " | Personal Max Hp: " + personalMaxHP);
            //se per esempio si subiscono 12 danni avendo 10 hp si tolgono 10 hp non 12
            damage = Mathf.Min(damage, personalMaxHP); 
//            Debug.Log("#1 --> " + damage);
            //se per esempio si subiscono 10 danni avendo 7 hp rimanenti si tolgono 7 hp non 10
            //damage = Mathf.Min(damage, personalCurrHP);
            //Debug.Log("#2 --> " + damage);
            currHP = Mathf.Max(currHP - damage, 0);
//            Debug.Log("Final: " + currHP);
            //currHP -= damage;
        }
        MenuManager.instance.UpdateBossHpHUD(currHP, maxHP);
    }
    public void OnBossDeath() {
//        Debug.Log("Boss dead | " + isMultipleBoss + " | " + bossNumber + " --> " + bossesRemaining);
        if (isMultipleBoss) {
            bossesRemaining--;
            if (bossesRemaining <= 0) 
                End();
        } else 
            End();
    }

    private void End() {
        MenuManager.instance.ActivateBossHpBar(false);
        WaveManager.instance.SetBossDefeated(true);
    }

    private void SingletonCheck() {
        if (instance == null) 
            instance = this;
        else Destroy(gameObject);
    }
}
