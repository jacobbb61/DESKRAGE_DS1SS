using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelTrigger : MonoBehaviour
{

    public bool LevelEnd = false;
    public float EndAnimLenght;
    public Animator SceneTransitionAnim;
    public PlayerManager PM;
    public PlayerControllerV2 PC;
    public PlayerMenuManager PUI;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(EndLevel());
        }
    }


    IEnumerator EndLevel()
    {
        PC.CanAttack = false;
        PC.CanFollowUp = false;
        PC.CanMove = false;
        PUI.CanInput = false;
        PM.FinishedGame = true;

        SceneTransitionAnim.SetTrigger("Exit");

        WorldSaveGameManager.Instance.SaveGame();


        yield return new WaitForSeconds(EndAnimLenght); //Wait for end of level anim to fade to black
        SceneManager.LoadScene("End Credits", LoadSceneMode.Single);
    }

}
