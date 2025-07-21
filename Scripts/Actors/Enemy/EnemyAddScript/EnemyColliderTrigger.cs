using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyColliderTrigger : MonoBehaviour
{
    private Enemy parent;
    private void Awake() {
        parent = GetComponentInParent<Enemy>();
        //Debug.Log("Parent: " + parent);
    }
    private void OnTriggerEnter2D(Collider2D coll) {
        parent.collidingWithPlayer = true;
//        Debug.Log("Player entered trigger");
    }
    private void OnTriggerExit2D(Collider2D coll) {
        parent.collidingWithPlayer = false;
    }

}
