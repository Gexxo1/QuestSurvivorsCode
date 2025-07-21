using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * in game data ci sono i progressi
 * che devono essere mantenuti per sempre
*/
[System.Serializable]
public class ResultsData : GameData 
{
    public int potsBroken;
    public int totalManaUsed;
    public int damageTaken;
    public int projectilesShot;
    public ResultsData() {
        potsBroken = 0;
        totalManaUsed = 0;
        damageTaken = 0;
        projectilesShot = 0;
    }

    public override string ToString() {
        return base.ToString() + "\n"
            + "potsBroken[ " + potsBroken + " ]\n"
            + "totalManaUsed[ " + totalManaUsed + " ]\n"
            + "damageTaken[ " + damageTaken + " ]\n"
            + "projectilesShot[ " + projectilesShot + " ]\n";
    }
    /*
    public string[] GetResultsArray() {
        return new string[] {
            "gold[ " + gold + " ]",
            "totalKills[ " + totalKills + " ]",
            "totalPlaytime[ " + totalPlaytime + " ]",

            "damageTaken[ " + damageTaken + " ]",
            "potsBroken[ " + potsBroken + " ]",
            "totalManaUsed[ " + totalManaUsed + " ]"
        };
    }
    */
    //Dictionary: <key, (nome visualizzato, valore effettivo)>
    public Dictionary<string, (string, string)> GetResultsDictionary() {
        return new Dictionary<string, (string, string)> {
            {"gold", ("Gold Obtained", gold.ToString())},
            {"totalKills", ("Enemies Slained", totalKills.ToString())},
            {"totalPlaytime", ("Run Playtime", totalPlaytime.ToString())},
            
            {"damageTaken", ("Damage Taken", damageTaken.ToString())},
            {"potsBroken", ("Pots Broken", potsBroken.ToString())},
            {"totalManaUsed", ("Mana Used", totalManaUsed.ToString())},
            {"projectilesShot", ("Projectiles Shot", projectilesShot.ToString())}
        };;
    }
}
