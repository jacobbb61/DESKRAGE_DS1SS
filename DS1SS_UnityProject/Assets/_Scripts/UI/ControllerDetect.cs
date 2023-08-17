using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDetect : MonoBehaviour
{
    public string[] joystickNames;
    private InputDeviceManager IDM;
    /*public enum controllerType {PC, Xbox, PS};
    public controllerType activeControllerType;*/

    private void Start()
    {
        IDM = GetComponent<InputDeviceManager>();
        CheckInputType();
    }

    /*private void Update()
    {
        if (Input.anyKey)
        {
            CheckInputType();
        }
    }*/

    public void CheckInputType()
    {
        joystickNames = Input.GetJoystickNames();


        if (joystickNames.Length == 0)
        {
            IDM.Device = "PC";
            IDM.SwitchToPC();
            Debug.Log("No controller detected");
        }
        else
        {
            foreach (string joystickName in joystickNames)
            {
                if (joystickName != string.Empty)
                {
                    Debug.Log("Controller connected: " + joystickName);
                }

                if (joystickName.ToLower().Contains("xbox") || joystickName.ToLower().Contains("x-box"))
                {
                    IDM.Device = "XBox";
                    IDM.SwitchToXbox();

                }
                else if (joystickName.ToLower().Contains("ps4") || joystickName.ToLower().Contains("playstation") || joystickName.ToLower().Contains("wireless controller"))
                {
                    IDM.Device = "PS";
                    IDM.SwitchToPC();
                }
            }
        }

        Debug.Log(IDM.Device);
    }
}
