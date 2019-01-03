using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputController : MonoBehaviour
{

    public Joystick joystick;
    public UiButtonController uiButtonController;

    private void Start()
    {
        GameManager.instance.touchInputController = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}