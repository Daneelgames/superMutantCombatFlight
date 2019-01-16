using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
    }

    #region Singleton
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<BulletController>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<BulletController>>();

        foreach (Pool pool in pools)
        {
            Queue<BulletController> objectPool = new Queue<BulletController>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj =  Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj.GetComponent<BulletController>());
                obj.transform.SetParent(pool.parent);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public BulletController SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }

        BulletController objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.gameObject.transform.position = position;
        objectToSpawn.gameObject.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}