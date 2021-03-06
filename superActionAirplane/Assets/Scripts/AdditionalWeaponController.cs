﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalWeaponController : MonoBehaviour
{
    public Vector2 offset;
    public GameObject bullet;
    public List<GameObject> bulletBurst;
    public float delay = 1;
    float currentDelay = 0;
    public GameObject smallExplosion;
    public Transform shotHolder;
    public Animator anim;
    PlayerController pc;

    public AudioSource _audio;

    ObjectPooler objectPooler;

    public PowerUpController powerUpController;
    public GameObject feedbackDrop;

    Transform weaponSpot; // spot for weapon parented by player

    private void Start()
    {
        Instantiate(feedbackDrop, transform.position, Quaternion.identity);
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        //StartCoroutine(GameManager.instance.menuController.GuiActorPlay("PowerUp", gameObject.name));

        objectPooler = ObjectPooler.instance;

        if (pc.additionalWeapons.Count < 3) // if there is a free spot
        {
            weaponSpot = pc.additionalWeaponSpots[pc.additionalWeapons.Count]; // take free spot 
            pc.additionalWeapons.Add(this); // add weapon
        }
        else // if there is no free spot
        {
            GetWeaponSpot();
        }
    }

    void GetWeaponSpot()
    {
        for (int i = 0; i < 3; i++)
        {
            if (pc.additionalWeapons[i].name != gameObject.name)
            {
                pc.additionalWeapons[i].Remove();
                pc.additionalWeapons[i] = this; // replace existing weapon
                weaponSpot = pc.additionalWeaponSpots[i];

                return;
            }
        }
    }

    private void Update()
    {
        if (currentDelay > 0)
            currentDelay -= Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, weaponSpot.position, 0.7f * Time.deltaTime * 10);

    }
    
    public void Shot()
    {
        if (currentDelay <= 0)
        {
            if (_audio)
            {
                _audio.pitch = Random.Range(0.75f, 1.25f);
                _audio.Play();
            }

            if (bullet != null)
            {
                if (anim)
                    anim.SetTrigger("Shoot");
                BulletController newBullet = objectPooler.SpawnBulletFromPool(bullet.name, shotHolder.position, Quaternion.identity);
                Transform newTarget;
                bool autoAim = false;
                if (pc.aimAssist.currentTargetTransform != null) // if aimAssist has someone inside
                {
                    if (newBullet.rocket)
                        autoAim = true;

                    newTarget = pc.aimAssist.currentTargetTransform;
                }
                else
                {
                    newTarget = pc.target.transform;
                }

                newBullet.SetTarget(newTarget, new Vector3(offset.x, offset.y, 0), autoAim);

                currentDelay = delay;
            }
            else if (bulletBurst.Count > 0) // bullet burst
            {
                Transform newTarget;
                if (pc.aimAssist.currentTargetTransform != null) // if aimAssist has someone inside
                {
                    newTarget = pc.aimAssist.currentTargetTransform;
                }
                else
                {
                    newTarget = pc.target.transform;
                }
                StartCoroutine(ShotBulletBurst(newTarget));
            }
        }
    }

    IEnumerator ShotBulletBurst(Transform _newTarget)
    {
        currentDelay = delay;
        if (anim)
            anim.SetTrigger("Shoot");
        foreach (GameObject go in bulletBurst)
        {

            BulletController newBullet = objectPooler.SpawnBulletFromPool(go.name, shotHolder.position, Quaternion.identity);
            if (_newTarget != null)
            {
                _newTarget = pc.target.transform;
            }
            newBullet.SetTarget(_newTarget, offset, false);
            yield return new WaitForSeconds(newBullet.delayNextShotTime);
        }
    }

    public void Remove()
    {
        //Instantiate(smallExplosion, shotHolder.position, Quaternion.identity);

        if (powerUpController)
            powerUpController.ToggleEffectInactive();

        Destroy(gameObject);
    }
    public void Reparent(int index)
    {
        weaponSpot = pc.additionalWeaponSpots[index].transform;
    }
}