using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsUp", menuName = "ScriptableObjects/Stats/StatsUp")]
public class StatsUp : Stats
{
    
    /*

    public string GetAllReplacements(string s, bool isTiered, int tier) {
        List<double> list;
        if(tier != 0)
            list = GetStatsList(tier);
        else
            list = GetStatsListUntiered();
        int i = 0;
        string outs = s;
        //Debug.Log("---replacing " + list.Count + " stats in " + title + " tier " + tier);
        foreach(double d in list) {
            string v = "<v" + i + ">";
            //Debug.Log("Value no." + i + " [" + d + "] Regex[" + v + "]");
            outs = Regex.Replace(outs, v, list[i].ToString(), RegexOptions.IgnoreCase);
            i++;
        }
        return outs;
    }
    public List<double> GetStatsListUntiered() {
        return GetStatsList(1);
    }
    */
}
