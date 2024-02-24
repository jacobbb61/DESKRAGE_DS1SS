using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameOnEnter : MonoBehaviour
{
    public PlayerManager playerManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        WorldSaveGameManager.Instance.Player = playerManager;
        WorldSaveGameManager.Instance.SaveGame();
    }
}
