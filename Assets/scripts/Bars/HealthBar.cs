using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Here is define the healt bar and update in real time.
/// </summary>
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public Text healthPoints;
    // set the max healt of the player 
    public void setMaxHealt(float maxHealt)
    {
        slider.maxValue = maxHealt;
       // healthPoints.text = "Hp: " + maxHealt;

        gradient.Evaluate(1f);
    }
    // set the current health form the player
    public void SetHealt(float healt, float maxHealt)
    {
        slider.value = healt;
        healthPoints.text = "Hp: " + (int)maxHealt+"/"+(int)healt;

        fill.color = gradient.Evaluate(slider.normalizedValue);// normalize value form 0 to 1
    }
}

