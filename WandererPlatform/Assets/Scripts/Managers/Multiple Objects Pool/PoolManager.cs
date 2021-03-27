using System;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType {
    Bullet,
    BulletImpactEffect,
    EnemyDeathEffect,
    EnemyFireBall,
    ItemCollectEffect,
}

[Serializable]
public class PoolInfo {
    public PoolObjectType type;
    public int amount;
    public GameObject prefab;
    public GameObject container;

    [HideInInspector] public List<GameObject> pool = new List<GameObject>();
}

public class PoolManager : MonoBehaviour {
    public static PoolManager Instance { get; private set; }

    [SerializeField] private List<PoolInfo> listOfPoolObjects;

    private void Awake() {
        //Instance = this;
        if (Instance != null) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        foreach (PoolInfo poolObject in listOfPoolObjects) {
            FillPool(poolObject);
        }
    }

    private void FillPool(PoolInfo poolInfo) {
        for (int i = 0; i < poolInfo.amount; i++) {
            GameObject objInstance = Instantiate(poolInfo.prefab, poolInfo.container.transform);
            objInstance.gameObject.SetActive(false);
            poolInfo.pool.Add(objInstance);
        }
    }

    public GameObject GetPoolObject(PoolObjectType type) {
        PoolInfo selectedPoolInfo = GetPoolByType(type);
        List<GameObject> pool = selectedPoolInfo.pool;

        GameObject objInstance;
        if (pool.Count > 0) {
            objInstance = pool[pool.Count - 1];
            pool.Remove(objInstance);
        }
        else {
            objInstance = Instantiate(selectedPoolInfo.prefab, selectedPoolInfo.container.transform);
        }

        return objInstance;
    }

    public void CoolObject(GameObject obj, PoolObjectType type) {
        obj.SetActive(false);

        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if (!pool.Contains(obj)) {
            pool.Add(obj);
        }
    }

    private PoolInfo GetPoolByType(PoolObjectType type) {
        foreach (PoolInfo poolObject in listOfPoolObjects) {
            if (type == poolObject.type) {
                return poolObject;
            }
        }
        return null;
    }
}
