using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchMarkTest : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start() {
        player = GameManager.instance.player;
    }
    public void PerformBenchmarkTest() {
        FollowPlayerPosition();
    }

    private void FollowPlayerPosition() {
        Vector2 targetDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = 5 * targetDirection; 
    }
}
