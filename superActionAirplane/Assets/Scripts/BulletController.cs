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
    public bool rocket = false;
    bool autoAim = false;

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
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (target == null)
        {
            target = GameManager.instance.pc.target.transform;
        }
        if (rocket && target)
        {
            if (autoAim && target.gameObject.activeInHierarchy)
                transform.LookAt(target.position);
            rb.velocity = transform.forward * speed;
        }
    }
}