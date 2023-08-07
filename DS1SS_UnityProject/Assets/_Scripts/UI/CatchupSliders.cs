using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CatchupSliders : MonoBehaviour
{
    private CanvasManager CM;
    private Slider healthSlider;
    private Slider staminaSlider;
    private Slider healthCatchupSlider;
    private Slider staminaCatchupSlider;
    [SerializeField] private float catchupSliderDelay = 1;
    [SerializeField] private float catchupSliderSpeed = 0.02f;
    public bool staminaCatchupActive;
    public bool healthCatchupActive;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Build")
        {
            CM = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
            healthSlider = CM.PlayerHealthSlider;
            staminaSlider = CM.PlayerStaminaSlider;
            healthCatchupSlider = CM.PlayerHealthCatchupSlider;
            staminaCatchupSlider = CM.PlayerStaminaCatchupSlider;

            healthCatchupSlider.value = healthSlider.value;
            staminaCatchupSlider.value = staminaSlider.value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log("Health catchup slider value = " + healthCatchupSlider.value);
        Debug.Log("Heath slider value = " + healthSlider.value);*/
    }

    public IEnumerator ManualUpdate(bool isHealthUpdate)
    {
        if (isHealthUpdate)
        {
            healthCatchupActive = true;
            //healthCatchupSlider.value = healthSlider.value;
            yield return new WaitForSeconds(catchupSliderDelay);
            while (healthCatchupSlider.value > healthSlider.value)
            {
                healthCatchupSlider.value--;
                yield return new WaitForSeconds(catchupSliderSpeed);
                if (healthCatchupSlider.value == healthSlider.value)
                {
                    break;
                }
            }
            healthCatchupActive = false;
        }
        else
        {
            staminaCatchupActive = true;
            //staminaCatchupSlider.value = staminaSlider.value;
            yield return new WaitForSeconds(catchupSliderDelay);
            while (staminaCatchupSlider.value > staminaSlider.value)
            {
                staminaCatchupSlider.value--;
                yield return new WaitForSeconds(catchupSliderSpeed);
                if (staminaCatchupSlider.value == staminaSlider.value)
                {
                    break;
                }
            }
            staminaCatchupActive = false;
        }
    }
}
