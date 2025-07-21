using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Collectable
{
    [SerializeField] protected Item itemPrefab;
    public override void OnPickup() {   
        if(collected)
            return;
        GameManager.instance.inv.AddItemToInventory(itemPrefab.gameObject);
        base.OnPickup();
    }

    public GameObject getPickupGO() {
        return itemPrefab.gameObject;
    }   
}
