using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelTrigger : MonoBehaviour
{

    public bool LevelEnd = false;
    public float EndAnimLenght;
    public float EndAnimLenght2;
    public Animator SceneTransitionAnim;
    public PlayerManager PM;
    public PlayerControllerV2 PC;
    public PlayerMenuManager PUI;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {


            if (PM.HasBeenHit == false && PM.HasDied == false)
            {
                AchievementsGameManager.Instance.UnlockedAchievement(5);
                EndAnimLenght2 = 7f;
                StartCoroutine(Awesome());
            }
            else
            {
                if (PM.HasBeenHit == false)
                {
                    AchievementsGameManager.Instance.UnlockedAchievement(5);
                    EndAnimLenght2 = 3.5f;
                }
                if (PM.HasDied == false)
                {
                    AchievementsGameManager.Instance.UnlockedAchievement(6);
                    EndAnimLenght2 = 3.5f;
                }

            }
            StartCoroutine(EndLevel());
        }
    }
    IEnumerator Awesome()
    {
        
        yield return new WaitForSeconds(3.5f);
        AchievementsGameManager.Instance.UnlockedAchievement(6);
        EndAnimLenght2 = 0;
    }

        IEnumerator EndLevel()
    {
        PC.State = "Idle";
        PC.CanAttack = false;
        PC.CanFollowUp = false;
        PC.CanMove = false;
        PUI.CanInput = false;
        PM.FinishedGame = true;

        yield return new WaitForSeconds(EndAnimLenght2);

        SceneTransitionAnim.SetTrigger("Exit");

        WorldSaveGameManager.Instance.CurrentCharacterData.FinishedGame = true;
        WorldSaveGameManager.Instance.SaveGame();


        yield return new WaitForSeconds(EndAnimLenght); //Wait for end of level anim to fade to black
        SceneManager.LoadScene("End Credits", LoadSceneMode.Single);
    }

}
