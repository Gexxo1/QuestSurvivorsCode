using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    protected void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player")
            GameManager.instance.loadScene(sceneToLoad);
    }
}
