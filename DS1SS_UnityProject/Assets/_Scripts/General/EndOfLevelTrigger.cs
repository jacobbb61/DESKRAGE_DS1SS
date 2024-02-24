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

            PC.MyRb.velocity = Vector2.zero;

            if (PM.HasBeenHit == false && PM.HasDied == false && GameSaveGameManager.Instance.GameSaveData.Achievement_5==false && GameSaveGameManager.Instance.GameSaveData.Achievement_6 == false)
        {
                AchievementsGameManager.Instance.UnlockedAchievement(5);
                EndAnimLenght2 = 2.5f;
                StartCoroutine(Awesome());
                Debug.Log("Unlocked Both");
                return;
            }
            else
            {
                if (PM.HasBeenHit == false && GameSaveGameManager.Instance.GameSaveData.Achievement_6 == false)
                {
                    AchievementsGameManager.Instance.UnlockedAchievement(6);
                    EndAnimLenght2 = 3.5f;
                StartCoroutine(EndLevel());
                Debug.Log("Unlocked 6");
                return;
            }
                if (PM.HasDied == false && GameSaveGameManager.Instance.GameSaveData.Achievement_5 == false)
                {
                    AchievementsGameManager.Instance.UnlockedAchievement(5);
                    EndAnimLenght2 = 3.5f;
                StartCoroutine(EndLevel());
                Debug.Log("Unlocked 5");
                return;
            }

            }

        Debug.Log("Unlocked None");
        StartCoroutine(EndLevel());
        
    }
    IEnumerator Awesome()
    {
        
        yield return new WaitForSeconds(3.5f);
        AchievementsGameManager.Instance.UnlockedAchievement(6);
        StartCoroutine(EndLevel());
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
