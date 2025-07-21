using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ContactWeapon : Weapon
{
    [Header("Contact Weapon")]
    private ContactHitbox hitbox;
    protected override void Awake() {
        base.Awake();
        hitbox = GetComponentInChildren<ContactHitbox>();
        if(hitbox == null) 
            Debug.LogWarning("No ContactHitbox found on wielder: " + wielder);
    }
    protected override void Start() {
        base.Start();
//        Debug.Log(hitbox);
        /*
        if(wielder.TryGetComponent(out Collider2D coll)) 
            hitbox.SetCollider(coll);
        else
            Debug.LogWarning("No collider2d found on wielder: " + wielder);
        */
    }
    public override string GetWeaponType() {
        return "ContactWeapon";
    }

}

