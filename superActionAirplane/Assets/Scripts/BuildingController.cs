using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public float speed = 10;
    public HideSolidController hideSolidController;

    private void Start()
    {
        GameManager.instance.spawnerController.AddSolidOnScene(this);
    }

    private void FixedUpdate()
    {
        float newZ = transform.position.z - speed * Time.deltaTime;
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