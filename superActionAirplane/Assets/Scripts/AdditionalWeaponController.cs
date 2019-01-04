using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalWeaponController : MonoBehaviour
{
    public Vector2 offset;
    public GameObject bullet;
    public List<GameObject> bulletBurst;
    public float delay = 1;
    float currentDelay = 0;

    Transform weaponSpot; // spot for weapon parented by player

    private void Start()
    {
        if (GameManager.instance.pc.additionalWeapons.Count < 3) // if there is a free spot
        {
            weaponSpot = GameManager.instance.pc.additionalWeaponSpots[GameManager.instance.pc.additionalWeapons.Count]; // take free spot 
            GameManager.instance.pc.additionalWeapons.Add(this); // add weapon
        }
        else // if there is no free spot
        {
            int index = Random.Range(0,3); // take random spot
            GameManager.instance.pc.additionalWeapons[index].Remove();
            GameManager.instance.pc.additionalWeapons[index] = this; // replace existing weapon
            weaponSpot = GameManager.instance.pc.additionalWeaponSpots[index]; 
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
            if (bullet != null)
            {
                GameObject newBullet = GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
                BulletController bc = newBullet.GetComponent<BulletController>();
                Transform newTarget;
                bool autoAim = false;
                if (GameManager.instance.pc.aimAssist.currentTargetTransform != null) // if aimAssist has someone inside
                {
                    if (bc.rocket)
                        autoAim = true;

                    newTarget = GameManager.instance.pc.aimAssist.currentTargetTransform;
                }
                else
                {
                    newTarget = GameManager.instance.pc.target.transform;
                }

                bc.SetTarget(newTarget, new Vector3(offset.x, offset.y, 0), autoAim);

                currentDelay = delay;
            }
            else if (bulletBurst.Count > 0) // bullet burst
            {
                Transform newTarget;
                if (GameManager.instance.pc.aimAssist.currentTargetTransform != null) // if aimAssist has someone inside
                {
                    newTarget = GameManager.instance.pc.aimAssist.currentTargetTransform;
                }
                else
                {
                    newTarget = GameManager.instance.pc.target.transform;
                }
                StartCoroutine(ShotBulletBurst(newTarget));
            }
        }
    }

    IEnumerator ShotBulletBurst(Transform _newTarget)
    {
        currentDelay = delay;
        foreach (GameObject go in bulletBurst)
        {
            GameObject newBullet = GameObject.Instantiate(go, transform.position, Quaternion.identity);
            BulletController _bulletController = newBullet.GetComponent<BulletController>();
            if (_newTarget != null)
            {
                _newTarget = GameManager.instance.pc.target.transform;
            }
            _bulletController.SetTarget(_newTarget, offset, false);
            yield return new WaitForSeconds(_bulletController.delayNextShotTime);
        }
    }

    public void Remove()
    {
        Destroy(gameObject, 1);
    }
}