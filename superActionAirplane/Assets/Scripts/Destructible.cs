﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Destructible : MonoBehaviour
{
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

    private void Start()
    {
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
                    GameManager.instance.pc.aimAssist.RemoveDeadEnemy(gameObject);
                    Destroy(dropBoxController.gameObject);
                }
            }
        }
    }

    public void Damage(float damage)
    {
        if (!invincible)
        {
            health -= damage;

            if (damageFeedbackAnimator)
                damageFeedbackAnimator.SetTrigger("Damage");

            if (health > 0)
                objectPooler.SpawnGameObjectFromPool(smallExplosion.name, transform.position, Quaternion.identity);
            else // if object has no health
            {
                invincible = true;
                objectPooler.SpawnGameObjectFromPool(explosion.name, transform.position, Quaternion.identity);

                CameraShaker.Instance.ShakeOnce(6, 6, 0.1f, 1);
                if (waveController) //if gameObject is an enemy
                {
                    Drop();
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
                    Destroy(dropBoxController.gameObject);
                }
            }
        }
    }

    void Destroyed()
    {
        GameManager.instance.pc.aimAssist.RemoveDeadEnemy(gameObject);
        gameObject.SetActive(false);
    }

    public void SetInvincible(bool _true)
    {
        invincible = _true;
    }

    void Drop()
    {
        if (dropBoxController == null) // drop from enemy
        {
            if (GameManager.instance.spawnerController.currentWave == 0)
            {
                DropDropBox();
            }
            else if (!GameManager.instance.spawnerController.dropBoxOnScene && GameManager.instance.spawnerController.dropBoxDelay <= 0)
            {
                float randomDrop = Random.Range(0f, 100f);
                if (randomDrop > GameManager.instance.spawnerController.dropRate)
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

    void DropDropBox()
    {
        if (GameManager.instance.pc.isActiveAndEnabled)
        {
            GameObject drop = GameObject.Instantiate(GameManager.instance.spawnerController.dropBox, transform.position, Quaternion.identity);
        }
    }
}