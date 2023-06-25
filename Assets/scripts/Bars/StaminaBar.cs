using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaminaBar : MonoBehaviour
{
     
    public Slider slider;
    public Gradient gradient;

    public void setMaxStamina(float maxStamina)
    {

        slider.maxValue = maxStamina;
        gradient.Evaluate(1f);
    }
    public void setStamina(float stamina,float MaxStamina)
    {
        slider.value = stamina;
        gradient.Evaluate(slider.normalizedValue);
    }
}
