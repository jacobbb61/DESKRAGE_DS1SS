using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCreditsManager : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(EndCredits());
    }

    IEnumerator EndCredits()
    {
        Debug.Log("A");
        yield return new WaitForSeconds(120f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
