using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierMaster;

public class MovePointExample : MonoBehaviour {

    public BezierMaster.BezierMaster bezier;
    public float speed = 5f;
    public int pathResolution = 20;
    public Transform lookAtNextTarget;
    public float lookSpeed = 10;

    Vector3[] points;
    int n = 0;

	// Use this for initialization
	void Start () {
        points = bezier.GetPath(pathResolution);

    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, points[n], speed * Time.deltaTime);

        if(transform.position == points[n])
        {
            n++;

            if (n == points.Length)
                n = 0;
        }
	}

    private void LateUpdate()
    {
        lookAtNextTarget.LookAt(points[n]);
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, lookAtNextTarget.eulerAngles, 0.9f * Time.deltaTime * lookSpeed);
    }
}
