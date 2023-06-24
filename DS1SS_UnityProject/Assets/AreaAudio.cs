using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAudio : MonoBehaviour
{
    public GameObject AreaPointAudio;
    public GameObject Player;
    public LayerManagerV2 LM;
    public int CurrentLayer;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        LM = GameObject.FindGameObjectWithTag("LayerManager").GetComponent<LayerManagerV2>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && LM.CurrentLayerNumber == CurrentLayer)
        {
            AreaPointAudio.transform.parent = Player.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && LM.CurrentLayerNumber == CurrentLayer)
        {
            AreaPointAudio.transform.parent = transform;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && LM.CurrentLayerNumber == CurrentLayer)
        {
            AreaPointAudio.transform.position = Player.transform.position;
        }
    }
}
