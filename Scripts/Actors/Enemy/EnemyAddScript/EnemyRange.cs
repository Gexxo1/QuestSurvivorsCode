using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyRange : MonoBehaviour
{
    public bool isInRange = false;
    private void OnTriggerEnter2D(Collider2D coll) {
        if(coll.TryGetComponent(out Player p)) 
            isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D coll) {
        if(coll.TryGetComponent(out Player p))
            isInRange = false;
    }
}
