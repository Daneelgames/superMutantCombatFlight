using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxController : MonoBehaviour
{
    public int type = 0;
    public float lifeTime = 5;
    public Destructible destructibleController;
    public Rigidbody rb;
    public float speed = 1;

    [Header("0 is medpack, 1 is weapon")]
    public List<GameObject> sprites;

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

    public void SetType(int _type) // 0 = health; 1 = weapon
    {
        foreach(GameObject go in sprites)
        {
            if (sprites.IndexOf(go) != _type)
            {
                go.SetActive(false);
            }
        }
        type = _type;

        if (_type == 0)
            GameManager.instance.spawnerController.SetHealthOnScene(true);
    }

    public void DropHealth()
    {
        print("drop health");
        GameManager.instance.pc.GetLive();
        GameManager.instance.spawnerController.SetHealthOnScene(false);
    }

    public void DropWeapon()
    {
        int weaponIndex = Random.Range(0, GameManager.instance.spawnerController.additionalWeapons.Count);

        /*  //CHOOSE UNIQUE WEAPON
        
        GameObject newWeapon;

        if (GameManager.instance.pc.additionalWeapons.Count > 0)
        {
            List<AdditionalWeaponController> tempList = new List<AdditionalWeaponController>();

            foreach (AdditionalWeaponController weaponInSpawner in GameManager.instance.spawnerController.additionalWeapons)
            {
                bool canBeAdded = true;
                foreach (AdditionalWeaponController weaponOnPlayer in GameManager.instance.pc.additionalWeapons)
                {
                    if (weaponInSpawner.name == weaponOnPlayer.name)
                    {
                        canBeAdded = false;
                        print(weaponOnPlayer.name + " and " + weaponInSpawner.name);
                    }
                }
                if (canBeAdded)
                {
                    tempList.Add(weaponInSpawner);
                }
            }
            weaponIndex = Random.Range(0, tempList.Count);
            print(weaponIndex);
            newWeapon = Instantiate(tempList[weaponIndex].gameObject, transform.position, Quaternion.identity);
        }
        else
        {
            newWeapon = Instantiate(GameManager.instance.spawnerController.additionalWeapons[weaponIndex].gameObject, transform.position, Quaternion.identity);
        }
        */

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