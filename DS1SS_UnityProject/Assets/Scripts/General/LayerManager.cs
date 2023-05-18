using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class LayerManager : MonoBehaviour
{
    [SerializeField] private List<Layer> layers;
    [Tooltip("Set this to the initial starting layer or perish")]
    [SerializeField] private int activeLayer = -1;
    public UnityEvent OnLayerSwitch;

    //These are set in inspector
    [Tooltip("Size the layer shrinks to when moved back")]
    [SerializeField] private float backgroundScale = 0.5f;
    [Tooltip("Time taken to move layers when switched")]
    [SerializeField] private float transitionTime = 0.5f;

    //These are unchangable except via this script or the editor
    public float relativeLayerScale { get { return backgroundScale; } private set { backgroundScale = value;  } }
    public float transitionLength { get { return transitionTime; } private set { transitionTime = value; } }

    //Awake is called before start
    private void Awake()
    {
        //Start Section "Replace when load save" (move this idea to the start of start so load can be in awake)
        if (activeLayer < 0 || activeLayer >= layers.Count)
        {
            Debug.LogWarning("Warning: LayerManager appears to be set up incorrectly, layer set to backmost as a default");
            activeLayer = 0;
            if (layers.Count == 0)
            {
                Debug.LogError("Error: No Layers set up. Forcing stop.");

#if UNITY_EDITOR
                EditorApplication.isPlaying = false;//unplays your editor
#endif

                Application.Quit();
            }
        }
        //End Section "Replace when load save"
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            int dif = activeLayer - i;
            StartCoroutine(layers[i].Transition(0.001f, Mathf.Pow(relativeLayerScale, dif), dif < 0 ? 0f : 1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!layers[0].runningCR)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeLayer(activeLayer - 1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeLayer(activeLayer + 1);
            }
        }
    }

    [Tooltip("Change the 'active' group of objects")]
    public void ChangeLayer([Tooltip("layer index of the new active layer")] int target)
    {
        int direction = activeLayer - target;
        if (target < 0 || target >= layers.Count)
        {
            Debug.LogWarning("Attempted to move to out of bounds layer");
            return;
        }
        for (int i = 0; i < layers.Count; i++)
        {
            if (target > i)
            {
                layers[i].SetInactiveAndHide(direction);
            }
            else if (target == i)
            {
                layers[i].SetActive(direction);
            }
            else if (target < i)
            {
                layers[i].SetInactiveAndBehind(direction);
            }
            else
            {
                //PANIC
            }
        }
        activeLayer = target;
        OnLayerSwitch.Invoke();
    }

    [Tooltip("Change the 'active' group of objects to the frontmost")]
    public void ChangeLayer([Tooltip("Name of layer GameObject")] string target)
    {
        for (int i = 0; i < layers.Count; i++)
        {
            if (layers[i].name == target)
            {
                ChangeLayer(i);
                return;
            }
        }
    }


    //Get the Layer that a gameObject is on by recursively checking parents
    public int GetLayerFromObject(GameObject target)
    {
        Transform thing = target.transform;
        Layer answer;
        while (thing != null)
        {
            if (target.TryGetComponent<Layer>(out answer))
            {
                return layers.FindIndex(i => i.Equals(answer));
            }
            else
            {
                thing = thing.parent;
            }
        }
        Debug.LogWarning("LayerNotFound");
        return -1;
    }

}
