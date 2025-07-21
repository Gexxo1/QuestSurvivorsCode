using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "ScriptableObjects/Skin")]
public class Skin : ScriptableObject
{
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private Animation animation;
}
