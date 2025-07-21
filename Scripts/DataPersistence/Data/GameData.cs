using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * in game data ci sono i progressi
 * che devono essere mantenuti per sempre
*/
[System.Serializable]
public class GameData 
{
    //in game data
    public int gold;
    //dictionary data
    public SerializableDictionary<string, int> classes;
    public SerializableDictionary<string, bool> objectives;
    public SerializableDictionary<string, bool> maps;
    public SerializableDictionary<string, int> upgrades;
    //off game data
    public int totalGold = 0; //note: this is the total gold accumulated not the current gold, so it's not affected by spending gold in the shop
    public float totalPlaytime = 0;
    public int completedRuns = 0;
    public int totalKills = 0;
    public GameData() {
        gold = 0;

        classes = new SerializableDictionary<string, int>();
        objectives = new SerializableDictionary<string, bool>();
        maps = new SerializableDictionary<string, bool>();
        upgrades = new SerializableDictionary<string, int>();

        totalGold = 0;
        totalPlaytime = 0;
        completedRuns = 0;
        totalKills = 0;
    }

    public override string ToString() {
        string ct, od, mc, up;
        ct = od = mc = up = "";
        foreach (var classPair in classes) 
            ct += $"({classPair.Key}: {classPair.Value}) ";
        foreach (var objPair in objectives) 
            od += $"({objPair.Key}: {objPair.Value}) ";
        foreach (var mapPair in maps) 
            mc += $"({mapPair.Key}: {mapPair.Value}) ";
        foreach (var upPair in upgrades) 
            up += $"({upPair.Key}: {upPair.Value}) ";

        return "InGameData: gold[ " + gold + " ]\n" +
        
            "OffGameData: totalgold[ " + totalGold + " ] " + //NOTE: see line 19
            "totalPlaytime[ " + totalPlaytime + " ] " +
            "completedRuns[ " + completedRuns + " ] " +
            "totalKills[ " + totalKills + " ]\n" +

            "Dictionary: classes tier [ " + ct + "] " +
            "obj done [ " + od + " ] " +
            "cleared maps [ " + mc + " ] " +
            "upgrades [ " + up + "] ";
    }
}
