using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//IMPORTANTE: Mover è una classe abstract e non può essere implementata nei gameObjects, 
//è una classe utilizzata da altre classi attraverso l'ereditarietà

public abstract class Mover : MonoBehaviour
{
    [Header("Mover")]
    protected Animator animator;
    protected Vector3 moveDelta;
    protected Rigidbody2D rb;
    protected bool isKnocked = false;
    protected float movementPenalty = 1;
    [HideInInspector] public bool stopMoving = false;
    protected bool knockbackImmunity = false;
    protected virtual void Awake() {
        if(TryGetComponent(out Animator anim))
            animator = anim;
        if(TryGetComponent(out Rigidbody2D rb2d))
            rb = rb2d;
        else
            rb = GetComponentInChildren<Rigidbody2D>();
        
        if(rb == null)
            Debug.LogWarning("Rigidbody2D not found");
    }
    protected abstract void FixedUpdate();
    public virtual void Movement(Vector3 direction, float modifier) {
        if(isKnocked) return;
        moveDelta = (Vector2)direction.normalized;
        rb.velocity = (Vector2)moveDelta * (modifier * movementPenalty);
        //Debug.Log(gameObject + " Moving: " + moveDelta);
    }
    public virtual void Push(float modifier) { //used for dash
        rb.velocity = (Vector2)moveDelta * (modifier * movementPenalty);
    }
    public void Push(Vector2 dir, float modifier) { //used for dash with direction (enemy)
        rb.velocity = new Vector2(dir.x, dir.y) * (modifier * movementPenalty);
    }
    public virtual void PushForce(Vector3 direction, float modifier) { //used for knockback
        rb.AddForce(direction * modifier, ForceMode2D.Impulse);
    }
    public virtual void StopMoving() {
        rb.velocity = Vector2.zero;
    }
    
    protected IEnumerator KnockbackDelay(Vector3 dir, float mod) {
        if(knockbackImmunity)
            yield break;
        isKnocked = true;
        PushForce(dir,mod);

        yield return new WaitForSeconds(0.1f);

        isKnocked = false;

        yield return null;
    }
}
