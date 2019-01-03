using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    //public List<GameObject> enemies = new List<GameObject>();
    int enemyCount = 0;

    public void AddEnemy(GameObject go)
    {
        enemyCount += 1;
    //    enemies.Add(go);
    }

    public void EnemyDestroyed(GameObject go)
    {
        enemyCount -= 1;
    //    enemies.Remove(go);

        if (enemyCount == 0)
        {
            GameManager.instance.spawnerController.WaveDestroyed(this);
            Destroy(gameObject);
        }
    }
}