using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputDeviceManager : MonoBehaviour
{
    public GameObject[] PCIcons;
    public GameObject[] XboxIcons;
    public GameObject[] PSIcons;

    public string Device;

    void Awake()
    {
        PlayerInput input = FindObjectOfType<PlayerInput>();
        updateButtonImage(input.currentControlScheme);
    }

    void OnEnable()
    {
        InputUser.onChange += onInputDeviceChange;
    }

    void OnDisable()
    {
        InputUser.onChange -= onInputDeviceChange;
    }


    void onInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change == InputUserChange.ControlSchemeChanged && Device=="Auto")
        {
            updateButtonImage(user.controlScheme.Value.name);
        }
    }
    void updateButtonImage(string schemeName)
    {
        switch (schemeName)
        {
            case "Xbox Controller":
                SwitchToXbox();
                break;
            case "Gamepad":
                SwitchToXbox();
                break;
            default:
                SwitchToPC();
                break;
        }
    }








    private void Start()
    {
        Device = GameSaveGameManager.Instance.GameSaveData.Controls;


        switch (Device)
        {
            case "Auto":
                SwitchToPC();
                break;
            case "KeyBoard":
                SwitchToPC();
                break;
            case "Xbox":
                SwitchToXbox();
                break;
            case "PlayStation":
                SwitchToPS();
                break;
            default:
                Device = "Auto";
                SwitchToPC();
                break;
        }
    }
    public void SwitchToAuto()
    {
        Device = "Auto";
        foreach (GameObject icon in PCIcons)
        {
            icon.SetActive(true);
        }
        foreach (GameObject icon in XboxIcons)
        {
            icon.SetActive(false);
        }
        foreach (GameObject icon in PSIcons)
        {
            icon.SetActive(false);
        }
    }
    public void SwitchToPC()
    {
        if (Device!="Auto")  Device = "KeyBoard";
        Debug.Log("Switch to KeyBoard");
        foreach (GameObject icon in PCIcons)
        {
            icon.SetActive(true);
        }
        foreach (GameObject icon in XboxIcons)
        {
            icon.SetActive(false);
        }
        foreach (GameObject icon in PSIcons)
        {
            icon.SetActive(false);
        }
    }
    public void SwitchToXbox()
    {
        if (Device != "Auto") Device = "Xbox";
        Debug.Log("Switch to Xbox");
        foreach (GameObject icon in PCIcons)
        {
            icon.SetActive(false);
        }
        foreach (GameObject icon in XboxIcons)
        {
            icon.SetActive(true);
        }
        foreach (GameObject icon in PSIcons)
        {
            icon.SetActive(false);
        }
    }
    public void SwitchToPS()
    {
        if (Device != "Auto") Device = "PlayStation";
        Debug.Log("Switch to PlayStation");
        foreach (GameObject icon in PCIcons)
        {
            icon.SetActive(false);
        }
        foreach (GameObject icon in XboxIcons)
        {
            icon.SetActive(false);
        }
        foreach (GameObject icon in PSIcons)
        {
            icon.SetActive(true);
        }
    }



}
