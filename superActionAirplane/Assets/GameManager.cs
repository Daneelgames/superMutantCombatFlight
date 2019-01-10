using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool test = false;

    public PlayerController pc;
    public SpawnerController spawnerController;
    public TouchInputController touchInputController;
    public UiLivesController uiLivesController;
    public Transform projectileContainer;

    void Awake()
    {
        Application.targetFrameRate = 60;

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator Restart()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GetLinks(null);
    }

    private void Start()
    {
  //      GetLinks(null);
    }

    public void GetLinks(PlayerController _pc)
    {
        if (_pc)
            pc = _pc;
        if (!test)
        spawnerController = GameObject.Find("Spawner").GetComponent<SpawnerController>();
        uiLivesController = GameObject.Find("HeartsController").GetComponent<UiLivesController>();
        projectileContainer = GameObject.Find("ProjectilesContainer").transform;
    }
}