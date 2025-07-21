using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private float originalSize;
    private CircleCollider2D magnetColl;
    [SerializeField] private float magnetSpeed = 5.0f;
    private float originalSpd;
    private float amplifiedMagnetSpeed;
    private void Awake() {
        magnetColl = GetComponent<CircleCollider2D>();
        originalSize = magnetColl.radius;
        originalSpd = magnetSpeed;
        amplifiedMagnetSpeed = magnetSpeed * 2;
    }
    private void OnTriggerStay2D(Collider2D coll) {
        if(coll.TryGetComponent(out MagnetCollectable mc) && !mc.HasTarget())
            mc.SetTarget(magnetSpeed);
    }
    public void IncreaseSize(float sizeIncrease) {
        magnetColl.radius += sizeIncrease;
    }
    public void SetSize(float size) {
        magnetColl.radius = size;
        magnetSpeed = amplifiedMagnetSpeed;
    }
    public void ResetSize() {
        magnetColl.radius = originalSize;
        magnetSpeed = originalSpd;
    }
    private void OnApplicationQuit() {
        ResetSize();
    }
}
