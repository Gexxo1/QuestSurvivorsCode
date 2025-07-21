using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
    
    //void LoadSettings(SettingsData data);
    //void SaveSettings(ref SettingsData data);
}
