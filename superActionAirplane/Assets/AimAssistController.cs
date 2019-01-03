using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistController : MonoBehaviour
{
    List<Transform> targetsInRange = new List<Transform>();
    [HideInInspector]
    public Transform currentTargetTransform;
    public Transform pcTransform;

    void Start()
    {
        transform.SetParent(null);
        InvokeRepeating("CheckClosestTarget", 0.1f, 0.1f);
    }

    private void Update()
    {
        transform.position = pcTransform.position;
        transform.rotation = pcTransform.rotation;
    }

    void CheckClosestTarget()
    {
        /*
        WaveController _waveController = GameManager.instance.spawnerController.currentWaveController;
        if (_waveController && _waveController.enemies.Count > 0)
        {
            float minDistance = 100;
            GameObject closestGO = null;
            foreach (GameObject go in _waveController.enemies)
            {
                float distanceToGo = Vector3.Distance(go.transform.position, GameManager.instance.pc.transform.position);
                if (distanceToGo < minDistance)
                {
                    closestGO = go;
                    minDistance = distanceToGo;
                }
            }
            if (closestGO.transform.position.z > 1 && Vector3.Distance(GameManager.instance.pc.target.transform.position, closestGO.transform.position) < 1)
            {
                currentTargetTransform = closestGO.transform;
            }
        }
        */

        if (targetsInRange.Count > 0)
        {
            float minDistance = 100;
            foreach (Transform target in targetsInRange)
            {
                if (target.gameObject.activeInHierarchy)
                {
                    float newDistance = Vector3.Distance(target.transform.position, GameManager.instance.pc.transform.position);
                    if (newDistance < minDistance)
                    {
                        minDistance = newDistance;
                        currentTargetTransform = target;
                    }
                }
            }
        }
        else
            currentTargetTransform = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && other.gameObject.tag == "Enemies")
            targetsInRange.Add(other.gameObject.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11 && other.gameObject.tag == "Enemies")
        {
            if (targetsInRange.Contains(other.transform))
                targetsInRange.Remove(other.gameObject.transform);
        }
    }

    public void RemoveDeadEnemy(GameObject enemy)
    {
        foreach(Transform targetTransform in targetsInRange)
        {
            if (enemy.transform == targetTransform)
            {
                targetsInRange.Remove(enemy.transform);
                break;
            }
        }
    }
}