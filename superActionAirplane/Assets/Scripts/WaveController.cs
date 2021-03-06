﻿using System.Collections;
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

    public void RemoveWave()
    {
        StartCoroutine("MoveUp");
    }

    IEnumerator MoveUp()
    {
        float t = 0;

        while (t < 1)
        {
            transform.position += Vector3.up * Time.deltaTime * 30;
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}