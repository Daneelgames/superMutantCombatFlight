﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    public bool invinsible = false;
    public int lives = 3;
    public float movementSpeed = 1;

    public PlayerTargetController target;
    public PlayerTargetController target_2;
    public AimAssistController aimAssist;

    public GameObject explosion;

    float shotDelay = 0;
    public GameObject bullet;
    public List<GameObject> bulletBurst = new List<GameObject>();
    public List<AdditionalWeaponController> additionalWeapons = new List<AdditionalWeaponController>();
    public List<Transform> additionalWeaponSpots = new List<Transform>();
    public float bulletBurstDelay = 0.1f;
    public Transform shotHolder;

    public Animator animator;

    public Rigidbody rb; 
    Vector2 _move;
    Vector3 _moveRotation = Vector3.zero;

    bool hurt = false;

   // bool touchInput = true;

    public float touchMovementScaler = 1.2f;
    public SkinnedMeshRenderer mesh;

    public AudioSource _audio;

    ObjectPooler objectPooler;

    float x = 0;
    float y = 0;

    Vector3 newPos;

     GameObject parent;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;

         parent = new GameObject(); 
         parent.transform.position = transform.position; 
         parent.name = "PlayerMovementContainer"; 
        //if (Application.platform == RuntimePlatform.Android)
        //    touchInput = true;
    }

    void Update()
    {
        /*
        //if (Input.GetMouseButton(0) || Input.touchCount > 0)
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            PlayerInputTouch();
            GetMoveRotationTouch();
            ShootByTouch();
        }
        else
        {
            // reset mouse input
            transform.SetParent(null);
            newPos = Vector3.zero;

            PlayerInput();
            GetMoveRotation();
            Shoot();
        }
        */
        PlayerInput();
        GetMoveRotation();
        Shoot();

        ClampMovement();
        RotatePlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Solids" && other.gameObject.layer == 9) || (other.gameObject.tag == "Enemies" && other.gameObject.layer == 11))
            Damage();
    }

    public void Damage()
    {
        if (!invinsible)
        {
            invinsible = true;
            Time.timeScale = 0.3f;
            CameraShaker.Instance.ShakeOnce(16f, 16f, 0.1f, 1f);
            objectPooler.SpawnGameObjectFromPool(explosion.name, transform.position, transform.rotation);

            StartCoroutine(PlayerDeath());
        }
    }

    IEnumerator HurtTimer()
    {
       // animator.SetBool("Hurt", true);
        hurt = true;
        mesh.enabled = false;
        yield return new WaitForSeconds(0.1f);
        mesh.enabled = true;
        yield return new WaitForSeconds(0.1f);
        mesh.enabled = false;
        yield return new WaitForSeconds(0.1f);
        mesh.enabled = true;
        yield return new WaitForSeconds(0.1f);
        mesh.enabled = false;
        yield return new WaitForSeconds(0.1f);
        mesh.enabled = true;
        yield return new WaitForSeconds(0.1f);
        mesh.enabled = false;
        yield return new WaitForSeconds(0.1f);
        mesh.enabled = true;
        yield return new WaitForSeconds(0.1f);
        mesh.enabled = false;
        yield return new WaitForSeconds(0.1f);
        mesh.enabled = true;
        yield return new WaitForSeconds(0.1f);

        mesh.enabled = true;
        hurt = false;
    }

    IEnumerator PlayerDeath()
    {
        GameManager.instance.Restart();
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;

        invinsible = false;
        foreach (AdditionalWeaponController wpn in additionalWeapons)
        {
            wpn.Remove();
        }
        additionalWeapons.Clear();
        transform.SetParent(null);
        newPos = Vector3.zero;
        parent.transform.position = transform.position; 
        gameObject.SetActive(false);
    }

    void PlayerInputTouch()
    {
        Vector3 screenPos;
        Vector3 worldPos;

        if (transform.parent == null)
        {
            parent.transform.position = transform.position;
            screenPos = Input.mousePosition;
            screenPos.z = 10.0f;
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Vector3 parentNewPos = parent.transform.position;
            parentNewPos.x = worldPos.x;
            parentNewPos.y = worldPos.y;

            parent.transform.position = parentNewPos;
            transform.SetParent(parent.transform);
        }

        // get mouse position in screen space
        // (if touch, gets average of all touches)
        screenPos = Input.mousePosition;
        // set a distance from the camera
        screenPos.z = 10.0f;
        // convert mouse position to world space
        worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        // get current position of this GameObject
        newPos = parent.transform.position;
        // set x position to mouse world-space x position
        newPos.x = worldPos.x * touchMovementScaler;
        newPos.y = worldPos.y * touchMovementScaler;
        // apply new position
        parent.transform.position = Vector3.Lerp(parent.transform.position, newPos, Time.deltaTime * 5);
    }

    void GetMoveRotationTouch()
    {
        if (newPos != Vector3.zero)
        {
            if (newPos.x - parent.transform.position.x > 1)
            {
                _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, -50, 0.9f * Time.deltaTime * 10f);
            }
            else if (parent.transform.position.x - newPos.x > 1)
            {
                _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, 50, 0.9f * Time.deltaTime * 10f);
            }
            else
            {
                _moveRotation.y = Mathf.LerpAngle(_moveRotation.y, 0, 0.9f * Time.deltaTime * 10f);
                _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, 0, 0.9f * Time.deltaTime * 10f);
            }

            if (newPos.y - parent.transform.position.y > 1)
            {
                _moveRotation.x = Mathf.LerpAngle(_moveRotation.x, -50, 0.9f * Time.deltaTime * 10f);
            }
            else if (parent.transform.position.y - newPos.y > 1)
            {
                _moveRotation.x = Mathf.LerpAngle(_moveRotation.x, 50, 0.9f * Time.deltaTime * 10f);
            }
            else
            {
                _moveRotation.x = Mathf.LerpAngle(_moveRotation.x, 0, 0.9f * Time.deltaTime * 10f);
            }
        }
        else
        {
            _moveRotation.x = Mathf.LerpAngle(_moveRotation.x, 0, 0.9f * Time.deltaTime * 10f);
            _moveRotation.y = Mathf.LerpAngle(_moveRotation.y, 0, 0.9f * Time.deltaTime * 10f);
            _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, 0, 0.9f * Time.deltaTime * 10f);
        }
    }

    void PlayerInput() //PC CONTROLS
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(x, y, 0) * movementSpeed * touchMovementScaler;
    }

    void GetMoveRotation()
    {
        if (rb.velocity != Vector3.zero)
        {
            _moveRotation.x = Mathf.LerpAngle(_moveRotation.x, -50 * y, 0.9f * Time.deltaTime * 10f);
            _moveRotation.y = Mathf.LerpAngle(_moveRotation.y, 50 * x, 0.9f * Time.deltaTime * 10f);
            _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, -50 * x, 0.9f * Time.deltaTime * 10f);
        }
        else
        {
            _moveRotation.x = Mathf.LerpAngle(_moveRotation.x, 0, 0.9f * Time.deltaTime * 10f);
            _moveRotation.y = Mathf.LerpAngle(_moveRotation.y, 0, 0.9f * Time.deltaTime * 10f);
            _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, 0, 0.9f * Time.deltaTime * 10f);
        }
    }

    void ClampMovement()
    {
        if (transform.position.x > 3)
            transform.position = new Vector3(3, transform.position.y, 0);
         if (transform.position.x < -3)
            transform.position = new Vector3(-3, transform.position.y, 0);
         if (transform.position.y > 4)
            transform.position = new Vector3(transform.position.x, 4, 0);
         if (transform.position.y < -4)
            transform.position = new Vector3(transform.position.x, -4, 0);
    }


    void RotatePlayer()
    {
        transform.localEulerAngles = _moveRotation;
    }
    
    void Shoot()
    {
        if (shotDelay > 0)
            shotDelay -= Time.deltaTime;
        else
        {
            if (bullet)
            {
                ShotBullet();
            }
            else
                StartCoroutine("ShotBulletBurst");

            if (additionalWeapons.Count > 0)
            {
                foreach (AdditionalWeaponController additionalWeapon in additionalWeapons)
                {
                    additionalWeapon.Shot();
                }
            }
        }
    }

    void ShotBullet()
    {
        _audio.pitch = Random.Range(0.75f, 1.25f);
        _audio.Play();
        BulletController _bulletController = objectPooler.SpawnBulletFromPool("PlayerBullet", shotHolder.position, Quaternion.identity);
        _bulletController.SetTarget(target.transform, Vector3.zero, false);
        shotDelay = _bulletController.delayNextShotTime;
    }

    IEnumerator ShotBulletBurst()
    {
        foreach (GameObject go in bulletBurst)
        {
            BulletController _bulletController = objectPooler.SpawnBulletFromPool("PlayerBullet", shotHolder.position, Quaternion.identity);
            _bulletController.SetTarget(target.transform, Vector3.zero, false);
            shotDelay = _bulletController.delayNextShotTime;
            yield return new WaitForSeconds(bulletBurstDelay);
        }
    }

    public void ShootByTouch()
    {
        if (shotDelay > 0)
            shotDelay -= Time.deltaTime;

        else if (shotDelay <= 0)
        {
            if (bullet)
                ShotBullet();
            else
                StartCoroutine("ShotBulletBurst");
        }
            
        if (additionalWeapons.Count > 0)
        {
            foreach (AdditionalWeaponController additionalWeapon in additionalWeapons)
            {
                additionalWeapon.Shot();
            }
        }
    }

    public void SetSensitivity(float newSensitivity)
    {
        touchMovementScaler = newSensitivity;
    }

    public void SetScale(float newScale)
    {
        transform.localScale += Vector3.one * newScale * 0.3f;
    }
}