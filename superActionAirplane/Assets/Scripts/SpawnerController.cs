using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public int testStartWave = 0;

    public float solidsMinDelay = 1;
    public float solidsMaxDelay = 10;
    public float rangeX = 5;
    float currentDelay = 1;
    public float groundLevel = -6;

    public List<GameObject> solids = new List<GameObject>();
    public List<BuildingController> solidsOnScene = new List<BuildingController>();
    public List<GameObject> waves = new List<GameObject>();
    int currentWave = 0;
    public GameObject solidsParent;

    public GameObject boss;
    public bool bossState = false;

    [HideInInspector]
    public WaveController currentWaveController;

    private void Start()
    {
        currentWave = testStartWave;
        GetSolids();
        if (!bossState)
            Invoke("SpawnWave", 2f);
    }

    void GetSolids()
    {
        solidsParent = GameObject.Find("Solids");
    }

    public void AddSolidOnScene (BuildingController newSolid)
    {
        solidsOnScene.Add(newSolid);
    }
    public void RemoveSolid (BuildingController buildingController)
    {
        solidsOnScene.Remove(buildingController);
    }

    private void Update()
    {
        SpawnSolids();
    }

    void SpawnSolids()
    {
        if (!bossState)
        {
            if (currentDelay > 0)
            {
                currentDelay -= Time.deltaTime;
            }
            else
            {
                int solidIndex = Random.Range(0, solids.Count);

                if (!solidsParent)
                    GetSolids();

                Vector3 newSolidPosition = new Vector3(Random.Range(-rangeX, rangeX), groundLevel, 2000);
                GameObject go = GameObject.Instantiate(solids[solidIndex], newSolidPosition, Quaternion.identity);
                go.transform.SetParent(solidsParent.transform);
                currentDelay = Random.Range(solidsMinDelay, solidsMaxDelay);
            }
        }
    }

    public void WaveDestroyed(WaveController wave)
    {
        int newWaveIndex = currentWave + 1;
        if (waves.Count > newWaveIndex)
        {
            currentWave = newWaveIndex;
            SpawnWave();
        }
        else // if waves is over, spawn boss
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        bossState = true;
        GameObject.Instantiate(boss, Vector3.zero, Quaternion.identity);
        Invoke("HideSolids", 1);
    }

    void HideSolids()
    {
        foreach (BuildingController solid in solidsOnScene)
        {
            solid.hideSolidController.HideSolid();
        }
    }

    void SpawnWave()
    {
        GameObject newWave = GameObject.Instantiate(waves[currentWave], Vector3.zero, Quaternion.identity);
        currentWaveController = newWave.GetComponent<WaveController>();
    }
}