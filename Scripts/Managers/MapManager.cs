using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapData[] maps;
    [SerializeField] private List<bool> cleared;
    [SerializeField] private List<bool> unlocked;
    public static MapManager instance;
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Debug.LogError("Found another map manager in scene");
    }
    private void UnlockedSetup() {
        unlocked = new List<bool> { true };
        for(int i=1; i < cleared.Count; i++)
            unlocked.Add(cleared[i-1]); 
    }
    public void UnlockAll() { 
        for(int i=0; i < unlocked.Count; i++) 
            Unlock(i);
        for(int i=0; i < cleared.Count; i++) 
           SetMapCleared(i);
    }
    public void Unlock(int index) { 
        if(index >= unlocked.Count) {
            Debug.LogWarning("Trying to unlock map " + index + " but it doesn't exist");
            return;
        }
//        Debug.Log("map " + maps[index].sceneId + " succesfully unlocked");
        unlocked[index] = true;
    }
    private int SearchIndexByName(string name) {
        for(int i=0; i < maps.Length; i++) {
            if(maps[i].sceneId == name)
                return i;
        }
        return -1;
    }
    public string GetMapId(int index) {
        return maps[index].sceneId;
    }
    public int GetMapCount() {
        return maps.Length;
    }
    //map clear
    public void MapCleared() {
        int index = SearchIndexByName(GameManager.instance.GetCurrentSceneName());
        SetMapCleared(index);
    }
    public void SetMapCleared(int index) {
        cleared[index] = true;
    }
    public bool IsMapCleared(int index) {
        return cleared[index];
    }
    public int GetIndexByMapData(MapData map) {
        return SearchIndexByName(map.sceneId);
    }
    public bool IsMapClearedByMapData(MapData map) {
        return IsMapCleared(SearchIndexByName(map.sceneId));
    }
    /*
    public void SetClearedList(List<bool> list) {
        cleared = new List<bool>();
        for(int i=0; i < maps.Length; i++) {
            //Debug.Log((list != null) + " && " + (i < list.Count) + " && " + list[i]);
            bool value = list != null && i < list.Count && list[i];
            cleared.Add(value);
        }
        UnlockedSetup();
    }
    public List<bool> GetClearedList() {
        List<bool> list = new();
        for(int i=0; i < maps.Length; i++) {
            //Debug.Log("adding " + cleared[i]);
            list.Add(cleared[i]);
        }
        return list;
    }
    */
    
    public void SetClearedMaps(SerializableDictionary<string, bool> table) {
        cleared = new List<bool>();
        for(int i=0; i < maps.Length; i++) {
            string key = maps[i].sceneId;
            bool value = table.ContainsKey(key);
            if(table.ContainsKey(key))
                value = table[key];
            cleared.Add(value);
        }
        UnlockedSetup();
    }
    public SerializableDictionary<string,bool> GetClearedMaps() {
        SerializableDictionary<string,bool> table = new();
        for(int i=0; i < maps.Length; i++) {
            //Debug.Log("adding " + cleared[i]);
            table.Add(maps[i].sceneId, cleared[i]);
        }
        return table;
    }
    
    /*
    if(data.maps.ContainsKey(id)) data.maps.Remove(id);
        data.maps.Add(id,mapManager.IsMapCleared())
    */
    //getters & setters
    public bool IsUnlocked(int index) { 
        if(index >= unlocked.Count) return false;
        return unlocked[index]; 
    }
    public string[] GetSceneNames() {
        int n = maps.Length;
        string[] s = new string[n];
        for(int i=0; i < n; i++)
            s[i] = maps[i].sceneId;
        return s;
    }
    public string GetSceneName(int index) {
        return maps[index].sceneId;
    }
    public string GetSceneTitle(int index) {
        return maps[index].GetTitle();
    }
    public string GetSceneDescription(int index) {
        return maps[index].description;
    }
    public int GetSceneLength() {
        return maps.Length;
    }
    public Sprite GetScenePreview(int index) {
        return maps[index].preview;
    }
    private bool endlessSelection = false;
    private bool isTestSelection = false;
    public bool IsEndlessSelection() { return endlessSelection; } public bool IsTestSelection() { return isTestSelection; }
    public WaveData GetCurrentWaveDataIndex() {
        if(currentMapIndex == -1) return null;
        return maps[currentMapIndex].waveData;
    }
    public WaveData[] GetWaveData() {
        WaveData[] waveData = new WaveData[maps.Length];
        for(int i=0; i < maps.Length; i++)
            waveData[i] = maps[i].waveData;
        return waveData;
    }
    public List<Enemy> GetAllEnemiesList() {
        List<Enemy> enemies = new();
        for(int i=0; i < maps.Length; i++) {
            for(int j=0; j < maps[i].waveData.mainWaveEnemies.Length; j++) 
                enemies.Add(maps[i].waveData.mainWaveEnemies[j]);
            for(int j=0; j < maps[i].waveData.specialWaveEnemies.Length; j++) 
                enemies.Add(maps[i].waveData.specialWaveEnemies[j]);
            if(maps[i].waveData.swarmEnemy != null) 
                enemies.Add(maps[i].waveData.swarmEnemy);
            if(maps[i].waveData.bossEnemy != null) 
                enemies.Add(maps[i].waveData.bossEnemy);
        }
        return enemies;
    }
    public MapData GetMapDataByEnemy(Enemy enemy) {
        for(int i=0; i < maps.Length; i++) {
            for(int j=0; j < maps[i].waveData.mainWaveEnemies.Length; j++) 
                if(maps[i].waveData.mainWaveEnemies[j] == enemy)
                    return maps[i];
            for(int j=0; j < maps[i].waveData.specialWaveEnemies.Length; j++) 
                if(maps[i].waveData.specialWaveEnemies[j] == enemy)
                    return maps[i];
            if(maps[i].waveData.swarmEnemy == enemy)
                return maps[i];
            if(maps[i].waveData.bossEnemy == enemy)
                return maps[i];
        }
        return null;
    }
    int currentMapIndex = -1;
    public void LoadMap(int index, bool isEndless, bool isTest) {
        string sceneName = GetSceneName(index);
        Debug.Log("Map Loaded " + sceneName);
        SceneManager.LoadScene(sceneName);
        endlessSelection = isEndless;
        currentMapIndex = index;
        isTestSelection = isTest;
    }
    
}
