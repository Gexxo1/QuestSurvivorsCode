using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GlowEffect : MonoBehaviour
{
    private SpriteRenderer sprite;
    private bool decrement = true;
    private float nextActionTime = 0.0f;
    [SerializeField] private float period = 0.1f; 
    private float value = 0.1f;
    void Start()
    {  
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Time.time > nextActionTime) {
            //Debug.Log(Time.time + " >= "+ nextActionTime + " A(" + sprite.color.a + ")");
            nextActionTime += period;

            if(sprite.color.a <= 0.5f)
                decrement = false;
            else if(sprite.color.a >= 1f)
                decrement = true;
            
            if(decrement)
                sprite.color -= new Color(0f,0f,0f,value);
            else
                sprite.color += new Color(0f,0f,0f,value);
        }
    }
}
