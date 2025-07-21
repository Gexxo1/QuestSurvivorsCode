using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Data - Must be set")]
    [SerializeField] private WaveData waveData;
    
    [Header("Serialized + Istanced Attributes")] //attributi che vengono serializzati ma anche scritti
    private const float startingSpawnCooldown = 1f;
    //[SerializeField] private float currentSpawnCd = 1f;
    [SerializeField] private float spawnRateBoost = 1f;
    [SerializeField] private bool isEndlessMode = false;
    private bool isTestMap = false;
    [SerializeField] private bool showEnemyHpBar = false;
    //[SerializeField] private List<Item> itemPickupsFound;
    [Header("---Same For Each Map---")]
    [Header("Inits")]
    public Camera mainCamera;
    //privates
    private GameObject spawnPoints;
    private int posCount;
    private GameObject[] pos;
    private float timer;
    //cambio wave
    private int currentEnemyIndex = 0; 
    private float waveSplit;
    private float swarmSplits;
    private bool isBossSpawned = false;
    private int splitsNumber;
    public static WaveManager instance;
    [Header("Enemy Instances (Serialized only for showing)")]
    [SerializeField] private List<GameObject> enemyInstances;
    private void Awake() {
        if(instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    private void Start() {
        timer = 0; 
        //bool onMenuEndlessChoice = MapManager.instance != null ? MapManager.instance.IsEndlessSelection() : false;
        isEndlessMode = MapManager.instance.IsEndlessSelection() || isEndlessMode;
        isTestMap = MapManager.instance.IsTestSelection() || waveData.isTestMap;
        if(MapManager.instance.GetCurrentWaveDataIndex() != null)
            waveData = MapManager.instance.GetCurrentWaveDataIndex();
        UpdateEnemiesHpFlag();

        spawnPoints = GameObject.Find("EnemiesSpawnPoints");
        if(spawnPoints == null) {
            Debug.Log("WARNING: Spawnpoints not set");
            return;
        }

        posCount = spawnPoints.transform.childCount;
        pos = new GameObject[posCount];

        for(int i = 0; i < posCount; i++) 
            pos[i] = spawnPoints.transform.GetChild(i).gameObject;

        MenuManager.instance.activateTimer(true);

        splitsNumber = waveData.mainWaveEnemies.Length*2;
        if(splitsNumber != 0)
            waveSplit = waveData.waveEndTime / splitsNumber;
        else
            waveSplit = waveData.waveEndTime;

//        Debug.Log("Wave Split: " + waveSplit + " = " + waveData.waveEndTime + " / " + splitsNumber);
        if(waveData.swarmSpawnNumber != 0)
            swarmSplits = waveData.waveEndTime / waveData.swarmSpawnNumber;
        else
            swarmSplits = waveData.waveEndTime;

        if(waveData.skipToBoss) {
            SpawnBoss();
            return;
        }
        StartCoroutine(TimerSpawn());
        StartCoroutine(TimerSplit());
        StartCoroutine(TimerEnd());
        if(!waveData.disableSwarm)
            StartCoroutine(TimerSwarm());
        StartCoroutine(TimerPot());
    }

    [HideInInspector] public bool waveComplete;
    private bool isBossDefeated; public void SetBossDefeated(bool flag) { isBossDefeated = flag; }
    private void Update()
    {
        timer += Time.deltaTime; //timer globale
        if(isBossSpawned) {
            if(AreAllEnemiesDead())
                if(isBossDefeated)
                    OnWaveComplete();
            //Utility.PrintList(enemyInstances);
        }
        //if(Input.GetKeyDown(KeyCode.H)) SetAllEnemiesHpBar(!showEnemyHpBar);
    }
    //Timers
    private const int enemyNumberCap = 200;
    private IEnumerator TimerSpawn() {
//        Debug.Log("Spawn Timer started. Timer every: " + startingSpawnCooldown + " / " +  spawnRateBoost + " = " + startingSpawnCooldown / spawnRateBoost);
        while(!isBossSpawned) {
            yield return new WaitForSeconds(startingSpawnCooldown / spawnRateBoost); //WARNING: SE METTESSI QUALCOSIASI COSA PRIMA DI QUESTO WAIT CRASHEREBBE TUTTO
            if(enemyInstances.Count > enemyNumberCap) 
                continue;
            
            SpawnEnemyOrRanged();
        }
        Debug.Log("Spawn Timer ended. End variable: " + isBossSpawned);
        yield return null;
    }
    private const float spawnIncrease = 0.75f; //prev: 0.5f
    private IEnumerator TimerSplit() {
        while(!isBossSpawned) {
            yield return new WaitForSeconds(waveSplit);
           
            if(currentEnemyIndex < splitsNumber-1)
                currentEnemyIndex++;
            spawnRateBoost += spawnIncrease * currentEnemyIndex;
            Debug.Log("Current Split: " + currentEnemyIndex + " --> enemies now spawn every " + startingSpawnCooldown / spawnRateBoost + " seconds");
        }
        Debug.Log("Split Timer ended. End variable: " + isBossSpawned);
        yield return null;
    }
    private IEnumerator TimerEnd() {
//        Debug.Log("puzzolo");
        while(!isBossSpawned) {
            yield return new WaitForSeconds(waveData.waveEndTime);
            Debug.Log("End of Wave timer");
            SpawnBoss();
        }
        yield return null;
    }
    private IEnumerator TimerSwarm() {
        while(!isBossSpawned) {
            yield return new WaitForSeconds(swarmSplits);
            SpawnSwarm(waveData.enemiesPerSwarmNumber + currentEnemyIndex, waveData.enemiesPerSwarmNumber + currentEnemyIndex);
            //swarmEnemy.IncreaseSpeed(0.5f);
        }
        Debug.Log("Swarm Timer ended. End variable: " + isBossSpawned);
        yield return null;
    }
    private int waveBossesSpawned = 0;
    private IEnumerator TimerBossWave() {
        //Debug.Log("Boss Wave Timer started for " + waveBossesSpawned + " < " + waveData.numberBossWaveEnemies);
        while(true) {
            yield return new WaitForSeconds(waveData.bossWaveTick);
            if(waveBossesSpawned >= waveData.numberBossWaveEnemies) {
                Debug.Log("Boss Wave Timer ended.");
                yield break;
            }
            InstantiateEnemy(waveData.bossEnemy, GetRandomPointOutsideCamera(), true);
            waveBossesSpawned++;
            //Debug.Log("Wave Boss spawned n°"+waveBossesSpawned);
        }
    }
    
    private IEnumerator TimerPot() {
        while(!isBossSpawned) {
            yield return new WaitForSeconds(waveSplit/waveData.spawnPotTimes);
            SpawnPot();
        }
        Debug.Log("Pot Timer ended. End variable: " + isBossSpawned);
        yield return null;
    }
    //Spawn
    public void SpawnEnemyOrRanged() {
        if(timer >= waveData.specEnemySpawnBegin && (waveData.specialWaveEnemies.Length != 0)) {
            int randChance = Random.Range(0,101);
            if(waveData.specEnemySpawnChance >= randChance) {
                SpawnRanged();
                return;
            }
        }
        SpawnEnemy();
    }
    public void SpawnEnemy() {
        if(waveData.mainWaveEnemies.Length == 0) {
            Debug.LogWarning("Main Wave Data Null: No enemies to spawn");
            return;
        }

        Vector2 pos = GetRandomPointOutsideCamera();
        int index = GetRandEnemyIndex(currentEnemyIndex, waveData.mainWaveEnemies.Length);
        InstantiateEnemy(waveData.mainWaveEnemies[index], pos);
    }
    public void SpawnRanged() {
        if(waveData.specialWaveEnemies.Length == 0) 
            return;
        
        Vector2 pos = GetRandomPointOutsideCamera();
        int mapIndex = currentEnemyIndex % waveData.specialWaveEnemies.Length;
        int index = GetRandEnemyIndex(mapIndex, waveData.specialWaveEnemies.Length);
        InstantiateEnemy(waveData.specialWaveEnemies[index], pos);
    }
    private int GetRandEnemyIndex(int baseIndex, int maxIndex) {
        //es.: 2.5 --> floor = 2, ceiling = 3 --> the random index will be 2 or 3
        float currIndex = (float)baseIndex/2;
        int floor = Mathf.FloorToInt(currIndex);
        int ceiling = Mathf.CeilToInt(currIndex);

        int index = Random.Range(floor, ceiling+1);
        if(index >= maxIndex)
            index = maxIndex-1;
        return index;
    }
    
    private int bossesNoToSpawn = 0;
    
    private void SpawnBoss() {
        bossesNoToSpawn++;

        if(!isEndlessMode)
            isBossSpawned = true;
        else timer = 0;
    
        if(!waveData.isBossWave) {  //if it's a single boss
            for(int i=0; i < bossesNoToSpawn; i++) {
                Vector2 pos = GetRandomPointOutsideCamera();
                InstantiateEnemy(waveData.bossEnemy, pos);
            }
        }
        else  //if it's a boss wave
            StartCoroutine(TimerBossWave());

        Debug.Log("Boss spawned n°"+bossesNoToSpawn);
    }
    private void SpawnSwarm(int rows, int columns) {
        Vector2 pos = GetRandomPointOutsideCamera();
        SwarmEnemy[,] s = new SwarmEnemy[rows,columns];
        for(int i=0; i < rows; i++) 
            for(int j=0; j < columns; j++) 
                s[i,j] = waveData.swarmEnemy;
        
        for(int i=0; i < rows; i++) {
            for(int j=0; j < columns; j++) {
                Vector2 offset = new(i*0.75f,j*-0.5f);
                Vector2 newPos = pos + offset;
                InstantiateEnemy(s[i,j], newPos);
            }
        }
    }
    private void SpawnPot() {
        Vector2 pos = GetRandomPointOutsideCamera();
        ObjectPoolManager.SpawnObject(waveData.pot.gameObject, pos, Quaternion.identity, ObjectPoolManager.PoolType.Collectables);
    }
    //End checks
    private void InstantiateEnemy(Enemy enemy, Vector3 pos, bool isBoss = false) {
        enemyInstances.Add(ObjectPoolManager.SpawnObject(enemy.gameObject, pos, Quaternion.identity, ObjectPoolManager.PoolType.Enemy));
        int currIndex = enemyInstances.Count-1;
        if(isBoss && !isEndlessMode && enemyInstances[currIndex].TryGetComponent(out BossEnemy boss)) 
            boss.SetDropsList(null);
        
        if(!showEnemyHpBar) {
            Enemy e = enemyInstances[currIndex].GetComponent<Enemy>();
            e.SetHpBarActive(false);
        }
    }
    private void SetAllEnemiesHpBar(bool flag) {
        showEnemyHpBar = flag;
        foreach(GameObject enemy in enemyInstances) {
            Enemy e = enemy.GetComponent<Enemy>();
            e.SetHpBarActive(flag);
        }
    }
    public void UpdateEnemiesHpFlag() {
        showEnemyHpBar = PlayerPrefs.GetInt("EnemyHpBar",1) == 1;
        SetAllEnemiesHpBar(showEnemyHpBar);
    }
    public void RemoveEnemy(GameObject enemy) {
        bool isRemoved = enemyInstances.Remove(enemy);
        if(!isRemoved) 
            Debug.LogWarning("Enemy " + enemy + " was not removed from the list");
            
    }
    private bool AreAllEnemiesDead() {
        return enemyInstances.Count == 0;
    }
    private void OnWaveComplete() {
        if(waveComplete)
            return;
        waveComplete = true;
        if(!isTestMap) {
            ObjectiveManager.instance.OnMapCompleteObjectives();
            MapManager.instance.MapCleared();
            ObjectiveManager.instance.AddCompletedRuns();
            GameManager.instance.SaveGame();
        }
        //if(!isFinalMap) 
        MenuManager.instance.ShowWavecompleteMenu(true);
        //else
        //MenuManager.instance.ShowGamecompleteMenu(true);
    }

    /*
    * breve delay per aspettare che i nemici vengano prima inizializzati correttamente 
    * (uccidere nemici all'esatto momento dello spawn causava crash)
    */
    public IEnumerator ActivateDelay(GameObject e) {
        e.SetActive(false);
        yield return new WaitForSeconds(1);
        e.SetActive(true);
        yield return null;
    }
    Vector2 GetRandomPointOutsideCamera() {
        Vector2 randomPoint = Vector2.zero;
        bool pointOutsideCamera = false;

        while (!pointOutsideCamera) {
            // Genera un punto casuale all'interno del raggio specificato
            randomPoint = Random.insideUnitCircle * 9f;
            // Converte le coordinate del mondo in coordinate della telecamera
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(randomPoint);
            // Controlla se il punto è all'interno del campo visivo della telecamera
            if (screenPoint.x < 0f || screenPoint.x > 1f || screenPoint.y < 0f || screenPoint.y > 1f) 
                pointOutsideCamera = true;
//            Debug.Log("Random Point [" + randomPoint + "] Screen Point [" + screenPoint + "] Is Outside Camera " + pointOutsideCamera);
        }
        //GameManager.instance.ShowText("x",randomPoint);
        return randomPoint;
    }

    public WaveData GetWaveData() { return waveData; }
}
