using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : ProjectileWeaponPowerup
{
    [Header("Sentry")]
    [SerializeField] private GameObject visualSprite;
    private Vector2 direction;
    protected override void Awake() {
        SetColliderSizeAsCamera();
    }
    private void SetColliderSizeAsCamera() {
        // Ottenere la dimensione della camera
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        BoxCollider2D collider = GetComponentInChildren<BoxCollider2D>();
        
        collider.size = new Vector2(cameraWidth, cameraHeight);
    }
    protected override void Update() {
        base.Update();
        if(closestEnemy != null) {
            //Debug.DrawLine(endpoint.transform.position, closestEnemy.transform.position, Color.magenta);
            direction = (closestEnemy.transform.position - endpoint.transform.position).normalized;
            RotateToDirection(5.0f);
            if(CanShoot())
                Shoot(direction);
        }
        
    }
    protected void OnTriggerStay2D(Collider2D coll) {
        if(coll.TryGetComponent(out Enemy e)) 
            Aim(e);
    }
    private Enemy closestEnemy;
    protected override void Aim(Enemy target) {
        if(closestEnemy != null) {
            if(closestEnemy.isDead) {
                closestEnemy = null;
                return;
            }
            if(closestEnemy == target)
                return;
        }
        
        // Se non abbiamo ancora un nemico più vicino, o se questo nemico è più vicino del nemico corrente più vicino, allora aggiorna il nemico più vicino
        if (closestEnemy == null || Vector2.Distance(transform.position, target.transform.position) < Vector2.Distance(transform.position, closestEnemy.transform.position)) 
            closestEnemy = target;
    }
    private void RotateToDirection(float rotationSpeed) {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        visualSprite.transform.rotation = Quaternion.Slerp(visualSprite.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        /*
        Vector3 aimLocalScale = Vector3.one;
        if(angle > 90 || angle < -90) 
            aimLocalScale.y = -1f;
        else
            aimLocalScale.y = +1f;
        
        visualSprite.transform.localScale = aimLocalScale;
        */
    }
}
