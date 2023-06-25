using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script add the value of the wepon to the playerStats( this script is a non memory usage  way of putting and removing values
/// </summary>
public delegate void ModifiedEvent();// this is a function callback when the value is modified
[Serializable]
public class ModifiableFloat
{
    
    [SerializeField]
   
    private float baseValue=20;
    public float UpdateValue;
    public float BaseValue { get { return baseValue; } set { baseValue = value; UpdateModifiedValue(); } }// retrun base value  and when it gt set it runs Update value function
   // PlayerDataStats playerStats;
    [SerializeField]
    private float modifiedValue;
    public float ModifiedValue { get { return modifiedValue; } private set {modifiedValue= value;}}// works the same as the base value;
    public float temValue;

    
    public event ModifiedEvent ValueModified;// the delegate 
    public void UpdateBaseValue()
    {
        temValue = baseValue;
        baseValue += 10;
        //Debug.LogError("BAseValue "+ baseValue);
        modifiedValue = baseValue;
       
        
       
          
    }
    public ModifiableFloat(ModifiedEvent method = null)//create a constructor for the modifeid value 
    {
        ModifiedValue = BaseValue;
        if (method != null)
            ValueModified += method;
    }

    public List<FModifiers> modifiers = new List<FModifiers>();// this is the list of modifiers 
    public void RegisterModEvent(ModifiedEvent method)// this register the the event 
    {
        ValueModified += method;
    }
    public void UnregisterModEvent(ModifiedEvent method)// unregister event 
    {
        ValueModified -= method;
    }
    public void UpdateModifiedValue()//loop to all our modifier values 
    {
        float valueToAdd = 0;// ths value will either add or subtract to the cuurent value
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].AddValue(ref valueToAdd);// add the value ref the itemBuffValue that is generated 
        }
        ModifiedValue = baseValue + valueToAdd;// add the value to add to the base value 
        if (ValueModified != null)
        {
            ValueModified.Invoke();// when the stack is updated 
        }
    }

    public void AddModifier(FModifiers _modifier)// add the volue modifiers  
    {
        modifiers.Add(_modifier);
        UpdateModifiedValue();
    }
    public void RemoveModifier(FModifiers modifier)
    {
        modifiers.Remove(modifier);//remove the modifers 
        UpdateModifiedValue();// update again; reset the base value and the modified value 
    }
    
    
    
}