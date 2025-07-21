using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour, IDataPersistence
{
    public Objective[] objectives;
    public bool[] conditionsMet; //deve essere setuppato manualmente (inizialmente era incluso in objective, ma gli scriptable objects non erano ideali poichè salvavano pure oltre il runtime "conditions met")
    public static ObjectiveManager instance;
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Debug.LogError("Found another class manager in scene");
        conditionsMet = new bool[objectives.Length];
        CheckIfThereArentDupes();
        SortObjectives3();
    }
    private void CheckIfThereArentDupes() {
        for(int i=0; i < objectives.Length; i++) {
            for(int j=0; j < objectives.Length; j++) {
                if(i != j && objectives[i].GetId() == objectives[j].GetId())
                    Debug.LogWarning("Found duplicate objective id: " + objectives[i].GetId());
            }
        }
    }
    /*
    public void AchieveObjectiveById(string id) {
        for(int i=0; i < objectives.Length; i++) {
            if(objectives[i].GetId() == id) {
                if(!conditionsMet[i]) {
                    conditionsMet[i] = true;
                    MenuManager.instance.ShowObjectiveCompleted(i);
                    if(objectives[i].unlockedClass != null)
                        CharacterClassManager.instance.UnlockClassById(objectives[i].unlockedClass.getId());
                }
                return;
            }
        }
        
        Debug.LogWarning("Following objective id put in parameter not found: " + id);
    }
    */
    public void AchieveObjectiveByIndex(int index) {
        if(index >= objectives.Length) {
            Debug.LogWarning("L'index [" + index + "] goes beyond the objectives array length [" + objectives.Length + "]");
            return;
        }
        if(!conditionsMet[index]) {
            conditionsMet[index] = true;
            if(MenuManager.instance != null)
                MenuManager.instance.ShowObjectiveCompleted(index);
            if(objectives[index].unlockedClass != null)
                CharacterClassManager.instance.UnlockClassById(objectives[index].unlockedClass.getId());
        }
        
    }
    
    //Objectives check
    public void OnMapCompleteObjectives() {
        MapObjectiveCheck();
        HitObjectiveCheck();
        ManaUsageObjectiveCheck(GameManager.instance.totalManaUsed);
    }
    public void UnlockAll() {
        for(int i=0; i < objectives.Length; i++) 
            AchieveObjectiveByIndex(i);
    }
    private void MapObjectiveCheck() {
        for(int i=0; i < objectives.Length; i++) {
            if (objectives[i] is MapComplObjective lvlobj) {
                if (lvlobj.IsConditionTrue(0))
                    AchieveObjectiveByIndex(i);
            }
        }
    }
    public void KillsObjectiveCheck(int killsCounter) { 
        for(int i=0; i < objectives.Length; i++) {
            if (objectives[i] is KillsObjective lvlobj) {
                if (lvlobj.IsConditionTrue(killsCounter))
                    AchieveObjectiveByIndex(i);
            }
        }
    }
    public void GoldObjectiveCheck(int currGold) {
        for(int i=0; i < objectives.Length; i++) {
            if (objectives[i] is GoldObjective lvlobj) {
                if (lvlobj.IsConditionTrue(currGold))
                    AchieveObjectiveByIndex(i);
            }
        }
    }
    public void PotObjectiveCheck(int potsBroken) {
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is PotObjective lvlobj) 
                if (lvlobj.IsConditionTrue(potsBroken))
                    AchieveObjectiveByIndex(i);
    }
    public void ProjShotObjectiveCheck(int projShot) {
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is ProjShotObjective lvlobj) 
                if (lvlobj.IsConditionTrue(projShot))
                    AchieveObjectiveByIndex(i);
    }
    public void PlayerLevelObjectiveCheck(int currLvl) {
        for(int i=0; i < objectives.Length; i++) {
            if (objectives[i] is LevelObjective lvlobj) {
                if (lvlobj.IsConditionTrue(currLvl))
                    AchieveObjectiveByIndex(i);
            }
        }
    }
    public void StatsObjectiveCheck(Stats stats) {
        for(int i=0; i < objectives.Length; i++) {
            if (objectives[i] is StatsObjective statObj) {
                if (stats.AreStatsGreaterOrEqual(statObj.statTreshold))
                    AchieveObjectiveByIndex(i);
            }
        }
    }
    public void GottenPowerupsObjectiveCheck(List<PowerUp> inventoryPu) {
        for(int i=0; i < objectives.Length; i++) {
            if (objectives[i] is PowerupGetObjective lvlobj) {
                int count = 0;
                foreach(PowerUp invPu in inventoryPu) {
                    foreach(PowerUp neededPu in lvlobj.neededPowerups) 
                        if (invPu.title == neededPu.title)
                            count++;
                    if (lvlobj.IsConditionTrue(count))
                        AchieveObjectiveByIndex(i);
                }
            }
        }
    }

    private void HitObjectiveCheck() {
        int dmgTaken = GameManager.instance.player.GetDamageTaken();
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is HitObjective hitobj) 
                if (hitobj.IsConditionTrue(dmgTaken))
                    AchieveObjectiveByIndex(i);
    }

    public void ManaUsageObjectiveCheck(int totalManaUsed) {
        //Debug.Log("Current Mana Usage: [" + totalManaUsed + "]");
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is ManaUsageObjective hitobj) 
                if (hitobj.IsConditionTrue(totalManaUsed))
                    AchieveObjectiveByIndex(i);
    }
    //off game ones
    public void RunsCompletedObjectiveCheck() {
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is CompletedRunsObjective lvlobj) 
                if (lvlobj.IsConditionTrue(completedRunsCounter))
                    AchieveObjectiveByIndex(i);
    }

    public void TotalGoldAccumulatedObjectiveCheck() {
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is TotalGoldObjective lvlobj) 
                if (lvlobj.IsConditionTrue(totalGoldAccumulated))
                    AchieveObjectiveByIndex(i);
    }

    public void TotalTimePlayedObjectiveCheck() {
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is TotalPlaytimeObjective lvlobj) 
                if (lvlobj.IsConditionTrue((int)inGameTotalPlayTime))
                    AchieveObjectiveByIndex(i);
    }

    public void TotalKillsObjectiveCheck() {
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is TotalKillsObjective lvlobj) 
                if (lvlobj.IsConditionTrue(totalKills))
                    AchieveObjectiveByIndex(i);
    }

    public void PickupObjectiveCheck(string id) {
        Debug.Log("Checking for id: " + id);
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is OnPickupObjective lvlobj) 
                if (lvlobj.GetId() == id)
                    AchieveObjectiveByIndex(i);
    }

    public void PickupObjectiveCheck(Item item) {
        for(int i=0; i < objectives.Length; i++) 
            if (objectives[i] is OnPickupObjective lvlobj) 
                if (lvlobj.GetItem() == item)
                    AchieveObjectiveByIndex(i);
    }
    
    /* non worka per tutto
    public void GeneralObjectiveCheck(int value) {
        for(int i=0; i < objectives.Length; i++) {
            if (objectives[i].IsConditionTrue(value))
                AchieveObjectiveByIndex(i);
        }
    }
    */

    public SerializableDictionary<string,bool> GetObjDone() {
        SerializableDictionary<string,bool> table = new();
        for(int i=0; i < objectives.Length; i++) 
            table.Add(objectives[i].GetId(), conditionsMet[i]);
        
        return table;
    }

    public void SetObjDone(SerializableDictionary<string, bool> table) {
        for(int i=0; i < objectives.Length; i++) {
            string key = objectives[i].GetId();
            bool value = table.ContainsKey(key);
            if(table.ContainsKey(key))
                value = table[key];
            conditionsMet[i] = value;
        }
    }
    
    public bool isObjectiveDone(int index) {
        return conditionsMet[index];
    }

    public List<PowerUp> GetUnlockedPowerupsList() {
        List<PowerUp> list = new();
        for(int i=0; i < conditionsMet.Length; i++) {
            if(objectives[i].unlockedPowerup != null) {
//                Debug.Log("Powerup [" + objectives[i].unlockedPowerup.title + "] unlocked? " + conditionsMet[i]);
                list.Add(objectives[i].unlockedPowerup);
            }
        }

        return list;
    }
    
    public List<bool> GetConditionsMetForPowerups() {
        List<bool> list = new();
        for(int i=0; i < conditionsMet.Length; i++) {
            if(objectives[i].unlockedPowerup != null)
                list.Add(conditionsMet[i]);
        }
        return list;
    }
    //to do: make struct in objective manager that directly corresponds to the powerup
    public bool GetConditionsMetForPowerups(int index) {
        List<bool> list = GetConditionsMetForPowerups();
        return list[index];
    }
    
    public void SortObjectives() {
        Array.Sort(objectives, (x, y) => x.GetType().Name.CompareTo(y.GetType().Name));
    }
    public void SortObjectives2() { //sorting by class name
        Dictionary<string, int> classToFirstIndex = new();
        for (int i = 0; i < objectives.Length; i++) {
            string className = objectives[i].GetType().Name;
            if (!classToFirstIndex.ContainsKey(className)) 
                classToFirstIndex[className] = i;
        }
        Array.Sort(objectives, (x, y) => classToFirstIndex[x.GetType().Name].CompareTo(classToFirstIndex[y.GetType().Name]));
    }
    public void SortObjectives3() { //sorting by class name then by id
        Dictionary<string, int> classToFirstIndex = new();
        Dictionary<string, int> objectiveIdToIndex = new();
        
        for (int i = 0; i < objectives.Length; i++) {
            string className = objectives[i].GetType().Name;
            string objectiveId = objectives[i].GetId();
            
            if (!classToFirstIndex.ContainsKey(className)) 
                classToFirstIndex[className] = i;
            
            if (!objectiveIdToIndex.ContainsKey(objectiveId))
                objectiveIdToIndex[objectiveId] = i;
        }
        
        Array.Sort(objectives, (x, y) => {
            int classComparison = classToFirstIndex[x.GetType().Name].CompareTo(classToFirstIndex[y.GetType().Name]);
            
            if (classComparison == 0) {
                return x.GetId().CompareTo(y.GetId());
            }
            
            return classComparison;
        });
    }

    [Header("Off-Game Data")]
    public int totalGoldAccumulated = 0;
    public float inGameTotalPlayTime = 0;
    public int completedRunsCounter = 0;
    public int totalKills = 0;
    public void AddCompletedRuns() {
        completedRunsCounter++;
        //aggiungere check obiettivo
    }
    public void LoadData(GameData data) {
        totalGoldAccumulated = data.totalGold;
        inGameTotalPlayTime = data.totalPlaytime;
        completedRunsCounter = data.completedRuns;
        totalKills = data.totalKills;
    }

    public void SaveData(ref GameData data) {
        data.completedRuns = completedRunsCounter;
        //se ne occupa il gamemanager di salvare le altre variabili
    }
}
