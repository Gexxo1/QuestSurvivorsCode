using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMechanic : MonoBehaviour {
    [SerializeField] private CircleCollider2D homingAura;
    private Transform currTarget;
    private Bullet bulletRef;
    private bool stopHoming = false;
    private void Awake() {
        bulletRef = GetComponentInParent<Bullet>();
    }
    private void OnEnable() {
        ResetTarget();
        stopHoming = false;
    }
    private void OnDisable() {
        ResetTarget();
        stopHoming = true;
//        Debug.Log("Homing: Disabled");
    }
    /*  issue: ontriggerenter2d doesn't actually get the first closest target
            * changing target every ontriggerenter2d makes only work worse
        solution: use a list of targets and get the closest one by calculating the distance (it's the direction)
            * the closest one check it's only when a new target is found, and the entire list is checked
    */
    private void OnTriggerEnter2D(Collider2D coll) {
        //if (!coll.CompareTag("Enemy")) return; commentato perchè è gestito direttamente in physics2d
        if (currTarget != null || stopHoming) return;
        currTarget = coll.transform;
        Vector2 dir = (currTarget.position - bulletRef.transform.position).normalized;
        bulletRef.ChangeDirection(dir);
        //bulletRef.ChangeDirection(dir);
        //Debug.Log("Homing: New target found [" + currTarget.name + "] new direction [" + dir + "]");
    }
    /*
    private void OnTriggerExit2D(Collider2D coll) {
        //if (currTarget != null || stopHoming) return;
        bulletRef.RevertDirection();
    }
    */
    public void ResetTarget() {
        currTarget = null;
    }

    public void SetHomingAura(float size) {
        homingAura.radius = size;
    }
}