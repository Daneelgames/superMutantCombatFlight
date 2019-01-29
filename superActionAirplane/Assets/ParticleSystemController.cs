using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public ParticleSystem particles;

    public Color startColor;
    public Color finalColor;
    float maxDistance = 3;
    float distance;
    //public float t = 1;

    PlayerController pc;

    private void OnEnable()
    {
        var main = particles.main;
        main.startColor = startColor;
    }

    private void Start()
    {
        pc = GameManager.instance.pc;
    }

    private void Update()
    {
        if (transform.position.z > 1)
        {
            distance = Mathf.Abs(transform.position.z - 1);

            var main = particles.main;
            main.startColor = Color.Lerp(finalColor, startColor, distance / maxDistance);
        }
    }
}