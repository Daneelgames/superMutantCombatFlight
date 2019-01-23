using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaveController : MonoBehaviour
{
    public Animator anim;
    SpawnerController spawner;

    float x;

    void Start()
    {
        spawner = GameManager.instance.spawnerController;
        anim.Play(0, 0, Random.Range(0f, 1f));

        x = transform.position.x;

        InvokeRepeating("CheckPositionZ", 1, 0.1f);
    }

    private void Update()
    {
        transform.position = new Vector3(x, 0, transform.position.z - Time.deltaTime * spawner.movementSpeed);
    }

    void CheckPositionZ()
    {
        if (transform.position.z < 10)
        {
            transform.position = new Vector3(Random.Range(-10f, 10f), 0, 500);
        }
    }
}