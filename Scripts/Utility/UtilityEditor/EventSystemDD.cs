using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemDD : MonoBehaviour
{
    private static EventSystemDD eventInstance;
    void Awake(){
        DontDestroyOnLoad(this);
            
        if (eventInstance == null) 
            eventInstance = this;
        else 
        Destroy(gameObject);
        
    }
}
