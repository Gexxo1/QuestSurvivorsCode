using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerOld : MonoBehaviour
{
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;

        public void SetPool(string t, GameObject p, int s) {
            tag = t;
            prefab = p;
            size = s;
        }
    }

    #region Singleton
    public static ObjectPoolerOld instance;
    private void Awake() {
        instance = this;
    }
    #endregion
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    void Start() {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools) {
            Queue<GameObject> objectPool = new();
            for(int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        if(!poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        //if(objectToSpawn.TryGetComponent<IPooledObject>(out var pooledObj)) pooledObj.OnObjectSpawn();

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    
}
