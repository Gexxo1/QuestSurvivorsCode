using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAim : MonoBehaviour
{
    private GameObject weaponsHolder;
    [HideInInspector] public Vector3 aimDirection;
    private void Awake() {
        //weapon = transform.Find("Weapons").gameObject;
        weaponsHolder = FindWeaponsGO();
    }
    private GameObject FindWeaponsGO() {
        Transform weaponHolderTransform = transform.Find("Inventory");
        Transform wpnTransform;
        if (weaponHolderTransform != null) 
            wpnTransform = weaponHolderTransform.Find("Weapons");
        else
            wpnTransform = transform.Find("Weapons");
            
        if(wpnTransform == null) {
            Debug.LogWarning("Weapon not found in player");
            return null;
        }
        weaponsHolder = wpnTransform.gameObject;
        return weaponsHolder;
    }
    private void FixedUpdate() {
        if(Time.timeScale == 0f) //se il gioco è in pausa --> non mirare
            return;
        
        Aiming(Utility.GetMouseWorldPosition(), weaponsHolder.transform);
    }

    public void Aiming(Vector3 mousePosition, Transform aimTransform) {
        //Vector3 mousePosition = Utility.GetMouseWorldPosition();
        aimDirection = (mousePosition - weaponsHolder.transform.position).normalized;  
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0,0,angle);
        
        weaponsHolder.transform.right = aimDirection;
        //Debug.Log(currWeapon.title + " --> " + currWeapon.transform.right);
        Vector3 aimLocalScale = Vector3.one;
        if(angle > 90 || angle < -90) 
            aimLocalScale.y = -1f;
        else
            aimLocalScale.y = +1f;
        
        aimTransform.localScale = aimLocalScale;
        
    }
}
