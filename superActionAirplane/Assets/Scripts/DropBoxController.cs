using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxController : MonoBehaviour
{
    public float lifeTime = 5;
    public Destructible destructibleController;
    public Rigidbody rb;
    public float speed = 1;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        gameManager.spawnerController.ToggleDropBoxOnScene(true);
        StartCoroutine("MoveCrate");
    }

    void DestroyDropBox()
    {
        gameManager.pc.aimAssist.RemoveDeadEnemy(gameObject);

        ObjectPooler.instance.SpawnGameObjectFromPool(destructibleController.explosion.name, transform.position, Quaternion.identity);

        gameManager.spawnerController.ToggleDropBoxOnScene(false);
        Destroy(gameObject);
    }

    public void DropWeapon()
    {
        gameManager.spawnerController.ToggleDropBoxOnScene(false);

        int weaponIndex = Random.Range(0, gameManager.spawnerController.additionalWeapons.Count);
        int doubles = 0;

        for (int i = 0; i < gameManager.pc.additionalWeapons.Count; i++)
        {
            if (gameManager.spawnerController.additionalWeapons[weaponIndex].name == gameManager.pc.additionalWeapons[i].name)
            {
                doubles += 1;
            }
        }

        if (doubles == 3)
        {
            if (weaponIndex > 0)
                weaponIndex -= 1;
            else
                weaponIndex += 1;
        }

        GameObject newWeapon = Instantiate(gameManager.spawnerController.additionalWeapons[weaponIndex].gameObject, transform.position, Quaternion.identity);
        newWeapon.name = newWeapon.name.Substring(0, newWeapon.name.Length - 7);
        print(newWeapon.name + " is dropped on " + Time.deltaTime);
    }

    IEnumerator MoveCrate()
    {
        float t = 0;
        Vector3 startPosition = transform.position;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, new Vector3(transform.position.x, 4, 7), t);
            yield return null;
        }
        startPosition = transform.position;
        t = 0;
        while (t < 0.9f)
        {
            t += Time.deltaTime/9;
            transform.position = Vector3.Lerp(startPosition, new Vector3(transform.position.x, -5.25f, 7), t);
            yield return null;
        }
        DestroyDropBox();
    }
}