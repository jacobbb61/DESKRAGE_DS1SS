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

    private void Start()
    {
        Device = GameSaveGameManager.Instance.GameSaveData.Controls;
        switch (Device)
        {
            case "Keyboard":
                SwitchToPC();
                break;
            case "Xbox":
                SwitchToXbox();
                break;
            case "PlayStation":
                SwitchToPS();
                break;
            default:
                Device = "KeyBoard";
                SwitchToPC();
                break;
        }
    }
    public void SwitchToPC()
    {
        Device = "Keyboard"; 

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
        Device = "Xbox";

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
        Device = "PlayStation";

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
