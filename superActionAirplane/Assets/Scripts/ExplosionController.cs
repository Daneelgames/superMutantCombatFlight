using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public AudioSource _audio;
    public float speed = 2;

    private void OnEnable()
    {
        if (_audio)
        {
            _audio.pitch = Random.Range(0.75f, 1.25f);
            _audio.Play();
        }
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
