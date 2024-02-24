using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstusPickUpManager : MonoBehaviour
{

    public bool EstusPickedUp_1;
    public bool EstusPickedUp_2;
    public bool EstusPickedUp_3;
    public bool EstusPickedUp_4;
    public bool EstusPickedUp_5;
    public bool EstusPickedUp_6;

    public EstusPickUp EstusPickUp_1;
    public EstusPickUp EstusPickUp_2;
    public EstusPickUp EstusPickUp_3;
    public EstusPickUp EstusPickUp_4;
    public EstusPickUp EstusPickUp_5;
    public EstusPickUp EstusPickUp_6;

    public void ManualStart()
    {
        if (EstusPickedUp_1)
        {
            EstusPickUp_1.Remove();
        }
        if (EstusPickedUp_2)
        {
            EstusPickUp_2.Remove();
        }
        if (EstusPickedUp_3)
        {
            EstusPickUp_3.Remove();
        }
        if (EstusPickedUp_4)
        {
            EstusPickUp_4.Remove();
        }
        if (EstusPickedUp_5)
        {
            EstusPickUp_5.Remove();
        }
        if (EstusPickedUp_6)
        {
            EstusPickUp_6.Remove();
        }
    }


}
