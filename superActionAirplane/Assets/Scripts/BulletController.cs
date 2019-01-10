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
    Vector3 newShotTarget;

    private void Start()
    {
        transform.SetParent(GameManager.instance.projectileContainer);
    }

    public void SetRocket(bool active)
    {
        rocket = active;
    }

    public void SetTarget(Transform _target, Vector3 offset, bool _autoAim)
    {
        autoAim = _autoAim;
        newShotTarget = _target.localPosition;
        if (offset != Vector3.zero)
        {
             newShotTarget = new Vector3(_target.localPosition.x + Random.Range(-offset.x, offset.x),
                                         _target.localPosition.y + Random.Range(-offset.y, offset.y),
                                         _target.localPosition.z);
        }
        //transform.LookAt(newShotTarget);
        transform.LookAt(_target.position);
        target = _target;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (rocket && target)
        {
            if (autoAim && target.gameObject.activeInHierarchy)
            {
                rb.transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.localPosition, Time.deltaTime * speed);
            }
            else
                rb.transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
        }
        else
        {
            rb.transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            //rb.transform.localPosition = Vector3.MoveTowards(transform.localPosition, newShotTarget, Time.deltaTime * speed);
        }
    }
}