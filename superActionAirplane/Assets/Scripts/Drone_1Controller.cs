using System.Collections;
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
    public List <Animator> anim;

    public bool unfiniteShooting = false;

    ObjectPooler objectPooler;
    PlayerController pc;

    void Start()
    {
        pc = GameManager.instance.pc;
        objectPooler = ObjectPooler.instance;
        if (shotDelay > 0 && betweenShotsDelay > 0)
            InvokeRepeating("Shooting", shotDelay, betweenShotsDelay);
    }
    void Shooting()
    {
        if (pc.isActiveAndEnabled && gameObject.activeInHierarchy && canShoot)
        {
            if (gameObject.transform.position.z > 3 && pc.lives > 0)
            {
                if (gameObject.transform.position.y < 10 && gameObject.transform.position.x < 5f && gameObject.transform.position.x > -5f) // if noBossState, but y is < 7
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
            if (anim.Count > 0)
            {
                foreach(Animator a in anim)
                {
                    if (!unfiniteShooting)
                    {
                        a.SetTrigger("Shot");
                    }
                    else
                    {
                        a.SetBool("Shot", true);
                    }
                }
            }
            Vector3 shotOrigintPos = transform.position;
            if (shotHolder)
                shotOrigintPos = shotHolder.transform.position;


            BulletController newBullet = objectPooler.SpawnBulletFromPool("EnemyBullet", shotOrigintPos, Quaternion.identity);
            newBullet.SetTarget(GameManager.instance.pc.transform, shotRandomOffset, false);
        }
        else if (bulletBurst.Count > 0) // bullet burst
        {
            StartCoroutine(ShotBulletBurst());
        }
    }

    IEnumerator ShotBulletBurst()
    {
        if (anim.Count > 0)
        {
            foreach (Animator a in anim)
            {
                a.SetTrigger("Shot");
            }
        }
        foreach (GameObject go in bulletBurst)
        {
            Vector3 shotOrigintPos = transform.position;
            if (shotHolder)
                shotOrigintPos = shotHolder.transform.position;

            BulletController newBullet = objectPooler.SpawnBulletFromPool("EnemyBullet", shotOrigintPos, Quaternion.identity);
            newBullet.SetTarget(pc.transform, shotRandomOffset, false);
            yield return new WaitForSeconds(bulletBurstDelay);
        }
    }

    private void Update()
    {
        if (GameManager.instance.pc)
            transform.LookAt(pc.transform);
    }
}