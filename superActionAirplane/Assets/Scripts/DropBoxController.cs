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
        gameManager.spawnerController.ToggleDropBoxOnScene(this);
        StartCoroutine("MoveCrate");
    }

    void DestroyDropBox()
    {
        gameManager.pc.aimAssist.RemoveDeadEnemy(gameObject);

        ObjectPooler.instance.SpawnGameObjectFromPool(destructibleController.explosion.name, transform.position, Quaternion.identity);

        gameManager.spawnerController.ToggleDropBoxOnScene(null);
        Destroy(gameObject);
    }

    public void DropWeapon()
    {
        gameManager.spawnerController.ToggleDropBoxOnScene(null);

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
    }

    IEnumerator MoveCrate()
    {
        float t = 0;
        float newX = transform.position.x;

        if (newX > 4.5f)
            newX = 4.5f;

        if (newX < -4.5f)
            newX = -4.5f;

        Vector3 startPosition = transform.position;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, new Vector3(newX, 4, 7), t);
            yield return null;
        }
        startPosition = transform.position;
        t = 0;
        while (t < 4f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, new Vector3(newX, -5.25f, 7), t/4);
            yield return null;
        }
        DestroyDropBox();
    }

    public void RemoveDropBox()
    {
        StopAllCoroutines();
        StartCoroutine("MoveUp");
    }

    IEnumerator MoveUp()
    {
        float t = 0;

        while (t < 1)
        {
            transform.position += Vector3.up * Time.deltaTime * 30;
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}