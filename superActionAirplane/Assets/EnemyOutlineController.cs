using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOutlineController : MonoBehaviour
{
    public List<MeshRenderer> outlineMeshes;

    private void Start()
    {
        InvokeRepeating("ChangeColor", 0.1f, 0.1f);
    }

    void ChangeColor()
    {
        float distance = Mathf.Abs(1 - transform.position.z);
        print(distance / 5f);
        foreach (MeshRenderer mesh in outlineMeshes)
        {
            mesh.material.color = Color.Lerp(Color.red, Color.black, distance / 5f);
        }
    }
}