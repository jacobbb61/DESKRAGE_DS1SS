using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelTrigger : MonoBehaviour
{

    public bool LevelEnd = false;
    public float EndAnimLenght;
    public Animator EndOfLevelAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(EndLevel());
        }
    }


    IEnumerator EndLevel()
    {
        LevelEnd = true;
        //Player Inputs OFF
        //Trigger EndLevel Animation
        //Trigger EndLevel Audio
        yield return new WaitForSeconds(EndAnimLenght); //Wait for end of level anim to fade to black
        SceneManager.LoadScene("End Credits", LoadSceneMode.Single);
    }
}
