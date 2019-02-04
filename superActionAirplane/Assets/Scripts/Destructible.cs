using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Destructible : MonoBehaviour
{
    public int coinDrop = 1;

    public float health = 5;
    public GameObject smallExplosion;
    public GameObject explosion;
    public WaveController waveController;
    public BossController bossController;
    
    public DropBoxController dropBoxController; // if dropBoxController != null, GO is dropBox

    public bool canBeDamagedBySolids = true;
    bool invincible = false;
    public Animator damageFeedbackAnimator;


    ObjectPooler objectPooler;
    GameManager gameManager;

    bool dead = false;

    private void Start()
    {
        gameManager = GameManager.instance;
        objectPooler = ObjectPooler.instance;

        if (waveController)
            waveController.AddEnemy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "AimAssist" && other.gameObject.tag != "Projectiles")
        {
            if (gameObject.layer == 11) //if enemy crashes
            {
                if (!dropBoxController)
                {
                    if (canBeDamagedBySolids && other.gameObject.tag == "Solids" && other.gameObject.layer == 9)
                        Damage(1);
                }
                else
                {
                    objectPooler.SpawnGameObjectFromPool(explosion.name, transform.position, Quaternion.identity);
                    gameManager.pc.aimAssist.RemoveDeadEnemy(gameObject);
                    Destroy(dropBoxController.gameObject);
                }
            }
        }
    }

    public void Damage(float damage)
    {
        if (!invincible && !dead && GameManager.instance.playerAlive)
        {
            health -= damage;

            if (damageFeedbackAnimator)
            {
                damageFeedbackAnimator.SetTrigger("Damage");
            }

            if (health > 0)
            {
                objectPooler.SpawnGameObjectFromPool(smallExplosion.name, transform.position, Quaternion.identity);
            }
            else // if object has no health
            {
                invincible = true;
                objectPooler.SpawnGameObjectFromPool(explosion.name, transform.position, Quaternion.identity);

                CameraShaker.Instance.ShakeOnce(6, 8, 0.1f, 1);

                if (!waveController && !bossController) // dropBox
                {
                    Drop();
                    gameManager.pc.aimAssist.RemoveDeadEnemy(gameObject);
                    Destroy(dropBoxController.gameObject);
                }
                else
                {
                    StartCoroutine(Destroyed());
                }
            }
        }
    }

    IEnumerator Destroyed()
    {
        dead = true;
        Time.timeScale = 0.5f;

        if (coinDrop > 0)
            DropCoins();

        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1;
        gameManager.pc.aimAssist.RemoveDeadEnemy(gameObject);
        dead = false;

        if (bossController)
            bossController.PieceDestroyed(this);
        else if (waveController)
        {
            Drop();
            waveController.EnemyDestroyed(gameObject);
        }

        gameObject.SetActive(false);
    }

    void DropCoins()
    {
        gameManager.AddCoins(coinDrop);
        /*
        for (int i = 0; i < coinDrop; i ++)
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);

            objectPooler.SpawnGameObjectFromPool("Coin", transform.position + new Vector3(x,y,z), Quaternion.identity);
        }
        */
    }

    public void SetInvincible(bool _true)
    {
        invincible = _true;
    }

    void Drop()
    {
        if (GameManager.instance.playerAlive)
        {
            if (dropBoxController == null) // drop from enemy
            {
                if (gameManager.spawnerController.currentWave == 0)
                {
                    DropDropBox();
                }
                else if (!gameManager.spawnerController.dropBoxOnScene && GameManager.instance.spawnerController.dropBoxDelay <= 0)
                {
                    float randomDrop = Random.Range(0f, 100f);
                    if (randomDrop > gameManager.spawnerController.dropRate)
                    {
                        DropDropBox();
                    }
                }
            }
            else // drop from dropBox
            {
                //print(dropBoxController.gameObject.name + " is dropped a weapon");
                dropBoxController.DropWeapon();
            }
        }
    }

    void DropDropBox()
    {
        if (gameManager.pc.isActiveAndEnabled)
        {
            GameObject drop = Instantiate(gameManager.spawnerController.dropBox, transform.position, Quaternion.identity);
        }
    }
}