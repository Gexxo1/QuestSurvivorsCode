using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public string id;
    private void Awake() {
        id = name + transform.position.ToString() + transform.eulerAngles.ToString();

//        Debug.Log("id=[" + id+"]");
        for(int i=0; i < Object.FindObjectsOfType<DontDestroy>().Length; i++) {
//            Debug.Log(Object.FindObjectsOfType<DontDestroy>()[i]);
            if(Object.FindObjectsOfType<DontDestroy>()[i] != this) {
                if(Object.FindObjectsOfType<DontDestroy>()[i].name == gameObject.name) {
                    //Debug.Log("distruzione[" + gameObject.name + "]");
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
        
    }
}
