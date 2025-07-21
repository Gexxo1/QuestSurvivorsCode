using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drop", menuName = "ScriptableObjects/Drop/DropList")]
public class DropList : ScriptableObject
{
    [SerializeField] Drop[] drops;

    public Drop[] getDropList() {
        return drops;
    }
}
