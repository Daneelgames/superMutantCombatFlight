﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_1Controller : MonoBehaviour
{
    public Vector2 shotRandomOffset;
    public bool canShoot = true;
    public GameObject bullet;
    public List<GameObject> bulletBurst;
    public float shotDelay = 1;
    public float betweenShotsDelay = 1.5f;
    public float bulletBurstDelay = 0.1f;

    public LayerMask layerMask;

    public GameObject shotHolder;
    public Animator anim;

    void Start()
    {
        InvokeRepeating("Shooting", shotDelay, betweenShotsDelay);
    }
    void Shooting()
    {
        if (gameObject.activeInHierarchy && canShoot)
        {
            if (gameObject.transform.localPosition.z > 3 && GameManager.instance.pc.lives > 0)
            {
                if (!GameManager.instance.spawnerController.bossState)
                {
                    if (gameObject.transform.localPosition.y < 7) // if noBossState, but y is < 7
                    {
                        Shot();
                    }
                }
                else // if bossState
                {
                    Shot();
                }
            }
        }
    }

    public void SetCanShoot(bool can)
    {
        canShoot = can;
        if (!can)
            CancelInvoke("Shooting");
        else
            InvokeRepeating("Shooting", shotDelay, betweenShotsDelay);
    }

    void Shot()
    {
        if (bullet) // single bullet
        {
            if (anim)
                anim.SetTrigger("Shot");

            Vector3 shotOrigintPos = transform.position;
            if (shotHolder)
                shotOrigintPos = shotHolder.transform.position;

            GameObject newBullet = GameObject.Instantiate(bullet, shotOrigintPos, Quaternion.identity);
            newBullet.GetComponent<BulletController>().SetTarget(GameManager.instance.pc.transform, shotRandomOffset, false);
        }
        else if (bulletBurst.Count > 0) // bullet burst
        {
            StartCoroutine(ShotBulletBurst());
        }
    }

    IEnumerator ShotBulletBurst()
    {
        foreach (GameObject go in bulletBurst)
        {
            Vector3 shotOrigintPos = transform.position;
            if (shotHolder)
                shotOrigintPos = shotHolder.transform.position;

            GameObject newBullet = GameObject.Instantiate(go, shotOrigintPos, Quaternion.identity);
            BulletController _bulletController = newBullet.GetComponent<BulletController>();
            //_bulletController.SetTarget(target.transform.parent, Vector3.zero);
            _bulletController.SetTarget(GameManager.instance.pc.transform, shotRandomOffset, false);
            yield return new WaitForSeconds(bulletBurstDelay);
        }
    }

    private void Update()
    {
        if (GameManager.instance.pc)
            transform.LookAt(GameManager.instance.pc.transform);
    }
}