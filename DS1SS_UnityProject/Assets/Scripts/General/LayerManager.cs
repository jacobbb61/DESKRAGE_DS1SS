using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class LayerManager : MonoBehaviour
{
    public static LayerManager instance { get; private set; }
    [SerializeField] private List<Layer> layers;
    [Tooltip("Set this to the initial starting layer or perish")]
    [SerializeField] private int internalActiveLayer = -1;

    [Tooltip("This Event is called at the start of the transition, and passes the duration into functions in it")]
    public UnityEvent<float> OnLayerSwitch;

    public int activeLayer { get { return internalActiveLayer; } private set { internalActiveLayer = value; } }

    //These are set in inspector
    [Tooltip("Size the layer shrinks to when moved back")]
    [SerializeField] private float backgroundScale = 0.5f;
    [Tooltip("Time taken to move layers when switched")]
    [SerializeField] private float transitionTime = 0.5f;

    //These are unchangable except via this script or the editor
    public float relativeLayerScale { get { return backgroundScale; } private set { backgroundScale = value; } }
    public float transitionLength { get { return transitionTime; } private set { transitionTime = value; } }

    //Awake is called before start
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple Layer Managers Detected");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;//unplays your editor
#endif

            Application.Quit();
        }
        //Start Section "Replace when load save" (move this idea to the start of start so load can be in awake)
        if (internalActiveLayer < 0 || internalActiveLayer >= layers.Count)
        {
            Debug.LogWarning("Warning: LayerManager appears to be set up incorrectly, layer set to backmost as a default");
            internalActiveLayer = 0;
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
            int dif = internalActiveLayer - i;
            StartCoroutine(layers[i].Transition(0.001f, Mathf.Pow(relativeLayerScale, dif), dif < 0 ? 0f : 1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //remove when doors only
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeLayer(internalActiveLayer - 1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeLayer(internalActiveLayer + 1);
        }
        //end remove when doors only
    }

    [Tooltip("Change the 'active' group of objects")]
    public void ChangeLayer([Tooltip("layer index of the new active layer")] int target)
    {
        if (!layers[0].runningCR)
        {
            int direction = internalActiveLayer - target;
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
            internalActiveLayer = target;
            OnLayerSwitch.Invoke(transitionTime);
        }
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

    public Layer GetLayer(int index)
    {
        if(index < layers.Count && index >=0)
        {
            return layers[index];
        }
        Debug.LogError("Attempted to get nonexistant Layer");
        return null;
    }

    public Layer GetLayer()
    {
        return layers[activeLayer];
    }

    public bool TryGetLayer(out Layer result, int index)
    {
        if (index < layers.Count && index >= 0)
        {
            result = layers[index];
            return true;
        }
        Debug.LogError("Attempted to get nonexistant Layer");
        result = null;
        return false;
    }
}
