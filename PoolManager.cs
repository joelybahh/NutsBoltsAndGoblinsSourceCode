using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PoolManager : MonoBehaviour {

    Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

    static PoolManager _instance;
    public static PoolManager Instance {
        get {
            if(_instance == null) {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    public void CreatePool( GameObject prefab, int poolSize, Transform parent = null ) {
        int poolKey = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey)) {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for(int i = 0; i < poolSize; i++) {
                GameObject newObj = Instantiate(prefab) as GameObject;
                newObj.SetActive(false);
                if (parent == null) newObj.transform.parent = transform.GetChild(0);
                else newObj.transform.parent = parent;
                poolDictionary[poolKey].Enqueue(newObj);
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation ) {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey)) {
            GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.SetActive(true);
            objectToReuse.transform.position = position;
            objectToReuse.transform.rotation = rotation;
        }
    }

    // TODO: encorporate this into the other function 
    public void ReuseTextAndReText( GameObject prefab, Vector3 position, Quaternion rotation, string newText ) {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey)) {
            
            GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
            objectToReuse.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = newText;
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.SetActive(true);
            objectToReuse.transform.position = position;
            objectToReuse.transform.rotation = rotation;

        }
    }
}
