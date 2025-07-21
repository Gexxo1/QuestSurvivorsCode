using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData")]

public class WaveData : ScriptableObject
{
    [Header("Wave Init - Must be set")]
    public Enemy[] mainWaveEnemies;
    public Enemy[] specialWaveEnemies;
    public SwarmEnemy swarmEnemy;
    public Enemy bossEnemy;
    public Breakable pot;
    
    [Header("Wave Settings")]
    public int waveEndTime = 600; // in seconds
    public int specEnemySpawnBegin = 300; // in seconds
    public int swarmSpawnNumber = 6;
    public int enemiesPerSwarmNumber = 6;
    public int spawnPotTimes = 5; //number of times pot spawns in waveEndTime
    [Range(0,100)] public int specEnemySpawnChance = 5;
    [Header("BossWave - Touch following settings only if isBossWave is true")]
    public bool isBossWave = false;
    public int bossWaveTick = 5; // boss spawn timer in seconds
    public int numberBossWaveEnemies = 100;

    [Header("Test Settings")]
    public bool isTestMap = false;
    public bool disableSwarm = false;
    public bool skipToBoss = false;
}
