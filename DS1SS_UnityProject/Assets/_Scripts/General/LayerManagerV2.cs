using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class LayerManagerV2 : MonoBehaviour
{
    public int CurrentLayerNumber;
    public string CurrentLayer;

    public GameObject BackLayer;
    public GameObject MiddleLayer;
    public GameObject FrontLayer;
    public GameObject Player;

    private PlayerControllerV2 PC;



    private void Start()
    {
        PC = Player.GetComponent<PlayerControllerV2>();
    }

    public void ChangeLayer(int layer)
    {
        StartCoroutine(PlayerLayer(layer));

       if(Player.GetComponent<EnemyLock>().LockedOn)
        {
            Player.GetComponent<EnemyLock>().LockedOn = false;
            PC.IsLockedOn = Player.GetComponent<EnemyLock>().LockedOn;
            Player.GetComponent<EnemyLock>().LockOnSymbol.SetActive(Player.GetComponent<EnemyLock>().LockedOn);
        }
    }


    public void LoadLayer(string layer)
    {
        switch (layer)
        {
            case "Back":
                BackLayer.SetActive(true);
                MiddleLayer.SetActive(false);
                FrontLayer.SetActive(false);
                Player.GetComponent<SortingGroup>().sortingLayerName = "BackSortingLayer";
                CurrentLayer = "Back";
                break;

            case "Middle":
                BackLayer.SetActive(false);
                MiddleLayer.SetActive(true);
                FrontLayer.SetActive(false);
                Player.GetComponent<SortingGroup>().sortingLayerName = "MiddleSortingLayer";
                CurrentLayer = "Middle";
                break;

            case "Front":
                BackLayer.SetActive(false);
                MiddleLayer.SetActive(false);
                FrontLayer.SetActive(true);
                Player.GetComponent<SortingGroup>().sortingLayerName = "FrontSortingLayer";
                CurrentLayer = "Front";
                break;
            default:
                LoadLayer("Middle");
                break;
        }
    }
    public void EnviornmentLayer(int layer)
    {
        switch (layer)
        {
            case 0:
                BackLayer.SetActive(true);
                MiddleLayer.SetActive(false);
                FrontLayer.SetActive(false);
                Player.GetComponent<SortingGroup>().sortingLayerName = "BackSortingLayer";
                CurrentLayer = "Back";
                break;

            case 1:
                BackLayer.SetActive(false);
                MiddleLayer.SetActive(true);
                FrontLayer.SetActive(false);
                Player.GetComponent<SortingGroup>().sortingLayerName = "MiddleSortingLayer";
                CurrentLayer = "Middle";
                break;

            case 2:
                BackLayer.SetActive(false);
                MiddleLayer.SetActive(false);
                FrontLayer.SetActive(true);
                Player.GetComponent<SortingGroup>().sortingLayerName = "FrontSortingLayer";
                CurrentLayer = "Front";
                break;
        }
    }

    public IEnumerator PlayerLayer(int layer)
    {
        PC.CanMove = false;
        PC.IsGrounded = true;
        PC.IsRunning = false;
        PC.IsRolling = false;
        PC.IsMovingInput = false;
        PC.MyRb.velocity = Vector2.zero;
        PC.MyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        PC.Speed=0f;
        PC.VerticalSpeed=0f;
        //trigger anim to change layer
        yield return new WaitForSeconds(.25f);
        EnviornmentLayer(layer);
        yield return new WaitForSeconds(.25f);
        //give back control
        PC.MyRb.constraints = RigidbodyConstraints2D.None;
        PC.MyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        PC.CanMove = true;
    }
}
