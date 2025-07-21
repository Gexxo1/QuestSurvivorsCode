using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ClassSelection", menuName = "ScriptableObjects/Class/Selection")]
public class CharacterClassSelection : ScriptableObject
{
   [SerializeField] public Class charClass;
   [SerializeField] public bool isOwned;
   [SerializeField] public int cost;
}
