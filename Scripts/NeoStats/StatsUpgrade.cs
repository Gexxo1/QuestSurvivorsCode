using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeoStats", menuName = "ScriptableObjects/NeoStats/NeoStats")]
public class StatsUpgrade : ScriptableObject
{
    public List<Stat> stats;
    [Serializable] public class Stat {
        public StatID id;
        public StatType type;
        public float value;
    }
}
