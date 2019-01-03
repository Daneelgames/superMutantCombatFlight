using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeMarkerController : MonoBehaviour
{
    public Transform enemyTarget;
    bool targeting = false;

    public void SetTarget(Transform _target)
    {
        transform.SetParent(null);
        enemyTarget = _target;
        targeting = true;
    }

    private void Update()
    {
        if (targeting)
        transform.position = Vector3.Lerp(transform.position, enemyTarget.transform.position, 0.9f * Time.deltaTime * 10);
    }

    public void Shot()
    {
        targeting = false;
        transform.SetParent(GameManager.instance.pc.target.transform);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
