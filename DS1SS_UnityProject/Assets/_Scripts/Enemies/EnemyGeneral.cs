using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyGeneral : MonoBehaviour
{
    public Vector3 OriginPosition;
    public UnityEvent RespawnEvent;

    public void Respawn()
    {
        RespawnEvent.Invoke();
    }

}
