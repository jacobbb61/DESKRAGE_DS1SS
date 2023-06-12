using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Bonfire : MonoBehaviour
{
    private PlayerItems playerItems;

    // Start is called before the first frame update
    void Start()
    {
        playerItems = FindObjectOfType<PlayerItems>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseBonfire()
    {
        // Play animation
        // Play sound
        // Heal player
        playerItems.estusFlasks = playerItems.maxFlasks;
    }
}
