using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct Memory
{
    public bool booleanMemory;
    public int integerMemory;
    public string stringMemory;
    public float floatMemory;
    public Vector2 vector2Memory;
}


public class MemoryTarget : MonoBehaviour
{
    public string rename;
    public Memory mem;
    public UnityEvent<bool> boolMem;
    public UnityEvent<int> intMem;
    public UnityEvent<string> stringMem;
    public UnityEvent<float> floatMem;
    public UnityEvent<Vector2> vector2Mem;

    //Awake is called before Start
    private void Awake()
    {
        //load stuff in awake on the loading script
    }

    // Start is called before the first frame update
    void Start()
    {
        boolMem.Invoke(mem.booleanMemory);
        intMem.Invoke(mem.integerMemory);
        stringMem.Invoke(mem.stringMemory);
        floatMem.Invoke(mem.floatMemory);
        vector2Mem.Invoke(mem.vector2Memory);
        Debug.Log(rename);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
