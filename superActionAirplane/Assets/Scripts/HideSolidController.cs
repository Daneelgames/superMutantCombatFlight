using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSolidController : MonoBehaviour
{
    public List<Collider> colliders;
    public List<GameObject> art;

    public void HideSolid()
    {
        foreach(Collider coll in colliders)
            coll.enabled = false;

        if (art.Count > 0)
        {
            StartCoroutine("Hide");
        }
    }

    IEnumerator Hide()
    {
        foreach (GameObject obj in art)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in art)
        {
            obj.SetActive(false);
        }
    }
}
