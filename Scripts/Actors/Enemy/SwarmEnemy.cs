using UnityEngine;

public class SwarmEnemy : Enemy
{
    [SerializeField] private float swarmSpeedModifier = 3f;
    private Vector3 dir;
    protected override void OnEnable() {
        base.OnEnable();
        dir = GameManager.instance.player.transform.position - transform.position;
    }
    protected override void Aggro() {
        Movement(dir, stats.moveSpeed*swarmSpeedModifier);
    }

    //this type of enemy doesn't aim to player
    protected override void AimToPlayer() { }

    //if this enemy collides with a certain tagged tile (found in boundaries), it makes it despawn
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("EnemyDespawn"))
            DestroyEnemy(0);
    }

    
}   
