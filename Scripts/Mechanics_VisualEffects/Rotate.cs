using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float orbitSpeed = 100;
    private float rotationSpeed = 400;
    protected Transform anchor;
    void Start()
    {
        anchor = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
//        Debug.Log("anchor: " + anchor.localPosition);
        transform.RotateAround(anchor.localPosition, Vector3.forward, Time.deltaTime * orbitSpeed);
        transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
    }
}
