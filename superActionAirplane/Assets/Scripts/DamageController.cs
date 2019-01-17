using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public float damage = 1;
    public GameObject explosion;
    ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "AimAssist")
        {
            if (gameObject.layer == 10) // player's bullet
            {
                if (other.gameObject.tag == "Solids" || other.gameObject.tag == "Enemies" || (other.gameObject.tag == "Drop" && other.gameObject.layer == 12))
                {
                    DamageOther(other);
                    DestoyBullet();
                }
            }
            else if (gameObject.layer == 11) // if enemy's bullet
            {
                if (other.gameObject.name == "Player") // bullet hit player
                {
                    if (gameObject.tag == "Enemies" || gameObject.tag == "Projectiles")
                    {
                        GameManager.instance.pc.Damage();
                        DestoyBullet();
                    }
                }
                else if (other.gameObject.tag == "Solids" || other.gameObject.tag == "Enemies") // bullet hit player
                {
                    DestoyBullet();
                }
            }
        }
    }

    void DamageOther(Collider other)
    {
        Destructible destructible = other.gameObject.GetComponent<Destructible>();
        if (destructible)
            destructible.Damage(damage);
    }

    void DestoyBullet()
    {
        objectPooler.SpawnGameObjectFromPool(explosion.name, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}