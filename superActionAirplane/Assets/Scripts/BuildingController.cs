using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public HideSolidController hideSolidController;
    float newZ;

    private void Start()
    {
        GameManager.instance.spawnerController.AddSolidOnScene(this);
        StartCoroutine("Grow");
        InvokeRepeating("CheckPositionZ", 5f, 0.1f);
    }

    IEnumerator Grow()
    {
        float t = 0;
        transform.localScale = Vector3.zero;
        while (t < 3)
        {
            t += Time.deltaTime;
            transform.localScale = new Vector3(t/3, t/3, t/3);
            yield return null;
        }
    }

    public void Wane()
    {
        StartCoroutine("WaneCoroutine");
    }

    IEnumerator WaneCoroutine()
    {
        float t = 0;
        transform.localScale = Vector3.zero;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, t * -3, transform.position.z);
            yield return null;
        }
        SolidDestroyed();
    }

    void CheckPositionZ()
    {
        if (transform.position.z < 0)
        {
            CancelInvoke();
            Invoke("SolidDestroyed", 0.5f);
        }
    }

    private void Update()
    {
        newZ = transform.position.z - GameManager.instance.spawnerController.movementSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }

    void SolidDestroyed()
    {
        GameManager.instance.spawnerController.RemoveSolid(this);
        Destroy(gameObject);
    }
}