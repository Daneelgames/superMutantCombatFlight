using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    Vector3 targetPosition = new Vector3(-4,6,0);

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.9f * Time.deltaTime * 6);

        if (Vector3.Distance(transform.position, targetPosition) < 1)
            gameObject.SetActive(false);
    }
}