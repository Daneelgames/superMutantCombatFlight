using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetController : MonoBehaviour
{
    public GameObject targetCrosshair;
    public float speed = 1;
    float groundLevel = -6;
    public ChargeMarkerController chargeMarker;

    private void Awake()
    {
        transform.SetParent(null);
    }

    private void Start()
    {
        groundLevel = GameManager.instance.spawnerController.groundLevel;
    }

    private void Update()
    {
        Vector3 newPos;
        if ( GameManager.instance.pc.aimAssist.currentTargetTransform != null) // if enemy is in range
        {
            newPos = Vector3.Lerp(transform.position, GameManager.instance.pc.aimAssist.currentTargetTransform.position, 0.9f * Time.deltaTime * speed);
            if (GameManager.instance.pc.currentChargeTime >= GameManager.instance.pc.chargeTime) // if charge is ready
            {
                if (!chargeMarker.gameObject.activeInHierarchy)
                {
                    chargeMarker.gameObject.SetActive(true);
                    chargeMarker.SetTarget(GameManager.instance.pc.aimAssist.currentTargetTransform);
                }
            }
        }
        else
        {
            newPos = Vector3.Lerp(transform.position, targetCrosshair.transform.position, 0.9f * Time.deltaTime * speed);
        }
            
        transform.position = newPos;

        if (transform.position.y < groundLevel)
            transform.position = new Vector3(transform.position.x, groundLevel, transform.position.z);
    }
}