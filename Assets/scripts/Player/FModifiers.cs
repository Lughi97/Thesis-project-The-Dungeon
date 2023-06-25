using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FModifiers // An interface contains definitions for a group of related functionalities that a class or a struct can implement.
{
    void AddValue(ref float value);//reference another value and add the modifier vlue to the current value 
}

