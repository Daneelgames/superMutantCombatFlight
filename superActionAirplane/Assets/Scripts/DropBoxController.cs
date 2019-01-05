using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxController : MonoBehaviour
{
    public int type = 0;
    public float lifeTime = 5;
    public Destructible destructibleController;
    public Animator anim;

    [Header("0 is medpack, 1 is weapon")]
    public List<GameObject> sprites;

    private void Start()
    {
        Invoke("DestroyDropBox", lifeTime);
    }

    void DestroyDropBox()
    {
        GameManager.instance.pc.aimAssist.RemoveDeadEnemy(gameObject);
        Destroy(gameObject);
    }

    void SetUninvincible()
    {
        destructibleController.SetInvincible(false);
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

        destructibleController.SetInvincible(true);
        Invoke("SetUninvincible", 1); // this is needed for dropbox don't die in first moment
    }

    public void DropHealth()
    {
        print("drop health");
        GameManager.instance.pc.GetLive();
    }

    public void DropWeapon()
    {
        // choose weapon
        int weaponIndex = Random.Range(0, GameManager.instance.spawnerController.additionalWeapons.Count);
        GameObject newWeapon;

        if (GameManager.instance.pc.additionalWeapons.Count > 0)
        {
            List<AdditionalWeaponController> tempList = new List<AdditionalWeaponController>();

            foreach (AdditionalWeaponController weaponInSpawner in GameManager.instance.spawnerController.additionalWeapons)
            {
                bool canBeAdded = true;
                foreach (AdditionalWeaponController weaponOnPlayer in GameManager.instance.pc.additionalWeapons)
                {
                    if (weaponInSpawner.name == weaponOnPlayer.name) canBeAdded = false;
                }
                if (canBeAdded)
                {
                    tempList.Add(weaponInSpawner);
                    print(weaponInSpawner); // PRINT HERE
                }
            }
            weaponIndex = Random.Range(0, tempList.Count);
            newWeapon = Instantiate(tempList[weaponIndex].gameObject, transform.position, Quaternion.identity);
        }
        else
        {
            newWeapon = Instantiate(GameManager.instance.spawnerController.additionalWeapons[weaponIndex].gameObject, transform.position, Quaternion.identity);
        }
        newWeapon.name = newWeapon.name.Substring(0, newWeapon.name.Length - 7);
    }
}