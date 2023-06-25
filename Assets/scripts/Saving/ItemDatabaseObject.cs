using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName ="Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject,ISerializationCallbackReceiver// use this becuase unity dosen't serialize the dictionary
{
    public ItemObject[] ItemObject;// array of all the item of the inventory
    // import an itme and easly get id of teh item
  
 
  

    // we convert the object into string to store the data into a database

    // function that are called before and after unity serialized the data
    public void OnAfterDeserialize() // going to use only after/  populate  the dictionary
    {
        
        for(int i=0; i< ItemObject.Length; i++)
        {
            // after disirilize we find the id of the itemobject
            ItemObject[i].data.Id = i;//set the id of the item we  added to the database
       
        }
    }

    public void OnBeforeSerialize()
    {
       
     
    }
}
