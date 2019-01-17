using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public float movementSpeed = 100;

    public int testStartWave = 0;
    public float spawnZ = 200;

    public float solidsMinDelay = 1;
    public float solidsMaxDelay = 10;
    public float rangeX = 5;
    float currentDelay = 1;
    public float groundLevel = -6;

    public List<AdditionalWeaponController> additionalWeapons;
    public List<GameObject> solids = new List<GameObject>();
    public List<GameObject> trash = new List<GameObject>();
    public List<BuildingController> solidsOnScene = new List<BuildingController>();
    public List<GameObject> waves = new List<GameObject>();
    public List<GameObject> wavesInGame = new List<GameObject>();
    public int currentWave = 0;
    public GameObject solidsParent;
    public GameObject trashParent;
    int trashSide = -1;
    public GameObject dropBox;

    public GameObject boss;
    public bool bossState = false;

    public SkyController skyController;


    [HideInInspector]
    public WaveController currentWaveController;

    private void Start()
    {
        currentWave = testStartWave;
        GetSolids();
        if (!bossState)
        {
            GenerateSpawnList();
            Invoke("SpawnWave", 2f);
            Invoke("SpawnTrash", 0.1f);
        }
    }

    void GenerateSpawnList()
    {
        List<GameObject> tempList = new List<GameObject>(waves);
        wavesInGame.Add(tempList[0]);
        tempList.RemoveAt(0);

        for (int i = 0; i < 9; i++)
        {
            int random = Random.Range(0, tempList.Count);
            wavesInGame.Add(tempList[random]);
            tempList.RemoveAt(random);
        }
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

                Vector3 newSolidPosition = new Vector3(Random.Range(-rangeX, rangeX), groundLevel, spawnZ);
                GameObject go = GameObject.Instantiate(solids[solidIndex], newSolidPosition, Quaternion.identity);
                go.transform.SetParent(solidsParent.transform);
                currentDelay = Random.Range(solidsMinDelay, solidsMaxDelay);
            }
        }
    }

    void SpawnTrash()
    {
        Vector3 newSolidPosition;
        if (trashSide == -1)
        {
            newSolidPosition = new Vector3(Random.Range(-30f, -15f), -6.5f, spawnZ);
        }
        else
        {
            newSolidPosition = new Vector3(Random.Range(30f, 15f), -6.5f, spawnZ);
        }

        GameObject go = GameObject.Instantiate(trash[Random.Range(0, trash.Count)], newSolidPosition, Quaternion.identity);
        go.transform.SetParent(trashParent.transform);
        trashSide *= -1;
        Invoke("SpawnTrash", .1f);
    }

    public void WaveDestroyed(WaveController wave)
    {
        int newWaveIndex = currentWave + 1;
        if (wavesInGame.Count > newWaveIndex)
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
            if (solid.hideSolidController)
                solid.hideSolidController.HideSolid();
        }
    }

    void SpawnWave()
    {
        GameObject newWave = GameObject.Instantiate(wavesInGame[currentWave], Vector3.zero, Quaternion.identity);
        currentWaveController = newWave.GetComponent<WaveController>();
    }
}