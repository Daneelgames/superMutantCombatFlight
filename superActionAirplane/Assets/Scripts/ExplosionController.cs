using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    public float speed = 2;

    void Start()
    {
        Destroy(gameObject, 2);
    }

    private void Update()
    {
        float newZ = transform.position.z - Time.deltaTime * speed;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}
