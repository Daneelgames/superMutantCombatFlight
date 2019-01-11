using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public float speed = 100;
    public HideSolidController hideSolidController;
    float newZ;

    private void Start()
    {
        GameManager.instance.spawnerController.AddSolidOnScene(this);
    }

    private void FixedUpdate()
    {
        newZ = transform.position.z - speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

    }

    private void Update()
    {
        if (transform.position.z < -10)
        {
            GameManager.instance.spawnerController.RemoveSolid(this);
            Destroy(gameObject);
        }
    }
}