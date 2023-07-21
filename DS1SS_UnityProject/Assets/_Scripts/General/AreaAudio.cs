using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAudio : MonoBehaviour
{
    public GameObject AreaPointAudio;
    public GameObject LeftRestPos;
    public GameObject RightRestPos;
    public GameObject Player;
    public LayerManagerV2 LM;
    public int CurrentLayer;
    public bool InAudio;
    public bool Checked;

    private Collider2D ThisCol;
    
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        LM = GameObject.FindGameObjectWithTag("LayerManager").GetComponent<LayerManagerV2>();
        ThisCol = GetComponent<Collider2D>();
        CheckIfInside();

    }

    private void OnEnable()
    {
        CheckIfInside();
    }

    private void LateUpdate()
    {
        if (InAudio) { StayInAudio(); }
        if (LM.CurrentLayerNumber != CurrentLayer) { ExitAudio(); }
        else if (LM.CurrentLayerNumber == CurrentLayer && Checked==false) { CheckIfInside(); }
    }

    public void CheckIfInside()
    {
        if (ThisCol.OverlapPoint(Player.transform.position))
        {
            EnterAudio();
            Debug.Log("player in bounds");
        }
        else
        {
            ExitAudio();
            Debug.Log("player not in bounds");
        }
        Checked = true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && LM.CurrentLayerNumber == CurrentLayer)
        {
            EnterAudio();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && LM.CurrentLayerNumber == CurrentLayer)
        {
            ExitAudio();
        }
    }


    public void EnterAudio()
    {
        AreaPointAudio.transform.parent = Player.transform;
        if (LM.CurrentLayerNumber == CurrentLayer) { AreaPointAudio.SetActive(true); }
        InAudio = true;
    }
    public void ExitAudio()
    {
        AreaPointAudio.transform.parent = transform;
        if (LM.CurrentLayerNumber != CurrentLayer) { AreaPointAudio.SetActive(false); }
        InAudio = false;
        Checked = false;

        //place audio at the side the player is on
        if(transform.position.x < Player.transform.position.x)
        {
            AreaPointAudio.transform.position = RightRestPos.transform.position;
        }
        else
        {
            AreaPointAudio.transform.position = LeftRestPos.transform.position;
        }
    } 
    public void StayInAudio()
    {
       AreaPointAudio.transform.position = Player.transform.position;
    }
}
