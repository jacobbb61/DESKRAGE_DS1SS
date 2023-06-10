using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    public List<GameObject> weapons;
    public List<GameObject> armor;
    public List<GameObject> keys;
    public int estusFlasks = 0;
    private int maxFlasks = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (estusFlasks > maxFlasks)
        {
            estusFlasks = maxFlasks;
        }
    }
}
