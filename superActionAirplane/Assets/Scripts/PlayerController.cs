using System.Collections;
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

    Vector2 movementVector;
    public Rigidbody rb; 
    Vector2 _move;
    Vector3 _moveRotation = Vector3.zero;

    bool hurt = false;

    bool touchInput = true;

    public float touchMovementScaler = 1.2f;
    public SkinnedMeshRenderer mesh;

    ObjectPooler objectPooler;

    Vector3 startPos;
    Vector3 distance;
    public Text inputDistance;


    private void Start()
    {
        objectPooler = ObjectPooler.instance;

        //if (Application.platform == RuntimePlatform.Android)
        //    touchInput = true;
    }

    void Update()
    {
        PlayerInput();
        GetMoveRotation();
        Shoot();
        ShootByTouch();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Solids" && other.gameObject.layer == 9) || (other.gameObject.tag == "Enemies" && other.gameObject.layer == 11))
            Damage();
    }

    public void Damage()
    {
        CameraShaker.Instance.ShakeOnce(16f, 16f, 0.1f, 1f);
        objectPooler.SpawnGameObjectFromPool(explosion.name, transform.position, transform.rotation);

        PlayerDeath();
        return;

        if (!hurt)
        {
            CameraShaker.Instance.ShakeOnce(16f, 16f, 0.1f, 1f);

            objectPooler.SpawnGameObjectFromPool(explosion.name, transform.position, transform.rotation);
            if (!invinsible)
            {
                lives -= 1;
                GameManager.instance.uiLivesController.UpdateLivesUI(); // UI animation

                // lost weapon
                if (additionalWeapons.Count > 0)
                {
                    int random = Random.Range(0, additionalWeapons.Count);
                    additionalWeapons[random].Remove();
                    additionalWeapons.RemoveAt(random);
                    if (additionalWeapons.Count > 0)
                    {
                        foreach (AdditionalWeaponController wpn in additionalWeapons)
                        {
                            wpn.Reparent(additionalWeapons.IndexOf(wpn));
                        }
                    }
                }

                // lost all weapons
                /*
                foreach (AdditionalWeaponController w in additionalWeapons)
                {
                    w.Remove();
                    additionalWeapons.Remove(w);
                }
                additionalWeapons.Clear();
                */
            }

            if (lives > 0)
            {
                StartCoroutine("HurtTimer");
            }
            else //death
            {
                PlayerDeath();
            }
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

    void PlayerDeath()
    {
        foreach(AdditionalWeaponController wpn in additionalWeapons)
        {
            wpn.Remove();
        }
        additionalWeapons.Clear();
        //target.gameObject.SetActive(false);
        //target_2.gameObject.SetActive(false);
        GameManager.instance.Restart();
        gameObject.SetActive(false);
    }

    void PlayerInput()
    {
        /*
        if (!touchInput)
            _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else
        {
            _move = new Vector2(GameManager.instance.touchInputController.joystick.Horizontal, GameManager.instance.touchInputController.joystick.Vertical) * touchMovementScaler;

        }
        */
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = Camera.main.ScreenToViewportPoint(touch.position);
                    startPos = new Vector3(startPos.x, startPos.y, 0);
                     break;

                case TouchPhase.Moved:
                    distance = Camera.main.ScreenToViewportPoint(touch.position) - startPos;
                    distance = new Vector3(distance.x, distance.y, 0);
                    break;
            }
            inputDistance.text = ""+ distance;
        }
    }


    void GetMoveRotation()
    {
        _moveRotation.x = Mathf.LerpAngle(_moveRotation.x, _move.y * -30, 0.9f * Time.deltaTime * 10f);
        _moveRotation.y = Mathf.LerpAngle(_moveRotation.y, _move.x * 50, 0.9f * Time.deltaTime * 10f);
        _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, _move.x * -50, 0.9f * Time.deltaTime * 10f);
    }

    void FixedUpdate()
    {
        movementVector = ClampMovement(_move);
        MovePlayer();
        RotatePlayer();
    }

    Vector2 ClampMovement(Vector2 move)
    {
        if (transform.position.x < -3 && distance.x < 0)
        {
            distance.x = Mathf.Lerp(distance.x, 0, 0.9f);
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, -3, 0.9f), transform.position.y, transform.position.z);
        }
        if (transform.position.x > 3 && distance.x > 0)
        {
            distance.x = Mathf.Lerp(distance.x, 0, 0.9f);
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, 3, 0.9f), transform.position.y, transform.position.z);
        }
        if (transform.position.y < -4 && distance.y < 0)
        {
            distance.y = Mathf.Lerp(distance.y, 0, 0.9f);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, -4, 0.9f), transform.position.z);
        }
        if (transform.position.y > 4.5f && distance.y > 0)
        {
            distance.y = Mathf.Lerp(distance.y, 0, 0.9f);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 4.5f, 0.9f), transform.position.z);
        }
        return move;
    }

    void MovePlayer()
    {
        transform.position += new Vector3 (distance.x, distance.y, 0);
        /*
        Vector3 force = new Vector3(movementVector.x, movementVector.y, 0);
        rb.velocity = force * movementSpeed;
        */
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
            if (!touchInput)
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
    }

    void ShotBullet()
    {
        //GameObject newBullet = GameObject.Instantiate(bullet, shotHolder.position, Quaternion.identity);
        BulletController _bulletController = objectPooler.SpawnBulletFromPool("PlayerBullet", shotHolder.position, Quaternion.identity);
        _bulletController.SetTarget(target.transform, Vector3.zero, false);
        shotDelay = _bulletController.delayNextShotTime;
    }

    IEnumerator ShotBulletBurst()
    {
        foreach (GameObject go in bulletBurst)
        {
            //GameObject newBullet = GameObject.Instantiate(go, transform.position, Quaternion.identity);
            BulletController _bulletController = objectPooler.SpawnBulletFromPool("PlayerBullet", shotHolder.position, Quaternion.identity);
            //_bulletController.SetTarget(target.transform.parent, Vector3.zero);
            _bulletController.SetTarget(target.transform, Vector3.zero, false);
            shotDelay = _bulletController.delayNextShotTime;
            yield return new WaitForSeconds(bulletBurstDelay);
        }
    }

    public void ShootByTouch()
    {
        if (touchInput)
        {
            if (shotDelay <= 0)
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
    }
}