using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public bool infiniteMode = false;
    public float movementSpeed = 100;
    float originalMovementSpeed = 100;
    int sharksOnScene = 0;

    public int testStartWave = 0;
    public float spawnZ = 200;

    public float solidsMinDelay = 1;
    public float solidsMaxDelay = 10;
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
    public float dropBoxDelay = 0;

    public GameObject boss;
    public bool bossState = false;

    public SkyController skyController;

    [HideInInspector]
    public WaveController currentWaveController;
    [HideInInspector]
    public bool dropBoxOnScene;

    [Header("0 to 100. When 0, will drop every time. Default is 66")]
    public float dropRate = 66;

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

                Vector3 newSolidPosition = new Vector3(0, groundLevel, spawnZ);
                GameObject go = GameObject.Instantiate(solids[solidIndex], newSolidPosition, Quaternion.identity);
                go.transform.SetParent(solidsParent.transform);

                switch(sharksOnScene)
                {
                    case 0:
                        currentDelay = Random.Range(solidsMinDelay, solidsMaxDelay);
                        break;
                    case 1:
                        currentDelay = 3;
                        break;
                    case 2:
                        currentDelay = 2f;
                        break;
                    case 3:
                        currentDelay = 1f;
                        break;
                }
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
        if (sharksOnScene > 0)
            Invoke("SpawnTrash", 0.05f);
        else
            Invoke("SpawnTrash", 0.1f);
    }

    public void WaveDestroyed(WaveController wave)
    {
        int newWaveIndex = currentWave + 1;
        if (wavesInGame.Count > newWaveIndex)
        {
            currentWave = newWaveIndex;
            SpawnWave();
        }
        else if (!infiniteMode) // if waves is over, spawn boss
        {
            SpawnBoss();
        }
        else if (infiniteMode)
        {
            GenerateSpawnList();
            currentWave = 1;
            SpawnWave();
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
    public void ToggleDropBoxOnScene(bool onScene)
    {
        dropBoxOnScene = onScene;
        StartCoroutine("DropBoxDelay");
    }

    IEnumerator DropBoxDelay()
    {
        dropBoxDelay = 5;
        while (dropBoxDelay > 0)
        {
            dropBoxDelay -= Time.deltaTime;
            yield return null;
        }
    }

    public void SetShark(int amount)
    {
        sharksOnScene += amount;
        if (sharksOnScene < 0)
            sharksOnScene = 0;

        movementSpeed = originalMovementSpeed + 33 * sharksOnScene;
    }
}