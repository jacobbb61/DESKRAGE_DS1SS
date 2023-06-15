using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroudCheck : MonoBehaviour
{
    public bool grounded;
    public float groundSlope; //slope of the "current" "ground" collider

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!other.isTrigger && other.gameObject.tag!="Player" && other.gameObject.layer == gameObject.layer)
        {
            grounded = true;
            groundSlope = other.gameObject.transform.rotation.z;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        grounded = false;
        groundSlope = 0;
    }
}
