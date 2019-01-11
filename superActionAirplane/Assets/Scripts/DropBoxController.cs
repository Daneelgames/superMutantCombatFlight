using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxController : MonoBehaviour
{
    public int type = 0;
    public float lifeTime = 5;
    public Destructible destructibleController;
    public Animator anim;
    public Rigidbody rb;
    public float speed = 1;

    bool moveUp = true;

    [Header("0 is medpack, 1 is weapon")]
    public List<GameObject> sprites;

    private void Start()
    {
        Invoke("DestroyDropBox", lifeTime);
        InvokeRepeating("SetVelocity", 0.1f, 0.1f);

        StartCoroutine(SetMovementDirection(true));
    }

    IEnumerator SetMovementDirection(bool _moveUp)
    {
        moveUp = _moveUp;

        yield return new WaitForSeconds(1);

        StartCoroutine(SetMovementDirection(!_moveUp));
    }

    private void SetVelocity()
    {
        if (moveUp)
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.up, 0.9f * Time.deltaTime * speed);
        else
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.down, 0.9f * Time.deltaTime * speed);
    }

    void DestroyDropBox()
    {
        GameManager.instance.pc.aimAssist.RemoveDeadEnemy(gameObject);
        Destroy(gameObject);
    }

    public void SetType(int _type) // 0 = health; 1 = weapon
    {
        foreach(GameObject go in sprites)
        {
            if (sprites.IndexOf(go) != type)
                go.SetActive(false);
        }
        sprites[type].SetActive(true);
        type = _type;

        anim.SetInteger("State", type);
        anim.SetTrigger("Play");

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
        // choose weapon
        int weaponIndex = Random.Range(0, GameManager.instance.spawnerController.additionalWeapons.Count);

        /*
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
}