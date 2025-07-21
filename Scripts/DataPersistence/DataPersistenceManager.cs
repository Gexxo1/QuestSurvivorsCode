using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
    private List<IDataPersistence> dataPersistencesObjects;
    public static DataPersistenceManager instance;
    [Header("File Storage Config")]
    [SerializeField] private bool useEncryption;
    [SerializeField] private string gameFileName;
    private FileDataHandler dataHandlerG;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("Settings has a duped instance: " + instance);
            return;
        }
        instance = this;
        if(!useEncryption)
            Debug.Log("Encryption is disabled");
    }
    //GameData
    public void NewGame() { 
        this.gameData = new GameData();
    }

    public void LoadGameSetup() {
        string savePath = Application.persistentDataPath;
        
        #if UNITY_WEBGL         
            savePath = "idbfs/Quest_Survivors_Data"; 
        #endif
        
        this.dataHandlerG = new FileDataHandler(savePath, gameFileName, useEncryption);
        this.dataPersistencesObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    
    public void LoadGame() { 
        //Carica da file | NOTA: Quando il file è mancante il risultato sarà NULL e non interferirà in alcun modo col codice a causa dell'if quassotto
        this.gameData = dataHandlerG.LoadGameData();

        if(this.gameData == null) {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects) {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("Loaded data: " + gameData.ToString());
    }

    public void SaveGame() {
        // pass data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects) {
            dataPersistenceObj.SaveData(ref gameData);
        }

        Debug.Log("Saved data: " + gameData.ToString());

        //Salva in un file
        dataHandlerG.SaveGameData(gameData);
    }

    private void OnApplicationQuit() {
        if(this.gameData != null)
            SaveGame();
        else
            Debug.Log("Data not present, quit without saving");
        /*
        if(this.settData != null)
            SaveSettings();
        else
            Debug.Log("Settings not present, quit without saving");
        */
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPeristenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPeristenceObjects);
    }

    
}

