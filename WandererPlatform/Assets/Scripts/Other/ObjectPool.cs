using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool Instance { get; private set; }


    private Queue<GameObject> availableObjects;
    [SerializeField] private GameObject objectToPool;

    private void Awake() {
        Instance = this;
        GrowPool();
    }

    private void GrowPool() {
        availableObjects = new Queue<GameObject>();

        for (var i = 0; i < 10; i++) {
            GameObject instanceToAdd = Instantiate(objectToPool, transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance) {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }
    
    public GameObject GetFromPool()
    {
        if (availableObjects.Count == 0) {
            GrowPool();
        }

        GameObject instance = availableObjects.Dequeue();
        //instance.SetActive(true);
        return instance;
    }  
}
