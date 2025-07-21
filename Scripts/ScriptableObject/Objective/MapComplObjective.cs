using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapObjective", menuName = "ScriptableObjects/Objectives/MapObjective")]
public class MapComplObjective : Objective
{
    public MapData map;
    public override bool IsConditionTrue(int value) { //il parametro c'è ma non deve essere inizializzato
        return WaveManager.instance.waveComplete && GameManager.instance.isCurrentScene(map.sceneId);
    }
    public override string GetReqDesc() {
        return "Complete map [" + map.GetTitle() + "]";
    }
    public override string GetTitle() {
        return "Map Completion " + map.GetTitle();
    }
    public override string GetId() { return base.GetId() + "map_" + id; }
    //public override string GetId() { return base.GetId() + "map_" + map.sceneId; }
}
