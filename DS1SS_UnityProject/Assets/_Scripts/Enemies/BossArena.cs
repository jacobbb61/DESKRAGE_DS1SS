using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArena : MonoBehaviour
{
    [SerializeField] DoorManager[] doors;
    public bool bossDead = false;

    public bool InBossFight;

    public bool ArenaIsActive;

    private void Update()
    {
        if (bossDead)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                // Audio stuffs
                // Wait(time);
                // Disable boss health
                // Achievement and saving stuff
                doors[i].inBossFight = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].inBossFight = true;
            }

            // Display boss health
            // Play boss music
            // Achievement and saving stuff
        }
    }

    IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }
}
