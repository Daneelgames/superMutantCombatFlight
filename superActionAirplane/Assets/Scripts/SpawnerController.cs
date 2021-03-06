﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public float movementSpeed = 150;
    float originalMovementSpeed = 150;
    int sharksOnScene = 0;

    public int testStartWave = 0;
    public float spawnZ = 200;

    public float solidsDelay = 5;
    float currentDelay = 1;
    public float groundLevel = -6;

    public List<AdditionalWeaponController> additionalWeapons;
    public List<GameObject> solids = new List<GameObject>();
    //public List<GameObject> trash = new List<GameObject>();
    public List<BuildingController> solidsOnScene = new List<BuildingController>();
    public List<GameObject> wavesEasy = new List<GameObject>();
    public List<GameObject> waves = new List<GameObject>();
    public List<GameObject> wavesInGame = new List<GameObject>();
    [SerializeField]
    List<GameObject> tempList;
    public int currentWave = 0;
    public GameObject solidsParent;
    //public GameObject trashParent;
   // int trashSide = -1;
    public GameObject dropBox;
    public float dropBoxDelay = 0;

    public SkyController skyController;

    [HideInInspector]
    public WaveController currentWaveController;
    [HideInInspector]
    public DropBoxController dropBoxOnScene;

    [Header("0 to 100. When 0, will drop every time. Default is 66")]
    public float dropRate = 66;

    bool canSpawn = true;
    [SerializeField]
    bool tutorial = true;       

    public void StartSpawning()
    {
        currentWave = testStartWave;
        GetSolids();

        GenerateSpawnList();
        canSpawn = true;

        //StartCoroutine(GameManager.instance.menuController.GuiActorPlay("FirstWave", "Player"));
        SpawnWave();
        Invoke("SpawnSolids", 5);
    }

    public void StopSpawning()
    {
        canSpawn = false;
        CancelInvoke();

        foreach (BuildingController b in solidsOnScene)
        {
            b.Wane();
        }
    }

    void GenerateSpawnList()
    {
        currentWave = 0;
        int maxWaves = 2;

        if (tutorial)
        {
            tempList = new List<GameObject>(wavesEasy);
        }
        else
        {
            wavesInGame.Clear();
            tempList.Clear();
            maxWaves = 9;
            tempList  = new List<GameObject>(waves);
        }
        wavesInGame.Add(tempList[0]);
        tempList.RemoveAt(0);

        for (int i = 0; i < maxWaves; i++)
        {
            int random = Random.Range(0, tempList.Count);
            wavesInGame.Add(tempList[random]);
            tempList.RemoveAt(random);
        }
    }

    public void ClearWaves()
    {
        if (currentWaveController)
        {
            currentWaveController.RemoveWave();
            wavesInGame.Clear();
            if (dropBoxOnScene)
                dropBoxOnScene.RemoveDropBox();

            tempList.Clear();

            tutorial = true;
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

    void SpawnSolids()
    {
        if (canSpawn)
        {
            int solidIndex = Random.Range(0, solids.Count);

            if (!solidsParent)
                GetSolids();

            Vector3 newSolidPosition = new Vector3(Random.Range(-2f, 2f), groundLevel, spawnZ);
            GameObject go = Instantiate(solids[solidIndex], newSolidPosition, Quaternion.identity);
            go.transform.SetParent(solidsParent.transform);
            

            
            if (tutorial)
                currentDelay = solidsDelay * 2;
            else
            {
                switch (sharksOnScene)
                {
                    case 1:
                        currentDelay = 2;
                        break;
                    case 2:
                        currentDelay = 1.25f;
                        break;
                    case 3:
                        currentDelay = 0.75f;
                        break;
                    case 0:
                        currentDelay = 5;
                        break;
                }
            }
            Invoke("SpawnSolids", currentDelay);
        }
    }

    public void WaveDestroyed(WaveController wave)
    {
        int newWaveIndex = currentWave + 1;

        if (GameManager.instance.playerAlive)
        {
            //StartCoroutine(GameManager.instance.menuController.GuiActorPlay("WaveDestroyed", "Player"));

            if (wavesInGame.Count > newWaveIndex)
            {
                currentWave = newWaveIndex;
                SpawnWave();
            }
            else
            {
                if (tutorial)
                    tutorial = false;

                GenerateSpawnList();
                currentWave = 1;
                SpawnWave();
            }
        }
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
        GameObject newWave = Instantiate(wavesInGame[currentWave], Vector3.zero, Quaternion.identity);
        currentWaveController = newWave.GetComponent<WaveController>();
    }
    public void ToggleDropBoxOnScene(DropBoxController box)
    {
        dropBoxOnScene = box;
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