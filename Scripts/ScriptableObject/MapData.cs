using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData")]
public class MapData : ScriptableObject
{
    [SerializeField] public string sceneId;
    [SerializeField] private string title;
    [SerializeField] [TextArea(2,4)] public string description = "description not set";
    [SerializeField] public Sprite preview;
    [SerializeField] public WaveData waveData;
    public string GetTitle() { return title; }
}