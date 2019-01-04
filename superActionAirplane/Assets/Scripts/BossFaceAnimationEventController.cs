using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFaceAnimationEventController : MonoBehaviour
{
    public List<Drone_1Controller> eyesOpenLeftDrone_1Controllers = new List<Drone_1Controller>();
    public List<Drone_1Controller> eyesOpenRightDrone_1Controllers = new List<Drone_1Controller>();
    public List<Drone_1Controller> mouthDrone_1Controllers = new List<Drone_1Controller>();
    public List<Drone_1Controller> brainControllers = new List<Drone_1Controller>();
    public List<GameObject> faceBlocks = new List<GameObject>();

    public List<Destructible> allDrones = new List<Destructible>();

    private void Start()
    {
        foreach (Drone_1Controller drone in brainControllers) // set brain inactive
        {
            drone.gameObject.SetActive(false);
        }
    }
    public void EyesOpen(int can ) // 0 - left is open; 1 - right is open
    {
        foreach (Drone_1Controller drone in eyesOpenLeftDrone_1Controllers)
        {
            if (can == 0)
                DroneCanShoot(drone, true);
            else
                DroneCanShoot(drone, false);
        }
        foreach (Drone_1Controller drone in eyesOpenRightDrone_1Controllers)
        {
            if (can == 0)
                DroneCanShoot(drone, false);
            else
                DroneCanShoot(drone, true);
        }
    }

    public void EyesClosed()
    {
        foreach (Drone_1Controller drone in eyesOpenLeftDrone_1Controllers)
        {
            DroneCanShoot(drone, false);
        }
        foreach (Drone_1Controller drone in eyesOpenRightDrone_1Controllers)
        {
            DroneCanShoot(drone, false);
        }
    }
    public void EyesOpenBoth()
    {
        foreach (Drone_1Controller drone in eyesOpenLeftDrone_1Controllers)
        {
            DroneCanShoot(drone, true);
        }
        foreach (Drone_1Controller drone in eyesOpenRightDrone_1Controllers)
        {
            DroneCanShoot(drone, true);
        }
    }

    void DroneCanShoot(Drone_1Controller drone, bool can)
    {
        if (drone.gameObject.activeInHierarchy)
            drone.SetCanShoot(can);
    }

    public void MouthOpen()
    {
        foreach (Drone_1Controller drone in mouthDrone_1Controllers)
        {
            DroneCanShoot(drone, true);
        }
    }

    public void MouthClosed()
    {
        foreach (Drone_1Controller drone in mouthDrone_1Controllers)
        {
            DroneCanShoot(drone, false);
        }
    }

    public void SetMouthInactive()
    {
        print("mouthDronesInactive");
        foreach (Drone_1Controller drone in mouthDrone_1Controllers)
        {
            drone.gameObject.SetActive(false);
        }
    }

    public void SetMouthActive()
    {
        print("mouthDronesActive");
        foreach (Drone_1Controller drone in mouthDrone_1Controllers)
        {
            drone.gameObject.SetActive(true);
        }
    }

    public void DronesInvincible()
    {
        foreach(Destructible drone in allDrones)
        {
            drone.SetInvincible(true);
        }
    }
    public void DronesUninvincible()
    {
        foreach (Destructible drone in allDrones)
        {
            drone.SetInvincible(false);
        }
    }

    public void BrainSetActive()
    {
        foreach (Drone_1Controller drone in brainControllers)
        {
            drone.gameObject.SetActive(true);
        }
        foreach (GameObject go in faceBlocks)
        {
            go.SetActive(false);
        }
    }
}