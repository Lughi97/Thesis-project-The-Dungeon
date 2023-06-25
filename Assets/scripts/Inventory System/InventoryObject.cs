using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

public enum InterfaceType// for the different interface
{
    Inventory,
    Equipment
    //chest

}

// the incentory is competely separated from the display inventory class
[CreateAssetMenu(fileName = "New Inventory ", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public InterfaceType type;
    public Inventory Container;
    //wtake directly form the inventory object the slots
    public InventorySlot[] GetSlots { get { return Container.Slots; } }// a property that returns the slots of the inventory


    public bool AddItem(Item _item, int _amount)// add the item in to the invetory
    {
        // if there is an available slot 
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemInventory(_item); 
        if(!database.ItemObject[_item.Id].isStacable|| slot==null)// if the item is not stacable or the slot is empty
        {
            SetEmptySlot(_item, _amount);// set a empty slot 
            return true;

        }
        slot.AddAmount(_amount);
        return true;

    }
    public int EmptySlotCount// count hom many empty slots is having the inventory ( a property)
    {
        get
        {
            int counter = 0;
            for (int i = 0; i <GetSlots.Length; i++)
            {
                if(Container.Slots[i].item.Id<=-1)
                    counter++;
            }
            return counter;
        }
    }
    public InventorySlot FindItemInventory(Item _item)//loocking for the item of  the inventory if the item exist or not
    {
        for (int i = 0; i <GetSlots.Length; i++)
        {
            if(Container.Slots[i].item.Id == _item.Id)// the item in the invenotry is equal to the same item that is passing to the function
            {
                return GetSlots[i];// reuturn the item in the container 
            }
        }
        return null;// there is no a item on the invenotory
    }
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i <GetSlots.Length; i++)
        {
            if(GetSlots[i].item.Id <= -1)
            {
               GetSlots[i].UpdateSlot(_item,_amount);
                return GetSlots[i];
            }
        }

        // Set up functionality when invenotry is full
        return null;
    }
    public void SwapItem(InventorySlot item1, InventorySlot item2)// swapped 2 items in the inventory
    {
        if(item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject) && !(item1.item.Id <= -1) && (item2.item.Id <= -1 || item2.item.Id >= 0))
        {
            
                InventorySlot temp = new InventorySlot(item2.item, item2.amount);
                item2.UpdateSlot(item1.item, item1.amount);
                item1.UpdateSlot(temp.item, temp.amount);
            
        }

    }
    public void RemoveItem(Item _item)
    {
        for (int i = 0; i <GetSlots.Length; i++)
        {
            if(GetSlots[i].item == _item)
            {
               GetSlots[i].UpdateSlot(null,0);
            }
        }
    }
    [ContextMenu("Save")]
    public void Save()
    {
        //Using Jason Utility( to serialiaze a scriptable object) and usign binary formatter and the file stream to create a file and write that string into that file and save it to a given location
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        // create the file
        
        FileStream file = File.Create(string.Concat(Application.persistentDataPath,savePath));//// concat use less memory thane concatanating sting with the +
        bf.Serialize(file, saveData);
        file.Close();

        //persistentDataPath: a function that unity priovides to ypu to be able to save files out to a persistent path across mu;tiple divices


        // IFormatter formatter = new BinaryFormatter();
        // Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        //formatter.Serialize(stream, Container);
        // stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            // to open the file 
             FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath),FileMode.Open);
            // now we need to convert the file into a scriptable object
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);// this is the obj to overwrite
            file.Close();// to not have memory leaks

           // IFormatter formatter = new BinaryFormatter();
          //  Stream stream= new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
          //  Inventory newContainer = (Inventory)formatter.Deserialize(stream);
          //  for (int i = 0; i < Container.Items.Length; i++)
           // {
          //      Container.Items[i].UpdateSlot(newContainer.Items[i].item, newContainer.Items[i].amount);
          //  }
          // stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
  
}

//delegate is a reference to a function 
public delegate void SlotUpdated(InventorySlot _slot);// _slot use to reference the slotDisplay variable 

[System.Serializable]
public class InventorySlot
{
    public itemType[] AllowedItems = new itemType[0];//wich one is allowed in the player equipment 
    [System.NonSerialized]//prevent the save system to save this variable (parent)(becasue it dosen't have an inventory obj
    public UserInterface parent;// to link to the parent the inventory belong to 
    [System.NonSerialized]// dose't show in the inspector
    public GameObject slotDisplay;// the UI of the slot 

    //fire this coe before ans after the slot is updated
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;// this is for adding item to the slot 
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;// this is for removing items to the slot

    public Item item;// the item in store in inventory slot
    public int amount;// the amount of the same object hold

    // to return the item object for teh swap function 
    //this ais a property a: act like variables 
    public ItemObject ItemObject
    {
        // property acsessors
   
        get // return the field the a is encapsulated  (read acessors )
        {
            if(item.Id >= 0)
            {
                return parent.inventory.database.ItemObject[item.Id]; //return the itemobjec form the database
            }
            return null;
        }
        //set : is allocted to teh field using the keyword value  
    }
    
    public InventorySlot()// the constructor for setting and value when the slot is created
    {
        UpdateSlot(new Item(), 0);
     
    }

    public InventorySlot(Item _item, int _amount)//constructor for setting the inventory slot with a certain amount 
    {
        UpdateSlot(_item, _amount);
     
    }

    public void UpdateSlot(Item _item, int _amount)// 
    {
        if (OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);//invoke this current class (invenotrySlot)
       // invoke fires all the methods connected to this delegate
        item = _item;
        amount = _amount;

        if (OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }

    public void  removeItem()
    {
        Debug.Log("Removed: "+ item.Name);
        item = new Item();
        amount = 0;
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
        
    }// if are the same item, add in the same slot

    public bool CanPlaceInSlot(ItemObject _itemObject)// check if the item can be place in the slot return a bool
    {
        if (AllowedItems.Length <= 0 || _itemObject==null || _itemObject.data.Id<=0) // to chek if the item is in the slot if ther is'nt any item can be placed
            return true;
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
                return true;
        }
        return false;// is not in the allowed item;
    }

}


[System.Serializable]

public class Inventory // to create an inventory with defined number of slots
{
   
   public InventorySlot[] Slots= new InventorySlot[30];

    public void Clear()// clear, all the slots
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].removeItem();
        }
    }
}

