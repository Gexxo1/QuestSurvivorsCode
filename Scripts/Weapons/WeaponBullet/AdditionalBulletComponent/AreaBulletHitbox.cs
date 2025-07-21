using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBulletHitbox : MonoBehaviour
{
    private BulletAreaDamage areaRef;
    private void Awake() {
        areaRef = GetComponentInParent<BulletAreaDamage>();
    }

    private void OnTriggerStay2D(Collider2D coll) {
//        Debug.Log("AreaBulletHitbox: OnTriggerStay2D " + areaRef);
        areaRef.HandleCollision(coll);
    }

    //this is called in animation when it ends
    public void ReleaseFromPool() {
        areaRef.ReleaseFromPool();
    }
}
