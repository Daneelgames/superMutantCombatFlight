using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool invinsible = false;

    public int lives = 3;

    public float movementSpeed = 1;
    //public int rolling = 0; //-1 roll left; 1 roll right
    //public float rollInputDelayMax = 0.2f;
    //float rollInputDelay = 0.3f;

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

    //bool canRollLeft = false;
    //bool canRollRight = false;

    Vector2 movementVector;
    public Rigidbody rb; 
    Vector2 _move;
    Vector3 _moveRotation = Vector3.zero;


    bool hurt = false;

    bool touchInput = false;

    public float touchMovementScaler = 1.5f;

    private void Start()
    {
        GameManager.instance.GetLinks();

        if (Application.platform == RuntimePlatform.Android)
            touchInput = true;

    }

    void Update()
    {
        PlayerInput();
        GetMoveRotation();
        Shoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Solids" && other.gameObject.layer == 9) || (other.gameObject.tag == "Enemies" && other.gameObject.layer == 11))
        Damage();
    }

    public void GetLive()
    {
        lives += 1;
        GameManager.instance.uiLivesController.UpdateLivesUI(); // UI animation
    }

    public void Damage()
    {
        if (!hurt)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            if (!invinsible)
            {
                lives -= 1;
                GameManager.instance.uiLivesController.UpdateLivesUI(); // UI animation
            }

            if (lives > 0)
            {
                StartCoroutine("HurtTimer");
            }
            else //death
            {
                target.gameObject.SetActive(false);
                target_2.gameObject.SetActive(false);
                gameObject.SetActive(false);
                GameManager.instance.StartCoroutine("Restart");
            }
        }
    }

    IEnumerator HurtTimer()
    {
       // animator.SetBool("Hurt", true);
        hurt = true;
        for (float t = 1; t>0; t -= Time.deltaTime)
        {
            yield return null;
        }
       // animator.SetBool("Hurt", false);
        hurt = false;
    }


    void PlayerInput()
    {
        if (!touchInput)
            _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else
            _move = new Vector2(GameManager.instance.touchInputController.joystick.Horizontal, GameManager.instance.touchInputController.joystick.Vertical) * touchMovementScaler;
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
        if (transform.position.x < -7 && move.x < 0)
        {
            move.x = Mathf.Lerp(move.x, 0, 0.9f);
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, -7, 0.9f), transform.position.y, transform.position.z);
        }
        if (transform.position.x > 7 && move.x > 0)
        {
            move.x = Mathf.Lerp(move.x, 0, 0.9f);
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, 7, 0.9f), transform.position.y, transform.position.z);
        }
        if (transform.position.y < -4 && move.y < 0)
        {
            move.y = Mathf.Lerp(move.y, 0, 0.9f);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, -4, 0.9f), transform.position.z);
        }
        if (transform.position.y > 4 && move.y > 0)
        {
            move.y = Mathf.Lerp(move.y, 0, 0.9f);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 4, 0.9f), transform.position.z);
        }
        return move;
    }

    void MovePlayer()
    {
        /*
        if (rolling != 0)
        {
            movementVector.x = 2 * rolling;
        }
        */

        Vector3 force = new Vector3(movementVector.x, movementVector.y, 0);
        rb.velocity = force * movementSpeed;
    }

    void RotatePlayer()
    {
        transform.localEulerAngles = _moveRotation;
    }
    
    void Shoot()
    {
        if (shotDelay > 0)
            shotDelay -= Time.deltaTime;

        if (!touchInput)
        {
            if (Input.GetButton("Fire1") && shotDelay <= 0)
            {
                if (bullet)
                {
                    animator.SetBool("Shoot", true);
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

            if (!Input.GetButton("Fire1") && bullet)
            {
                animator.SetBool("Shoot", false);
            }
        }
    }

    void ShotBullet()
    {
        GameObject newBullet = GameObject.Instantiate(bullet, shotHolder.position, Quaternion.identity);
        BulletController _bulletController = newBullet.GetComponent<BulletController>();
        _bulletController.SetTarget(target.transform, Vector3.zero, false);
        shotDelay = _bulletController.delayNextShotTime;
    }

    IEnumerator ShotBulletBurst()
    {
        animator.SetBool("Shoot", true);
        foreach (GameObject go in bulletBurst)
        {
            GameObject newBullet = GameObject.Instantiate(go, transform.position, Quaternion.identity);
            BulletController _bulletController = newBullet.GetComponent<BulletController>();
            //_bulletController.SetTarget(target.transform.parent, Vector3.zero);
            _bulletController.SetTarget(target.transform, Vector3.zero, false);
            shotDelay = _bulletController.delayNextShotTime;
            yield return new WaitForSeconds(bulletBurstDelay);
        }
        animator.SetBool("Shoot", false);
    }

    /*

    public void SetCharge(bool active)
    {
        charge = active;
        if(currentChargeTime >= chargeTime)
        {
            ChargedShot();
        }
    }

    public void Charge()
    {
        if (charge)
        {
            if (currentChargeTime < chargeTime)
                currentChargeTime += Time.deltaTime;
        }
        else
            currentChargeTime = 0;
    }

    */

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