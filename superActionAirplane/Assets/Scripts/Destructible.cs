using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float health = 5;
    public GameObject smallExplosion;
    public GameObject explosion;
    public WaveController waveController;
    public BossController bossController;
    
    public DropBoxController dropBox; // if dropbox != null, GO is dropBox

    public bool canBeDamagedBySolids = true;
    bool invincible = false;

    private void Start()
    {
        if (waveController)
        waveController.AddEnemy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "AimAssist")
        {
            if (gameObject.layer == 11) //if enemy crashes
            {
                if (canBeDamagedBySolids && other.gameObject.tag == "Solids" && other.gameObject.layer == 9)
                    Damage(1);
            }
        }
    }

    public void Damage(float damage)
    {
        if (!invincible)
        {

            health -= damage;
            if (health > 0)
                Instantiate(smallExplosion, transform.position, Quaternion.identity);
            else // if object has no health
            {
                Instantiate(explosion, transform.position, Quaternion.identity);

                if (waveController) //if gameObject is an enemy
                {
                    waveController.EnemyDestroyed(gameObject);
                    Destroyed();
                }
                else if (bossController)
                {
                    bossController.PieceDestroyed(this);
                    Destroyed();
                }
                else // dropBox
                {
                    Drop();
                    GameManager.instance.pc.aimAssist.RemoveDeadEnemy(gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
    void Destroyed()
    {
        Drop();
        GameManager.instance.pc.aimAssist.RemoveDeadEnemy(gameObject);
        gameObject.SetActive(false);
    }

    public void SetInvincible(bool _true)
    {
        invincible = _true;
    }

    void Drop()
    {
        if (dropBox == null) // drop from enemy
        {
            float randomDrop = Random.Range(0f, 100f);
            if (randomDrop > 75f)
            {

                DropDropBox();
            }
        }
        else // drop from dropBox
        {
            if (dropBox.type == 0)
            {
                dropBox.DropHealth();
            }
            else if (dropBox.type == 1)
            {
                dropBox.DropWeapon();
            }
        }
    }

    void DropDropBox()
    {
        GameObject drop = GameObject.Instantiate(GameManager.instance.spawnerController.dropBox, transform.position, Quaternion.identity);

        float random = Random.Range(0f, 100f);
        if (random > 35 * GameManager.instance.pc.lives) // if player are low on health
        {
            drop.GetComponent<DropBoxController>().SetType(0); // health
        }
        else
        {
            drop.GetComponent<DropBoxController>().SetType(1); // weapon
        }
    }
}