using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractableV2 : MonoBehaviour
{
    public UnityEvent Event;

    public void InvokeEvent()
    {
        Event.Invoke();
    }
}
