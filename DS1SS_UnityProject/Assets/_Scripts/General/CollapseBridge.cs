using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseBridge : MonoBehaviour
{
    [SerializeField] private GameObject bridge;
    //private Rigidbody2D rb;
    public string currentState;
  
    //private Rigidbody2D playerRB;
    public Collider2D Collider;
    public DoorOcludingSection DoorOcludingSection;
    [Tooltip("True = this gameobject is a bridge. False = this gameobject is a collapsable floor.")]public bool isBridge;

    public GameObject FloorAssets_UnBroken;
    public GameObject FloorAssets_Broken1;
    public GameObject FloorAssets_Broken2;
    public GameObject FloorAssets_BrokenCompletely;

    public GameObject BridgeAssets_UnBroken;
    public GameObject BridgeAssets_Broken;

    public PursuerArena PursuerArena;
    public Animator Anim;

    private bool CanBreak; //wait for time when loading into scene before player can trigger break

    // The bridge referenced in this script must have a collider and kinematic rigidbody

    public void ManualStart()
    {
        if (!isBridge)
        {
            StartCoroutine(WaitForLoad());
            switch (currentState)
            {
                case "UnBroken":
                    assetsOff();
                    FloorAssets_UnBroken.SetActive(true);
                    break;
                case "Breaking1":
                    assetsOff();
                    FloorAssets_Broken1.SetActive(true);
                    break;
                case "Breaking2":
                    assetsOff();
                    FloorAssets_Broken2.SetActive(true);
                    break;
                case "Broken":
                    assetsOff();
                    FloorAssets_BrokenCompletely.SetActive(true);
                    DoorOcludingSection.Open();
                    break;

            }
        }
        else
        {
            switch (currentState)
            {
                case "Closed":
                    BridgeAssets_UnBroken.SetActive(true);
                    BridgeAssets_Broken.SetActive(false);
                    Anim.Play("UnbrokenIdle");
                    break;
                case "Open":
                    BridgeAssets_Broken.SetActive(true);
                    BridgeAssets_UnBroken.SetActive(false); 
                    Anim.Play("BrokenIdle");
                    break;
            }
        }
    }

    IEnumerator WaitForLoad()
    {
        CanBreak = false;
        yield return new WaitForSeconds(1.5f);
        CanBreak = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBridge && CanBreak/* && playerRB.velocity.y < 0*/)
        {

            switch (currentState)
            {
                case "UnBroken":
                    assetsOff();
                    currentState = "Breaking1";
                    FloorAssets_Broken1.SetActive(true);
                    break;
                case "Breaking1":
                    assetsOff();
                    currentState = "Breaking2";
                    FloorAssets_Broken2.SetActive(true);
                    break;
                case "Breaking2":
                    assetsOff();
                    currentState = "Broken";
                    FloorAssets_BrokenCompletely.SetActive(true);
                    DoorOcludingSection.RevealArea();
                    Collider.enabled = false;
                    break;
            }
        }
        else if (collision.CompareTag("Player") && isBridge)
        {
            PursuerArena.EnterArena();
        }
    }


    public IEnumerator WaitToBreak()
    {
        yield return new WaitForSeconds(1);
    
        Collider.enabled = false; 
        
        yield return new WaitForSeconds(3);
   
        currentState = "Open";
        Anim.Play("BrokenIdle");

    }



    void assetsOff()
    {
        FloorAssets_UnBroken.SetActive(false);
        FloorAssets_Broken1.SetActive(false);
        FloorAssets_Broken2.SetActive(false);
        FloorAssets_BrokenCompletely.SetActive(false);
    }


}
