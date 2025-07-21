using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitbox : MonoBehaviour
{
    private Bullet bulletRef;
    [Header("Optional - can be null")]
    [SerializeField] private HomingMechanic homingRef;
    private void Awake() {
        bulletRef = GetComponentInParent<Bullet>();
//        if(homingRef == null) Debug.LogWarning("BulletHitbox: homing ref is not init");
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        //Debug.Log("BulletHitbox: OnTriggerEnter2D " + bulletRef);
        bulletRef.HandleCollision(coll);
    }
    private void OnTriggerExit2D(Collider2D coll) {
        if(homingRef != null)
            homingRef.ResetTarget();
    }

    //this is called in animation when it ends
    public void ReleaseFromPool() {
        bulletRef.ReleaseFromPool();
    }
}
