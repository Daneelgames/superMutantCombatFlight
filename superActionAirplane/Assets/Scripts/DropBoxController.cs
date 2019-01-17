using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxController : MonoBehaviour
{
    public float lifeTime = 5;
    public Destructible destructibleController;
    public Rigidbody rb;
    public float speed = 1;

    bool goingUp = true;
    float t = 0;
    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    void DestroyDropBox()
    {
        GameManager.instance.pc.aimAssist.RemoveDeadEnemy(gameObject);

        Instantiate(destructibleController.explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void DropWeapon()
    {
        int weaponIndex = Random.Range(0, GameManager.instance.spawnerController.additionalWeapons.Count);

        GameObject newWeapon = Instantiate(GameManager.instance.spawnerController.additionalWeapons[weaponIndex].gameObject, transform.position, Quaternion.identity);
        newWeapon.name = newWeapon.name.Substring(0, newWeapon.name.Length - 7);
    }

    private void Update()
    {
        if (goingUp)
        {
            if (transform.position.y < 4)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, new Vector3(transform.position.x, 4, 7), t);
            }
            else
            {
                t = 0;
                startPosition = transform.position;
                goingUp = false;
            }
        }
        else
        {
            if (transform.position.y > -5.25f)
            {
                t += Time.deltaTime/9f;
                transform.position = Vector3.Lerp(startPosition, new Vector3(transform.position.x, -5.25f, 7), t);
            }
            else
                DestroyDropBox();
        }
    }
}