using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasController : MonoBehaviour
{
    public PlayerController pc;
    // Start is called before the first frame update
    void Awake()
    {
        //transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 newPos = new Vector3(pc.transform.position.x, pc.transform.position.y, transform.position.z) * 3.3f;
        //transform.position = newPos;
    }
}