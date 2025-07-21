using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Item
{
    //[HideInInspector] public bool collected = false;
    [Header("Collectable")]
    protected Player player;
    [SerializeField] protected float collectDelay = 0.5f;
    protected bool collected = false;
    protected virtual void OnEnable() {
        collected = false;
    }
    protected virtual void Start() {
        player = GameManager.instance.player;
    }

    protected virtual void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
            OnPickup();
    }
    //this is should be put for inherited members in the end of the override method
    public virtual void OnPickup() 
    {
        if(collected) return;
        DestroyCollectable();
        collected = true;
    }
    public virtual void OnCollect() { Debug.Log("calling base class collect");}
    protected virtual void DestroyCollectable() {
        bool flag = ObjectPoolManager.TryReturnObjectToPool(gameObject);
        if(!flag) {
            Debug.LogWarning("Collectable " + name + " not found in pool, destroying it manually.");
            Destroy(gameObject);
        }
    }
    /*
    public IEnumerator DelayedCollect(float delay)
    {
        //collected = true; //anche se non è stato effettivamente collezionato lo setto a true per bloccare momentaneamente la collezione
        yield return new WaitForSeconds(delay);
        //collected = false;
        yield return null;
    }
    */
}
