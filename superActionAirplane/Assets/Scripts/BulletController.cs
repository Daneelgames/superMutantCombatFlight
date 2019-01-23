using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float delayNextShotTime = 0.75f;
    public float speed = 30;
    public float lifeTime = 1;
    public Rigidbody rb;

    Transform target = null;
    Vector3 targetPosition = Vector3.zero;
    public bool rocket = false;
    bool autoAim = false;

    public ParticleSystem particles;
    ParticleSystem.EmissionModule particlesEmissionModule;

    private void Start()
    {
        if (particles)
        {
            particlesEmissionModule = particles.emission;
        }
    }

    void CheckPositionZ()
    {
        if(transform.position.z < -1 )
        {
            particlesEmissionModule.enabled = false;
            CancelInvoke("CheckPositionZ");
            Invoke("DestroyBullet", 2);
        }
    }

    public void SetRocket(bool active)
    {
        rocket = active;
    }

    public void SetTarget(Transform _target, Vector3 offset, bool _autoAim)
    {
        autoAim = _autoAim;
        Vector3 newShotTarget = Vector3.zero;
        if (offset != Vector3.zero)
        {
             newShotTarget = new Vector3(_target.position.x + Random.Range(-offset.x, offset.x),
                                         _target.position.y + Random.Range(-offset.y, offset.y),
                                         _target.position.z);

            transform.LookAt(newShotTarget);
        }
        else
        {
            transform.LookAt(_target);
        }
        target = _target;
        rb.velocity = transform.forward * speed;

        if (gameObject.layer == 11) // if enemy bullet
            InvokeRepeating("CheckPositionZ", 0.1f, 0.1f);
        else
            Invoke("DestroyBullet", lifeTime);
    }

    public void DestroyBullet()
    {
        if (particles)
            ResetColor();
        StopAllCoroutines();
        Invoke("SetInactive", 2f);
    }

    void SetInactive()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (rocket && target)
        {
            if (autoAim && target.gameObject.activeInHierarchy)
            {
                if (target != null)
                    transform.LookAt(target.position);

            }
            rb.velocity = transform.forward * speed;
        }
    }

    public void ResetColor()
    {
        particlesEmissionModule.enabled = true;
    }
}