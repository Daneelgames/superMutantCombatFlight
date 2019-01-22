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
    }

    IEnumerator Grow()
    {
        float t = 0;
        transform.localScale = Vector3.zero;
        while (t <1)
        {
            t += Time.deltaTime;
      //      transform.localScale = new Vector3(t, t, t);
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        newZ = transform.position.z - GameManager.instance.spawnerController.movementSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Destroyer")
        {
            GameManager.instance.spawnerController.RemoveSolid(this);
            Destroy(gameObject);
        }
    }
}