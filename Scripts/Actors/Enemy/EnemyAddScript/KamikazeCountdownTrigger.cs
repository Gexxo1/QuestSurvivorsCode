using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeCountdownTrigger : MonoBehaviour
{
    [SerializeField] private KamikazeEnemy kamikazeRef;
    private void OnTriggerEnter2D(Collider2D coll) {
        if(coll.TryGetComponent(out Player p)) 
            kamikazeRef.BeginOrStopCountdown(true);
    }

    private void OnTriggerExit2D(Collider2D coll) {
        if(coll.TryGetComponent(out Player p)) 
            kamikazeRef.BeginOrStopCountdown(false);
    }
}
