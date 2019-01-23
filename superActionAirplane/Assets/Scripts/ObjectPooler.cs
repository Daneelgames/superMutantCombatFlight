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

    public List<Pool> poolsBullets;
    public List<Pool> poolsObjects;
    public Dictionary<string, Queue<BulletController>> poolDictionaryBullets;
    public Dictionary<string, Queue<GameObject>> poolDictionaryObjects;

    List<BulletController> bullets = new List<BulletController>();

    private void Start()
    {
        CreateBullets();
        CreateObjects();
    }

    void CreateBullets()
    {
        poolDictionaryBullets = new Dictionary<string, Queue<BulletController>>();

        foreach (Pool pool in poolsBullets)
        {
            Queue<BulletController> objectPoolBullets = new Queue<BulletController>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                BulletController objController = obj.GetComponent<BulletController>();
                objectPoolBullets.Enqueue(objController);
                obj.transform.SetParent(pool.parent);
                obj.SetActive(false);

                bullets.Add(objController);
            }
            poolDictionaryBullets.Add(pool.tag, objectPoolBullets);
        }
    }

    void CreateObjects()
    {
        poolDictionaryObjects = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in poolsObjects)
        {
            Queue<GameObject> objectPoolObjects = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPoolObjects.Enqueue(obj);
                obj.transform.SetParent(pool.parent);
            }
            poolDictionaryObjects.Add(pool.tag, objectPoolObjects);
        }
    }

    public BulletController SpawnBulletFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionaryBullets.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }

        BulletController objectToSpawn = poolDictionaryBullets[tag].Dequeue();

        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.gameObject.transform.position = position;
        objectToSpawn.gameObject.transform.rotation = rotation;

        poolDictionaryBullets[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnGameObjectFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionaryObjects.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionaryObjects[tag].Dequeue();

        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.gameObject.transform.position = position;
        objectToSpawn.gameObject.transform.rotation = rotation;

        poolDictionaryObjects[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void DisableAllProjectiles()
    {
        foreach(BulletController b in bullets)
        {
            if (b.isActiveAndEnabled)
                b.DestroyBullet();
        }
    }
}