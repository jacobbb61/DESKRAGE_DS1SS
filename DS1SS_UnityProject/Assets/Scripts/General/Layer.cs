using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Layer : MonoBehaviour
{
    [SerializeField] private UnityEvent onBecomeActive;
    [SerializeField] private UnityEvent onBecomeInactive;
    [SerializeField] private UnityEvent onBecomeHidden;
    [SerializeField] private float currentScale = 1;
    [SerializeField] private float currentAlpha = 1;
    public bool runningCR { get; private set; } = false;

    public int mask;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Move the layer to track to the camera
    }

    //Honestly these three functions could be cleaner, but they work so...
    internal void SetInactiveAndHide(int dir)
    {
        if (runningCR)
        {
            return;
        }
        if (dir < 0)
            StartCoroutine(Transition(gameObject.transform.parent.GetComponent<LayerManager>().transitionLength, currentScale * gameObject.transform.parent.GetComponent<LayerManager>().relativeLayerScale, 1f));
        if (dir > 0)
            StartCoroutine(Transition(gameObject.transform.parent.GetComponent<LayerManager>().transitionLength, currentScale / gameObject.transform.parent.GetComponent<LayerManager>().relativeLayerScale, 1f));
        onBecomeHidden.Invoke();
    }

    internal void SetActive(int dir)
    {
        if (runningCR)
        {
            return;
        }
        if (dir < 0)
            StartCoroutine(Transition(gameObject.transform.parent.GetComponent<LayerManager>().transitionLength, currentScale * gameObject.transform.parent.GetComponent<LayerManager>().relativeLayerScale, 1f));
        if (dir > 0)
            StartCoroutine(Transition(gameObject.transform.parent.GetComponent<LayerManager>().transitionLength, currentScale / gameObject.transform.parent.GetComponent<LayerManager>().relativeLayerScale, 1f));
        onBecomeActive.Invoke();
    }

    internal void SetInactiveAndBehind(int dir)
    {
        if (runningCR)
        {
            return;
        }
        if (dir < 0)
            StartCoroutine(Transition(gameObject.transform.parent.GetComponent<LayerManager>().transitionLength, currentScale * gameObject.transform.parent.GetComponent<LayerManager>().relativeLayerScale, 0f));
        if (dir > 0)
            StartCoroutine(Transition(gameObject.transform.parent.GetComponent<LayerManager>().transitionLength, currentScale / gameObject.transform.parent.GetComponent<LayerManager>().relativeLayerScale, 0f));
        onBecomeInactive.Invoke();
    }


    [Tooltip("enable or disable all Colliders in the layer")] //Depreciated when player layer was changed instead
    public void CollidersToggle(bool mode)
    {
        Collider2D[] all = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D c in all)
        {
            c.enabled = mode;
        }
    }

    [Tooltip("enable or disable all Effectors in the layer")] //Depreciated when player layer was changed instead
    public void EffectorsToggle(bool mode)
    {
        PlatformEffector2D[] all = GetComponentsInChildren<PlatformEffector2D>();
        foreach (PlatformEffector2D c in all)
        {
            c.enabled = mode;
        }
    }

    public IEnumerator Transition(float t, float s, float a)
    {
        EffectorsToggle(s==1);
        runningCR = true;
        float timePassed = 0f;
        float initScale = currentScale;
        float initAlpha = currentAlpha;
        SpriteRenderer[] rends = GetComponentsInChildren<SpriteRenderer>();
        while (timePassed < t)
        {
            currentScale = Mathf.Lerp(initScale, s, timePassed / t);
            currentAlpha = Mathf.Lerp(initAlpha, a, timePassed / t);
            gameObject.transform.localScale = Vector3.one * currentScale;
            foreach (SpriteRenderer r in rends)
            {
                r.color = new Color(r.color.r, r.color.g, r.color.b, currentAlpha);
            }
            timePassed += Time.deltaTime;
            yield return null;
        }
        currentScale = s;
        currentAlpha = a;
        gameObject.transform.localScale = Vector3.one * currentScale;
        foreach (SpriteRenderer r in rends)
        {
            r.color = new Color(r.color.r, r.color.g, r.color.b, currentAlpha);
        }
        runningCR = false;
        yield return null;
    }
}
