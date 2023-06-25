using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public Text manaPoints;
    // set the max healt of the player 
    public void setMaxMana(float maxMana)
    {
        slider.maxValue = maxMana;
        // healthPoints.text = "Hp: " + maxHealt;

        gradient.Evaluate(1f);
    }
    // set the current health form the player
    public void SetMana(float mana, float maxMana)
    {
        slider.value = mana;
        manaPoints.text = "" + (int)maxMana + "/" + (int)mana;

        fill.color = gradient.Evaluate(slider.normalizedValue);// normalize value form 0 to 1
    }
}
