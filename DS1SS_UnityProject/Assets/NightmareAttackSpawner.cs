using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareAttackSpawner : MonoBehaviour
{
    public GameObject Projectile;
    public int Spawned;
    public int SpawnedTotal;
    public float SpawnedTime;
    public bool stop = false;

    public void Start()
    {
        
    }


    public void spawn()
    {
        stop = false;
        GameObject spawnedProj = Instantiate(Projectile);

        float ran = Random.Range(-10, 10);

        spawnedProj.transform.parent = gameObject.transform;
        spawnedProj.transform.localPosition = new Vector3(ran, 0, 0);

        spawnedProj.GetComponent<ArrowV2>().Target = new Vector3(0,-20, 0);


        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(SpawnedTime);
        if (!stop) { spawn(); }
    }

}
