using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOutlineController : MonoBehaviour
{
    public List<MeshRenderer> outlineMeshes;
    public List<SkinnedMeshRenderer> outlineSkinnedMeshes;

    private void Start()
    {
        InvokeRepeating("ChangeColor", 0.1f, 0.1f);
    }

    void ChangeColor()
    {
        float distance = Mathf.Abs(1 - transform.position.z);
        if (outlineMeshes.Count > 0)
        {
            foreach (MeshRenderer mesh in outlineMeshes)
            {
                mesh.material.color = Color.Lerp(Color.red, Color.black, distance / 5f);
            }
        }
        else
        {
            foreach (SkinnedMeshRenderer mesh in outlineSkinnedMeshes)
            {
                mesh.material.color = Color.Lerp(Color.red, Color.black, distance / 5f);
            }
        }
    }
}