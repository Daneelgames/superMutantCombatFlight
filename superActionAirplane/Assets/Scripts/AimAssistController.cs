using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistController : MonoBehaviour
{
    List<Transform> targetsInRange = new List<Transform>();
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
        //transform.rotation = pcTransform.rotation;
    }

    void CheckClosestTarget()
    {
        DeleteMissings();

        if (targetsInRange.Count > 0)
        {
            float minDistance = 100;
            for (int i = targetsInRange.Count - 1; i >= 0; i--)
            {
                if (targetsInRange[i] != null && targetsInRange[i].gameObject.activeSelf && targetsInRange[i].position.z > 0)
                {
                    float newDistance = Vector3.Distance(targetsInRange[i].transform.position, GameManager.instance.pc.transform.position);
                    if (newDistance < minDistance)
                    {
                        minDistance = newDistance;
                        currentTargetTransform = targetsInRange[i];
                    }
                }
                else
                {
                    targetsInRange.RemoveAt(i);
                }
            }
            /*
            foreach (Transform target in targetsInRange)
            {
                if (target != null && target.gameObject.activeSelf)
                {
                    float newDistance = Vector3.Distance(target.transform.position, GameManager.instance.pc.transform.position);
                    if (newDistance < minDistance && target.position.z > 0)
                    {
                        minDistance = newDistance;
                        currentTargetTransform = target;
                    }
                }
            }
            */
        }
        else
            currentTargetTransform = null;
    }

    void DeleteMissings()
    {
        for (var i = targetsInRange.Count - 1; i > -1; i--)
        {
            if (targetsInRange[i] == null)
                targetsInRange.RemoveAt(i);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.activeInHierarchy)
        {
            if (other.gameObject.layer == 11 && other.gameObject.tag == "Enemies" || (other.gameObject.layer == 12 && other.gameObject.tag == "Drop"))
            {
                targetsInRange.Add(other.gameObject.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
         if (other.gameObject.layer == 11 && other.gameObject.tag == "Enemies" || (other.gameObject.layer == 12 && other.gameObject.tag == "Drop"))
        {
            //if (targetsInRange.Contains(other.transform))
                targetsInRange.Remove(other.gameObject.transform);
        }
    }

    public void RemoveDeadEnemy(GameObject enemy)
    {
        targetsInRange.Remove(enemy.transform);
        /*
        foreach (Transform targetTransform in targetsInRange)
        {
            if (enemy.transform == targetTransform)
            {
                targetsInRange.Remove(enemy.transform);
                break;
            }
        }
        */
    }
}