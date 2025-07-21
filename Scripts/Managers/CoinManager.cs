using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    private void Awake() {
        if(instance != null) {
            Debug.Log("WARNING: Duplicate CoinManager Instance: " + instance);
        }
        instance = this;
    }
    public Coin coinPrefab;
    public float posRange = 1;
    private Vector3 newPos;
    public void SpawnCoin(Vector3 pos) {
        newPos = new Vector3(pos.x + Random.Range(0,posRange), pos.y + Random.Range(0,posRange),0);
        Coin coinInstance = Instantiate<Coin>(coinPrefab, newPos, Quaternion.identity);
    }

    
}
