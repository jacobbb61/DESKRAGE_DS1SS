using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDeviceManager : MonoBehaviour
{
    public GameObject[] PCIcons;
    public GameObject[] XboxIcons;
    public GameObject[] PSIcons;

    public string Device;

    private void Start()
    {
        switch (Device)
        {
            case "PC":
                SwitchToPC();
                break;
            case "Xbox":
                SwitchToXbox();
                break;
            case "PS":
                SwitchToPS();
                break;
            default:
                Device = "PC";
                SwitchToPC();
                break;
        }
    }

    public void SwitchToPC()
    {
        foreach(GameObject icon in PCIcons)
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
