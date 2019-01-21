using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject targetTransform;
    public float cameraSpeed = 5;
    public float cameraZoneX = 3;
    public float cameraZoneY = 1.75f;
    PlayerController pc;

    public Animator anim;

    Vector3 newEulerAngles = Vector3.zero;
    int lsdCount = 0;
    int sharkCount = 0;

    private void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    /*
    private void LateUpdate()
    {
        GetRotation();
        transform.localEulerAngles = newEulerAngles;
    }

    void GetRotation()
    {
        float x = pc.transform.position.x;
        float y = pc.transform.position.y;

        //Camera's horizontal turn
        if (x < -cameraZoneX)
        {
            float newY = Mathf.LerpAngle(newEulerAngles.y, -25, 0.9f * Time.deltaTime * cameraSpeed);
            newEulerAngles = new Vector3(newEulerAngles.x, newY, newEulerAngles.z);

        }
        else if (x >= -cameraZoneX && x < cameraZoneX)
        {
            float newY = Mathf.LerpAngle(newEulerAngles.y, 0, 0.9f * Time.deltaTime * cameraSpeed);
            newEulerAngles = new Vector3(newEulerAngles.x, newY, newEulerAngles.z);
        }
        else if (x >= cameraZoneX)
        {
            float newY = Mathf.LerpAngle(newEulerAngles.y, 25, 0.9f * Time.deltaTime * cameraSpeed);
            newEulerAngles = new Vector3(newEulerAngles.x, newY, newEulerAngles.z);
        }

        //Camera's vertical turn
        if (y < -cameraZoneY)
        {
            float newX = Mathf.LerpAngle(newEulerAngles.x, 13f, 0.9f * Time.deltaTime * cameraSpeed);
            newEulerAngles = new Vector3(newX, newEulerAngles.y, newEulerAngles.z);

        }
        else if (y >= -cameraZoneY && y < cameraZoneY)
        {
            float newX = Mathf.LerpAngle(newEulerAngles.x, 0, 0.9f * Time.deltaTime * cameraSpeed);
            newEulerAngles = new Vector3(newX, newEulerAngles.y, newEulerAngles.z);
        }
        else if (y >= cameraZoneY)
        {
            float newX = Mathf.LerpAngle(newEulerAngles.x, -13, 0.9f * Time.deltaTime * cameraSpeed);
            newEulerAngles = new Vector3(newX, newEulerAngles.y, newEulerAngles.z);
        }
    }
    */

    public void SetLSD(int _lsd)
    {
        lsdCount += _lsd;
        anim.SetInteger("LSD", lsdCount);
    }

    public void SetShark(int _shark)
    {
        sharkCount += _shark;
        anim.SetInteger("Shark", sharkCount);
    }
}