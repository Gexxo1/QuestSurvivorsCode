using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MagnetCollectable : Collectable
{
    //magnet
    bool hasTarget;
    Vector3 targetPosition;
    private Rigidbody2D rb;
    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void OnEnable() {
        base.OnEnable();
        hasTarget = false;
    }

    public override void OnPickup() {
        if(collected)
            return;
        hasTarget = false;
        base.OnPickup();
    }
    
    private void FixedUpdate() {
        if(hasTarget)
            FollowPlayerPosition();
    }
    private float magnetSpeed = 5.0f;
    private void FollowPlayerPosition() {
        //player --> target position
        Vector2 targetDirection = (player.transform.position - transform.position).normalized;
        magnetSpeed += Time.deltaTime;
        rb.velocity = magnetSpeed * targetDirection; 
        //Debug.DrawLine(transform.position, targetPosition, Color.red);
        //Debug.Log("Collectable moving at " + magnetSpeed + " speed");
    }
    public void SetTarget(float spd) {
        hasTarget = true;
        magnetSpeed = spd;
    }
    public bool HasTarget() {
        return hasTarget;
    }
}
