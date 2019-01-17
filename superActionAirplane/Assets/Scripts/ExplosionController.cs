using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    public float speed = 2;

    private void OnEnable()
    {
        Invoke("DestroyExplosion", 2);
    }

    void DestroyExplosion()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float newZ = transform.position.z - Time.deltaTime * speed;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}
