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

    void Start()
    {
        InvokeRepeating("Shooting", shotDelay, betweenShotsDelay);
    }
    void Shooting()
    {
        if (gameObject.activeInHierarchy && canShoot)
        {
            if (gameObject.transform.position.z > 3 && GameManager.instance.pc.lives > 0)
            {
                if (!GameManager.instance.spawnerController.bossState)
                {
                    if (gameObject.transform.position.y < 7) // if noBossState, but y is < 7
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
            GameObject newBullet = GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<BulletController>().SetTarget(GameManager.instance.pc.transform, shotRandomOffset, false);
        }
        else if (bulletBurst.Count > 0) // bullet burst
        {
            StartCoroutine(ShotBulletBurst());
        }

        /*
        Transform playerTransform = GameManager.instance.pc.transform;
        RaycastHit hit;

        bool playerIsOnSight = true;
        if (Physics.Raycast(transform.position, GameManager.instance.pc.transform.position, out hit, Vector3.Distance(transform.position, playerTransform.position), layerMask))
        {
            if (hit.collider.tag == "Enemies" || hit.collider.tag == "Solids")
            {
                if (Vector3.Distance(transform.position, playerTransform.position) > Vector3.Distance(transform.position, hit.collider.transform.position)) // if hit is closer than player
                    playerIsOnSight = false;
            }
        }

        if (playerIsOnSight)
        {
            GameObject newBullet = GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<BulletController>().SetTarget(playerTransform);
        }
        */
    }


    IEnumerator ShotBulletBurst()
    {
        foreach (GameObject go in bulletBurst)
        {
            GameObject newBullet = GameObject.Instantiate(go, transform.position, Quaternion.identity);
            BulletController _bulletController = newBullet.GetComponent<BulletController>();
            //_bulletController.SetTarget(target.transform.parent, Vector3.zero);
            _bulletController.SetTarget(GameManager.instance.pc.transform, shotRandomOffset, false);
            yield return new WaitForSeconds(bulletBurstDelay);
        }
    }

}