using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public DoorManager DoorManagerToUnlock = null;


    public void UnlockOtherDoor()
    {
       if (DoorManagerToUnlock != null) DoorManagerToUnlock.CurrentDoorState_This = "Open";
    }
}
