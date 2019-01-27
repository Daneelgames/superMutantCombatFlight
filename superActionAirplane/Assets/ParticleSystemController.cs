using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public ParticleSystem particles;

    public Color startColor;
    public Color finalColor;
    float maxDistance = 10;
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

    private void Update() // НАДО ПРОВЕРЯТЬ ПО РАССТОЯНИЮ ДО ИГРОКА
    {
         distance = Vector3.Distance(transform.position, pc.transform.position);

        var main = particles.main;
        main.startColor = Color.Lerp(finalColor, startColor, distance / maxDistance);
    }
}