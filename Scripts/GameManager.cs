using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;
    [Header("Various Managers")]
    [SerializeField] private FloatingTextManager floatingTextManager;
    [SerializeField] private MenuManager menuManager;
    //Manager passati dal Main Menu
    private DataPersistenceManager dataManager;
    private WaveManager waveManager;
    private SettingsManager settingsManager;
    private CharacterClassManager classManager;
    private ObjectiveManager objectiveManager;
    private MapManager mapManager;
    private PowerupTableManager puTableManager;
    private PowerupManager puManager;
    [Header("Internal Gamemanager variables")]
    [SerializeField] private GameObject AllUI;
    [SerializeField] public bool isDebugModeOn = false;
    [Header("Ingame Settings Manager Initialization")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Text masterPercentText;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Text bgmPercentText;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Text sfxPercentText;
    [SerializeField] private Toggle fullscreenCheckmark;
    [SerializeField] private Toggle floatingTextCheckmark;
    [SerializeField] private Toggle enemyHealthBarCheckmark;
    private void Awake() {
        if(instance != null) {
            Debug.Log("Gamemanager ha un'istanza duplicata: " + instance);
            return;
        }
        instance = this;
    }
    private void Start() {
        ManagersInit();
        DataManagersInit();
        PowerupTableManagerInit();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetUpUI();
        isTestModeOn = TestManager.instance.IsTestModeOn;
    }
    
    private void Update() {
        UpdateInGameTimer();
//        Debug.Log(isTestModeOn);
        if(isTestModeOn) {
            ShowHideUI();
            SpeedUpGameTime();
            ActivateGodMode();
            BlockExperienceGain();
        }
    }
    #region cheats
    bool isTestModeOn;
    private void ShowHideUI() {
        if(Input.GetKeyDown(KeyCode.H))
            if(AllUI.activeInHierarchy)
                AllUI.SetActive(false);
            else
                AllUI.SetActive(true);
    }

    private void SpeedUpGameTime() {
        //if(!isDebugModeOn) return;
        if(Input.GetKeyDown(KeyCode.F1)) {
            IncreaseTimeScale();
            Debug.Log("Game speed up");
        }
        if(Input.GetKeyDown(KeyCode.F2)) {
            SetTimeScale(1);
            Debug.Log("Game speed normal");
        }
    }
    bool isGodModeOn = false;
    private void ActivateGodMode() {
        if(Input.GetKeyDown(KeyCode.F3)) {
            isGodModeOn = !isGodModeOn;
            player.ActivateGodMode(isGodModeOn);
            Debug.Log("God Mode: " + isGodModeOn);
        }
    }
    bool isNoXpOn = false;
    private void BlockExperienceGain() {
        if(Input.GetKeyDown(KeyCode.F4)) {
            isNoXpOn = !isNoXpOn;
            player.BlockXpGain(isNoXpOn);
            Debug.Log("No Xp Mode: " + isNoXpOn);
        }
    }
    #endregion
    private float currTimeScale = 1f;
    public void SetTimeScale(float value) {
        currTimeScale = value;
        Time.timeScale = value;
    }
    public void IncreaseTimeScale() {
        currTimeScale++;
        Time.timeScale = currTimeScale;
    }
    public void ResetTimeScale() {
        Time.timeScale = currTimeScale;
    }
    private void DataManagersInit() {
        GameObject dataManagerGO = GameObject.Find("DataPersistenceManager");
        if(dataManagerGO != null) {
            dataManager = dataManagerGO.GetComponent<DataPersistenceManager>();
            dataManager.LoadGameSetup();
        }
        else if(isDebugModeOn)
            Debug.Log("No data manager found in current scene. No file loaded");
        GameObject settingsManagerGO = GameObject.Find("SettingsManager");
        if(settingsManagerGO != null) {
            settingsManager = settingsManagerGO.GetComponent<SettingsManager>();
            settingsManager.SetupSettingsManager(resolutionDropdown,bgmSlider,sfxSlider,masterSlider,bgmPercentText,sfxPercentText,masterPercentText,fullscreenCheckmark,floatingTextCheckmark,enemyHealthBarCheckmark);
        }
        else if(isDebugModeOn)
            Debug.Log("No settings manager found in current scene.");
    }
    private void ManagersInit() {
        GameObject waveManagerGO = GameObject.Find("WaveManager");
        GameObject classManagerGO = GameObject.Find("ClassManager");
        GameObject objectiveManagerGO = GameObject.Find("ObjectiveManager");
        GameObject mapManagerGo = GameObject.Find("MapManager");
        GameObject powerupTableManagerGO = GameObject.Find("PowerupTableManager");
        GameObject puManagerGO = GameObject.Find("PowerupManager");


        if(waveManagerGO != null) 
            waveManager = waveManagerGO.GetComponent<WaveManager>();
        else if(isDebugModeOn)
            Debug.Log("No wave manager found in current scene.");

        if(classManagerGO != null) {
            classManager = classManagerGO.GetComponent<CharacterClassManager>();
            classManager.UpdatePlayerClass(player); 
        }
        else if(isDebugModeOn)
            Debug.Log("No class manager found in current scene.");
        
        if(objectiveManagerGO != null) 
            objectiveManager = objectiveManagerGO.GetComponent<ObjectiveManager>();
        else if(isDebugModeOn)
            Debug.Log("No class manager found in current scene. Player in the scene is a test player.");

        if(mapManagerGo != null) 
            mapManager = mapManagerGo.GetComponent<MapManager>();
        else if(isDebugModeOn)
            Debug.Log("No map manager found in current scene. No file loaded");
        
        if(powerupTableManagerGO != null) {
            puTableManager = powerupTableManagerGO.GetComponent<PowerupTableManager>();
            puTableManager.AddExclusiveToAvailablePowerups(inv);
        }
        else if(isDebugModeOn)
            Debug.Log("No powerup table manager found in current scene. No file loaded");

        if(puManagerGO != null)
            puManager = puManagerGO.GetComponent<PowerupManager>();
        else if(isDebugModeOn)
            Debug.Log("No powerup manager found in current scene.");
    }
    private void PowerupTableManagerInit() {
        if(puTableManager != null) 
            puTableManager.SetupAllPowerupsTier();
        else if(isDebugModeOn)
            Debug.LogWarning("No powerup table manager found in current scene.");
    }
    [Header("Scene")]
    private int sceneIndex = 0; 
    public string[] scenes;
    //References
    [Header("Player Ref")]
    public Player player;
    public Inventory inv;
    public int dataGold; //total gold out of the game
    [Header("Per-Run Data")]
    public int runGoldCounter = 0; //gold collected in the current run
    public int runPotBreakCounter = 0;
    public int inGameKills;
    public float inGameTimer;
    [HideInInspector] public int totalManaUsed = 0;
    [HideInInspector] public int totalProjsShot = 0;
    //note: off-game data is located in ObjectiveManager
    [SerializeField] private GameObject UI;
    private Transform checkpoint;
    //Metodi interni
    private void SetUpUI() {
        //hitpointText.text = player.hitpoint + "/" + player.maxHitpoint;
        OnHitpointChange();
        OnManapointChange();
        OnExperienceChange();
        menuManager.UpdateCoinHUD(runGoldCounter);
    }
    //Scena
    public void LoadNextScene() {
        sceneIndex++;
        SceneManager.LoadScene(scenes[sceneIndex]);
//        Debug.Log("Scena successiva chiamata '" + scenes[sceneIndex] + "' caricata con successo!");
    }
    public void LoadPreviousScene() {
        sceneIndex++;
        SceneManager.LoadScene(scenes[sceneIndex]);
        Debug.Log("Scena successiva chiamata '" + scenes[sceneIndex] + "' caricata con successo!");
    }
    public void LoadSceneByIndex(int xs) {
        if(xs >= sceneIndex) {
            SceneManager.LoadScene(scenes[xs]);
            Debug.Log("Scena specifica chiamata '" + scenes[sceneIndex] + "' caricata con successo!");
        } 
        else 
            Debug.Log("La scena specifica numero '" + xs + "' va oltre l'indice");
    }
    public string GetCurrentSceneName() {
        return SceneManager.GetActiveScene().name;
    }
    public bool isCurrentScene(string name) {
        if(GetCurrentSceneName() == name)
            return true;
        return false;
    }
    //Floating Text Manager
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }
    public void ShowText(string msg, Vector3 position, bool motion = false) { //used for debug
        Vector3 motionVector = Vector3.one;
        if(motion) motionVector = Vector3.up * 30;
        floatingTextManager.Show(msg, 18, Color.red, position, motionVector, 1);
    }
    public void ShowText(string msg, Vector3 position, Vector3 motion) { //used for debug
        floatingTextManager.Show(msg, 18, Color.red, position, Vector3.up * 30, 1);
    }
    public void GrantXp(int givenXP) {
        player.GrantXp(givenXP);
    }
    //Ui change
    public void OnExperienceChange() { 
        float xpRatio = (float)player.exp / (float)player.expTable[player.currLvl];
        string barTxt = player.exp + "/" + player.expTable[player.currLvl];
        menuManager.UpdateXpHUD(xpRatio,barTxt);
    } 
    //warning: not setting this parameter when addtostats is called may cause infinite loop
    public void OnHitpointChange(bool triggerEvents = true) { 
        //è necessario eseguire questo metodo OGNI VOLTA che modifichiamo gli hp, altrimenti non verrebbe mostrato nella barra
        if(player.currHP > player.stats.healthpoint)
            player.currHP = player.stats.healthpoint;
        float hpRatio = player.currHP / (float)player.stats.healthpoint;
        string hpTxt = player.currHP + "/" + player.stats.healthpoint;
        menuManager.UpdateHpHUD(hpRatio,hpTxt);
        
        if(player.currArmor > player.stats.armor)
            player.currArmor = player.stats.armor;
        float armorRatio = player.currArmor / (float)player.stats.armor;

        bool isArmorActive = player.stats.armor > 0;
        if(!isArmorActive) armorRatio = 0;
        
        string armorTxt = player.currArmor + "/" + player.stats.armor;
        menuManager.UpdateArmorHUD(armorRatio,armorTxt);
        menuManager.SetActiveArmorHUD(isArmorActive);

        if(triggerEvents) puManager.PlayerResouceTresholdEvent();
    }
    public void OnManapointChange(bool triggerEvents = true) { 
        if(player.currMp > player.stats.manapoint)
            player.currMp = player.stats.manapoint;
        
        float mpRatio = player.currMp / (float)player.stats.manapoint;
        string mpTxt = player.currMp + "/" + player.stats.manapoint;
        menuManager.UpdateMpHUD(mpRatio,mpTxt);
        if(triggerEvents) puManager.PlayerResouceTresholdEvent();
    }
    public void OnManaUsed(int manaUsed) {
        totalManaUsed += manaUsed;
        objectiveManager.ManaUsageObjectiveCheck(totalManaUsed);
    }
    public void OnCoinGet(int goldAmount, bool showText, Vector2 coinPos) { 
        runGoldCounter += goldAmount;
        menuManager.UpdateCoinHUD(runGoldCounter);
        if(showText) {
            Vector2 randomCoordinate = genRandCoord(coinPos - coinPos/2, coinPos + coinPos/2);
            ShowText("+" + goldAmount + " gold", 20, Color.yellow, randomCoordinate, Vector3.up * 50, 1.5f);
        }
        if(objectiveManager != null)
            objectiveManager.GoldObjectiveCheck(runGoldCounter);
    }
    public void IncreasePotBreakCounter(int value) { 
        runPotBreakCounter += value;
        if(objectiveManager != null)
            objectiveManager.PotObjectiveCheck(runPotBreakCounter);
    }
    public void IncreaseProjectileShotCounter(int value = 1) {
        totalProjsShot += value;
        if(objectiveManager != null)
            objectiveManager.ProjShotObjectiveCheck(totalProjsShot);
    }

    //Utilities
    private Vector2 genRandCoord(Vector2 min, Vector2 max) {
        return new Vector2(Random.Range(min.x,max.x),Random.Range(min.y,max.y)+1);
    }
    public void OnSceneLoaded(Scene s, LoadSceneMode mode) {
        if(player == null)
            return;
        if(checkpoint != null) 
            player.transform.position = checkpoint.position;
        else if(s.name != "MainMenu")
            player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        else
            Debug.LogWarning("OnSceneLoaded: Cannot find a spawn point");
    }
    public void loadScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void returnToMainMenu() {
        SaveGame();
        loadScene("MainMenu");
        deleteAllDontDestroy();
    }
    public void deleteAllDontDestroy() {
        for(int i=0; i < FindObjectsOfType<DontDestroy>().Length; i++) {
            //Debug.Log(Object.FindObjectsOfType<DontDestroy>()[i].gameObject);
            Destroy(FindObjectsOfType<DontDestroy>()[i].gameObject);
        }
    }

    public void spawnCollectable(Collectable collectable, int quantity, Vector3 pos, Vector3 posRange) {
        for(int i=0; i < quantity; i++) {
            float xr = Random.Range(-posRange.x,posRange.x);
            float yr = Random.Range(-posRange.y,posRange.y);
            Vector2 randPos = pos + new Vector3(xr,yr,0);
            //Collectable item = Instantiate(go, randPos, Quaternion.identity);
            //item.gameObject.SetActive(true);
            ObjectPoolManager.SpawnObject(collectable.gameObject, randPos, Quaternion.identity, ObjectPoolManager.PoolType.Collectables);
        }
    }
    public void spawnCollectableRandomPos(Collectable collectable, int quantity, Vector3 pos) {
        Vector3 newPos;
        for(int i=0; i < quantity; i++) {
            float rX = Random.Range(0f,1f); float rY = Random.Range(0f,1f);
            newPos = new Vector3(pos.x + rX, pos.y + rY, 0);
            //Collectable item = Instantiate(go, newPos, Quaternion.identity);
            //item.gameObject.SetActive(true);
            ObjectPoolManager.SpawnObject(collectable.gameObject, newPos, Quaternion.identity, ObjectPoolManager.PoolType.Collectables);
        }
        
    }
    //Drop
    public void dropItem(Drop[] drop, Vector3 pos, Vector2 posRange) {
        if(drop == null) {
            Debug.LogWarning("Drop list is null");
            return;
        }
        for(int i=0; i < drop.Length; i++) {
            //random chance
            int rand = Random.Range(0,101);
            int dropPercent = drop[i].percent;
            if(drop[i].item is Coin) 
                dropPercent += player.stats.luckPercent;
            if(dropPercent >= rand) 
                spawnCollectable(drop[i].item,drop[i].getQuantity(),pos,posRange);
        }
    }

    public void dropOneItem(Drop[] drop, Vector3 pos, Vector2 posRange) { //Nota da aggiustare: dà priorità ai primi oggetti dell'array
        int rand = Random.Range(0,101);
        for(int i=0; i < drop.Length; i++) {
            if(drop[i].percent >= rand) {
                spawnCollectable(drop[i].item,drop[i].getQuantity(),pos,posRange);
                return;
            }
        }
    }

    public void dropOneGuaranteeItem(Drop[] drop, Vector3 pos, Vector2 posRange) { //Ignora i drop rate dei singoli
        int r = Random.Range(0,drop.Length);
        spawnCollectable(drop[r].item,drop[r].getQuantity(),pos,posRange);
    }

    public void UpdateKills(int number, GameObject deadEnemy, bool waveEnemy = true) {
        inGameKills += number;
        objectiveManager.KillsObjectiveCheck(number);
        menuManager.UpdateKillsUI(inGameKills);
        if(WaveManager.instance != null && waveEnemy)
            WaveManager.instance.RemoveEnemy(deadEnemy);
    }

    public void UpdateInGameTimer() {
        inGameTimer += Time.deltaTime;
        menuManager.UpdateTimer(inGameTimer);
    }
    //used in: status effects & thorns damage | WARNING: do not use for main damage calculation
    public Damage RawDamageCalculation(Fighter wielder, Vector3 targetPos, int rawDamage, DamageType t, Color32 col) {
        Damage dmg = new() {
            amount = rawDamage,
            originalAmount = rawDamage,
            origin = Vector3.zero,
            hitter = wielder,
            hitPoint = targetPos,
            type = t,
            textColor = col,
            playSound = false,
            //sourceItem = item,
        };

        return dmg;
    }
    
    public Damage DamageCalculation(Fighter wielder, Stats wielderStats, Vector3 wielderPos, Fighter hitFighter, BaseWeaponStats weaponStats, 
                                    Vector2 hitPoint, DamageType dmgtype, bool hasManaGain, List<StatusEffectData> statusEffects,
                                    HitSource hitSource) { 
        int randCrit = Random.Range(0,101);
        int randLifesteal = Random.Range(0,101);
        
        //int addDmg = 0; if(wielder is Player p) addDmg = p.currLvl;
        int fixedDmg; 
//        Debug.Log("Base Damage: " + weaponStats.baseDamage + " * " + wielderStats.GetFixedDamagePercent());
        fixedDmg = (int)(weaponStats.baseDamage * wielderStats.GetFixedDamagePercent());
        bool isCrit = false;
//        Debug.Log("Early Damage: " + fixedDmg);
        //Interval damage
        int min = (int)(fixedDmg * 0.9);
        int max = (int)(fixedDmg * 1.1);
        fixedDmg = Random.Range(min,max+1); //"max + 1" because is exclusve
        //Crit
        Color col = Color.white;
        if(wielderStats.critRatePercent > randCrit) {
            fixedDmg = (int)(fixedDmg * wielderStats.getFixedCritDamagePercent());
            isCrit = true;
            //col = Color.;
        }

        //Armor
        /*
        if(hitFighter != null) //target == null --> caso: pot
            if(hitFighter.stats.armor != 0) { //funziona anche con valore negativo di armatura (in quel caso al posto di subire meno ne prendi di più)
                fixedDmg -= hitFighter.stats.armor - wielderStats.reduceTargetArmor;
                if(fixedDmg <= 0)
                    fixedDmg = 1; //il danno non può andare sotto lo 0
            }
        */
        //Lifesteal
        int healingAmount = 0; 
        if(wielderStats.lifestealPercent > 0) { //check non necessario, ma utile per il debug
//            Debug.Log("healing rng: " + wielder + " " + wielderStats.lifestealPercent + " > " + randLifesteal);
            if(wielderStats.lifestealPercent > randLifesteal)
                healingAmount = fixedDmg;
        }
        Damage dmg = new() {
            amount = fixedDmg,
            originalAmount = weaponStats.baseDamage,
            knockback = weaponStats.knockback,
            statusEffects = statusEffects,
            origin = wielderPos,
            isCrit = isCrit,
            hitter = wielder,
            hitFighter = hitFighter,
            sourceHeal = healingAmount,
            hitPoint = hitPoint,
            type = dmgtype,
            playSound = dmgtype == DamageType.normal,
            textColor = col,
            hasManaGain = hasManaGain,
            hitSource = hitSource,
        };
        //Debug.Log("Danno inflitto: " + dmg.amount);
        return dmg;
    }
    public void HealPlayer(int amount) {
        player.Heal(amount);
    }
    public void RestorePlayerMana(int amount) {
        player.RestoreMana(amount);
    }

    //warning: utilized for results data, must be called after the game is over
    
    public ResultsData GetRunResultsData() {
        ResultsData data = new() {
            //shown data
            gold = runGoldCounter,
            totalKills = inGameKills,
            totalPlaytime = inGameTimer,
            
            //hidden data
            damageTaken = player.GetDamageTaken(),
            potsBroken = runPotBreakCounter,
            totalManaUsed = totalManaUsed,
            projectilesShot = totalProjsShot,
        };
        return data;
    }
    public void Quit() {
        SaveGame();
        Application.Quit();
    }

    //Data NON RICHIAMARE
    public void LoadData(GameData data) {
        dataGold = data.gold;
        runGoldCounter = 0;
        //classManager.SetClassesTier(data.classesTier);//classManager.isClassOwned = data.ownedClasses;

        objectiveManager.SetObjDone(data.objectives);
        //MapManager.instance.SetClearedList(data.mapCleared);
    }

    public void SaveData(ref GameData data) {
        dataGold += runGoldCounter;
        data.gold = dataGold;
        //data.ownedClasses = classManager.GetClassOwnedToList();//data.ownedClasses = classManager.isClassOwned;
        data.objectives = objectiveManager.GetObjDone();
        data.maps = mapManager.GetClearedMaps();
        data.classes = classManager.GetClassesTierToDictionary();

        //off game data
        data.totalGold += runGoldCounter;
        data.totalPlaytime += inGameTimer;
        data.totalKills += inGameKills;
    }

    //Incapsulazione Data
    public void SaveGame() {
        Debug.Log("--GameManager Save Game--");
        if(dataManager != null)
            dataManager.SaveGame();
        else
            Debug.LogWarning("Data Manager is null. Failed to Save Game");
    }
    public void LoadGame() {
        Debug.Log("--GameManager Load Game--");
        if(dataManager != null)
            dataManager.LoadGame();
        else
            Debug.LogWarning("Data Manager is null. Failed to Load Game");
    }
    
    //Used for settings in game by UI --> DONT REMOVE THIS --> IT IS REFERENCED IN EDITOR
     public void SaveSettings() {
        //Debug.Log("--GameManager Save Settings---");
        if(settingsManager != null)        
            settingsManager.SaveSettings();
        else
            Debug.LogWarning("Settings Manager is null. Failed to Save Settings");
    }
    public void LoadSettings() {
        //Debug.Log("--GameManager Load Settings--");
        if(settingsManager != null)
            settingsManager.LoadSettings();
        else
            Debug.LogWarning("Settings Manager is null. Failed to Load Settings");
    }
    public void SettingsMasterVolume(float value) {
        settingsManager.SetMasterVolume(value);
    }
    public void SettingsEffectsVolume(float value) {
        settingsManager.SetEffectsVolume(value);
    }
    public void SettingsMusicVolume(float value) {
        settingsManager.SetMusicVolume(value);
    }
    public void SettingsSetResolution(int resolutionIndex) {
        settingsManager.SetResolution(resolutionIndex);
    }
    public void SettingsSetFullscreen(bool isFs) {
        settingsManager.SetFullscreen(isFs);
    }
    public void SettingsSetFloatingText(bool flag) {
        settingsManager.SetFloatingText(flag);
    }
    public void SettingsSetEnemyHpBar(bool flag) {
        settingsManager.SetEnemyHpBar(flag);
    }
}
