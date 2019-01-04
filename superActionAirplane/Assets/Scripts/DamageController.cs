using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public float damage = 1;
    public GameObject explosion;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "AimAssist")
        {
            if (gameObject.layer == 10) // player's bullet
            {
                if (other.gameObject.tag == "Solids" || other.gameObject.tag == "Enemies" || (other.gameObject.tag == "Drop" && other.gameObject.layer == 12))
                {
                    DamageOther(other);
                    DestroyBullet();
                }
            }
            else if (gameObject.layer == 11) // if enemy's bullet
            {
                if (other.gameObject.name == "Player") // bullet hit player
                {
                    if (gameObject.tag == "Enemies" || gameObject.tag == "Projectiles")
                    {
                        GameManager.instance.pc.Damage();
                        DestroyBullet();
                    }
                }
                else if (other.gameObject.tag == "Solids" || other.gameObject.tag == "Enemies") // bullet hit player
                {
                    DestroyBullet();
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

    void DestroyBullet()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}