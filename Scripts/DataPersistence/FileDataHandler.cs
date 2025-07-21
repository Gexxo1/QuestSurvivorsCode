using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption;
    private readonly string encryptionCodeWord = "pippopappo";
    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }
    //Game
    public GameData LoadGameData() {
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath)) {
            try {
                // Load the serialized data from the file
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //optionally decrypt the data
                if(useEncryption)
                    dataToLoad = EncryptDecrypt(dataToLoad);
                //Deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e) {
                Debug.LogError("Error upon loading data file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void SaveGameData(GameData data) {
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        //oppure (scelta peggiore in quanto alcuni Sistemi Operativi non hanno lo slash come Path)
        //string fullPath = dataDirPath + "/" + dataFileName;
        try{
            //crea una directory se non esiste già
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //convert data into json
            string dataToStore = JsonUtility.ToJson(data,true);
            //optionally encrypt data
            if(useEncryption)
                dataToStore = EncryptDecrypt(dataToStore);
            using (FileStream stream = new(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new(stream)) {
                    writer.Write(dataToStore);
                }
            }
        }catch(Exception e) {
            Debug.LogError("Error upon saving data file: " + fullPath + "\n" + e);
        }
        
    }
    private string EncryptDecrypt(string data) {
        string modifiedData = "";
        for(int i=0; i < data.Length; i++) {
            modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
