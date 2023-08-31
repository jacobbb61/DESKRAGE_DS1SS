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
        if (IDM.Device == "Auto")
        {
            CheckInputType();
        }
    }

    private void LateUpdate()
    {
        if (IDM.Device == "Auto")
        {
            CheckInputType();
        }
    }

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
                    IDM.Device = "Xbox";
                    IDM.SwitchToXbox();

                }
                else if (joystickName.ToLower().Contains("ps4") || joystickName.ToLower().Contains("playstation") || joystickName.ToLower().Contains("wireless controller") || joystickName.ToLower().Contains("dualshock"))
                {
                    IDM.Device = "PS";
                    IDM.SwitchToPS();
                }
                else
                {
                    IDM.Device = "Xbox";
                    IDM.SwitchToXbox();
                }
            }
        }

        Debug.Log(IDM.Device);
    }
}
