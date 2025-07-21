using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    protected Collider2D objCollider;
    private Collider2D[] hits = new Collider2D[10]; //l'indice indica con cosa collidiamo
    protected virtual void Start()
    {
        objCollider = GetComponent<Collider2D>();
//        Debug.Log("Collider init: " + objCollider);
    }

    protected virtual void Update()
    {
        //Prendi una array di elementi (con collider) che overlappano il collider
        objCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            //se l'elemento i-esimo è nullo (ovvero che non ha overlappato) skippa un ciclo
            if (hits[i] == null)
                continue;
            
            //Debug.Log("hits: " + hits[i]);
            OnCollide(hits[i]);

            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {

    }
}
