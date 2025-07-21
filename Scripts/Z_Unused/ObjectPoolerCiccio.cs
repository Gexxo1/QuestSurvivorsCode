using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem {
	public GameObject objectToPool;
	public int amountToPool = 1;
	public bool shouldExpand = true;

	public ObjectPoolItem(GameObject obj, int amt, bool exp = true) {
		objectToPool = obj;
		amountToPool = Mathf.Max(amt,2);
		shouldExpand = exp;
	}
}

public class ObjectPooler : MonoBehaviour {
	public static ObjectPooler SharedInstance;
	public List<ObjectPoolItem> itemsToPool;

	public List<List<GameObject>> pooledObjectsList;
	public List<GameObject> pooledObjects;
	private List<int> positions;

	void Awake() {
		SharedInstance = this;

		pooledObjectsList = new List<List<GameObject>>();
		pooledObjects = new List<GameObject>();
		positions = new List<int>();

		for (int i = 0; i < itemsToPool.Count; i++)
			ObjectPoolItemToPooledObject(i);
	}


	public GameObject GetPooledObject(int index) {
		Debug.Log("GetPooledObject: " + itemsToPool[index].objectToPool.name);
		int curSize = pooledObjectsList[index].Count;
		for (int i = positions[index] + 1; i < positions[index] + pooledObjectsList[index].Count; i++) {
			if (!pooledObjectsList[index][i % curSize].activeInHierarchy) {
				positions[index] = i % curSize;
				return pooledObjectsList[index][i % curSize];
			}
		}

		if (itemsToPool[index].shouldExpand) {
			Debug.Log("Expanding: " + itemsToPool[index].objectToPool.name);
			GameObject obj = Instantiate(itemsToPool[index].objectToPool);
			obj.SetActive(false);
			obj.transform.parent = transform;
			pooledObjectsList[index].Add(obj);
			return obj;
		}

		Debug.LogWarning("No existing objects of type: " + itemsToPool[index].objectToPool.name);
		return null;
	}

	public List<GameObject> GetAllPooledObjects(int index) { return pooledObjectsList[index]; }

	public int AddObject(GameObject GO, int amt = 3, bool exp = true) {
		ObjectPoolItem item = new(GO, amt, exp);
		int currLen = itemsToPool.Count;
		itemsToPool.Add(item);
		ObjectPoolItemToPooledObject(currLen);
		return currLen;
	}

	void ObjectPoolItemToPooledObject(int index) {
		ObjectPoolItem item = itemsToPool[index];

		pooledObjects = new List<GameObject>();
		for (int i = 0; i < item.amountToPool; i++) {
			GameObject obj = Instantiate(item.objectToPool);
			obj.SetActive(false);
			obj.transform.parent = transform;
			pooledObjects.Add(obj);
		}
		pooledObjectsList.Add(pooledObjects);
		positions.Add(0);
	}
}