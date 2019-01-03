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
    public GameObject bulletCharged;
    public List<GameObject> bulletBurst = new List<GameObject>();
    public float bulletBurstDelay = 0.1f;

    bool charge = false;
    public float chargeTime = 1;
    public float currentChargeTime = 0;

    public Animator animator;
    public Animator chargeAnimator;

    //bool canRollLeft = false;
    //bool canRollRight = false;

    Vector2 movementVector;
    public Rigidbody rb; 
    Vector2 _move;
    Vector3 _moveRotation = Vector3.zero;


    bool hurt = false;

    bool touchInput = false;

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
        Charge();

        //charge animate
        chargeAnimator.SetFloat("Charge",currentChargeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Solids" && other.gameObject.layer == 9) || (other.gameObject.tag == "Enemies" && other.gameObject.layer == 11))
        Damage();
    }

    public void Damage()
    {
        if (!hurt)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            if (!invinsible)
            {
                lives -= 1;
                GameManager.instance.uiLivesController.PlayerDamaged();
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
        animator.SetBool("Hurt", true);
        hurt = true;
        for (float t = 1; t>0; t -= Time.deltaTime)
        {
            yield return null;
        }
        animator.SetBool("Hurt", false);
        hurt = false;
    }


    void PlayerInput()
    {
        if (!touchInput)
            _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else
            _move = new Vector2(GameManager.instance.touchInputController.joystick.Horizontal, GameManager.instance.touchInputController.joystick.Vertical);
        //movementVector = new Vector2(_x, _y);

        /*
        if (rolling == 0 && !touchInput)
        {
            if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
            {
                canRollRight = false;

                if (!canRollLeft)
                {
                    rollInputDelay = rollInputDelayMax;
                    canRollLeft = true;
                }
                else if (rollInputDelay > 0)
                {
                    canRollLeft = false;
                    rolling = -1;
                    StartCoroutine(Roll());
                }
            }

            if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
            {
                canRollLeft = false;

                if (!canRollRight)
                {
                    rollInputDelay = rollInputDelayMax;
                    canRollRight = true;
                }
                else if (rollInputDelay > 0)
                {
                    canRollRight = false;
                    rolling = 1;
                    StartCoroutine(Roll());
                }
            }
        }

        if (rollInputDelay > 0)
            rollInputDelay -= Time.deltaTime;
        else
        {
            rollInputDelay = 0;
            canRollLeft = false;
            canRollRight = false;
        }
        */
    }

    /*
    public void RollByTouch()
    {
        if (touchInput)
        {
            if (_move.x >0)
            {
                rolling = 1;
                StartCoroutine(Roll());
            }
            else if (_move.x < 0)
            {
                rolling = -1;
                StartCoroutine(Roll());
            }
        }
    }

    IEnumerator Roll()
    {
        for (float t = 0.15f; t > 0; t -= Time.deltaTime)
        {
            yield return null;
        }
        rolling = 0;
    }

    */

    void GetMoveRotation()
    {
        _moveRotation.x = Mathf.LerpAngle(_moveRotation.x, _move.y * -30, 0.9f * Time.deltaTime * 10f);
        _moveRotation.y = Mathf.LerpAngle(_moveRotation.y, _move.x * 50, 0.9f * Time.deltaTime * 10f);
        _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, _move.x * -50, 0.9f * Time.deltaTime * 10f);

        /*
        if (rolling == 0)
        {
            _moveRotation.x = Mathf.LerpAngle(_moveRotation.x, _move.y * -30, 0.9f * Time.deltaTime * 10f);
            _moveRotation.y = Mathf.LerpAngle(_moveRotation.y, _move.x * 50, 0.9f * Time.deltaTime * 10f);
            _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, _move.x * -50, 0.9f * Time.deltaTime * 10f);
        }
        else
        {
            _moveRotation.z += 1200 * -rolling * Time.deltaTime;
        }

        if (_move.x < 0)
        {
            _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, 50, 0.9f * Time.deltaTime*10f);
        }
        else if (_move.x == 0)
        {
            _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, 0, 0.9f * Time.deltaTime * 10f);
        }
        else if (_move.x > 0)
        {
            _moveRotation.z = Mathf.LerpAngle(_moveRotation.z, -50, 0.9f * Time.deltaTime * 10f);
        }
        */
    }

    void FixedUpdate()
    {
        movementVector = ClampMovement(_move);
        MovePlayer();
        RotatePlayer();
    }

    /*
    IEnumerator Roll(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        //var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        var toAngle = Quaternion.Euler(0,0,-360);
        for (var t = 0f; t < 0.5f; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
        rolling = 0;
    }
    */

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
            if (Input.GetButtonDown("Fire1") && shotDelay <= 0)
            {
                if (bullet)
                    ShotBullet();
                else
                    StartCoroutine("ShotBulletBurst");
            }
            if (Input.GetButton("Fire1")) // charge
            {
                charge = true;
            }

            if (Input.GetButtonUp("Fire1")  && currentChargeTime > 0) // charged shot
            {
                ChargedShot();
            }
        }
    }

    void ChargedShot()
    {
        if (charge)
            charge = false;
        currentChargeTime = 0;
        if (target.chargeMarker.gameObject.activeInHierarchy)
        {
            GameObject newBullet = GameObject.Instantiate(bulletCharged, transform.position, Quaternion.identity);
            BulletController bc = newBullet.GetComponent<BulletController>();
            bc.SetCharge(true);
            bc.SetTarget(target.chargeMarker.enemyTarget, Vector3.zero);
            target.chargeMarker.Shot();
        }
    }

    void ShotBullet()
    {
        GameObject newBullet = GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
        BulletController _bulletController = newBullet.GetComponent<BulletController>();
        _bulletController.SetTarget(target.transform, Vector3.zero);
        shotDelay = _bulletController.delayNextShotTime;
    }

    IEnumerator ShotBulletBurst()
    {
        foreach (GameObject go in bulletBurst)
        {
            GameObject newBullet = GameObject.Instantiate(go, transform.position, Quaternion.identity);
            BulletController _bulletController = newBullet.GetComponent<BulletController>();
            //_bulletController.SetTarget(target.transform.parent, Vector3.zero);
            _bulletController.SetTarget(target.transform, Vector3.zero);
            shotDelay = _bulletController.delayNextShotTime;
            yield return new WaitForSeconds(bulletBurstDelay);
        }
    }

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


    public void ShootByTouch()
    {
        if (touchInput && shotDelay <= 0)
        {
            if (bullet)
                ShotBullet();
            else
                StartCoroutine("ShotBulletBurst");
        }
    }
}