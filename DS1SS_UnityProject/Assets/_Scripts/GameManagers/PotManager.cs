using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotManager : MonoBehaviour
{
    public Pot[] Pots;

    private void Start()
    {
        Pots = GetComponentsInChildren<Pot>();
    }
    public void ReloadPots()
    {
        foreach(Pot pot in Pots)
        {
            pot.Broken=false;
            pot.gameObject.SetActive(true);
            if (gameObject.activeInHierarchy)
            {
                pot.ResetPot();
            }
        }
    }
}
