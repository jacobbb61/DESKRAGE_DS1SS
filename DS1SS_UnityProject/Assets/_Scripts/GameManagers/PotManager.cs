using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotManager : MonoBehaviour
{
    public Pot[] Pots;

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
