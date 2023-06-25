using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The interface can change in the code
/// </summary>
public class DynamicInterface : UserInterface
{
    public GameObject InventoryPrefab;
    public int x_Start;
    public int y_Start;
    public bool isDisplay = false;
    public int x_space_Between_item;
    public int number_of_Column;
    public int Y_Space_Between_Item;

    //create the image that are going to be placed in the inventory
    public override void CreateSlot()
    {
        slotOnInterface = new Dictionary<GameObject, InventorySlot>();// use for not have errors ;i++
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(InventoryPrefab, Vector2.zero, Quaternion.identity, transform); // the transform in wich the parent of the ob is attached to 
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            //delegate is used when we call a function in a call function and this function has a paramater in it
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            slotOnInterface.Add(obj, inventory.GetSlots[i]);
        }
    }

    // place the image in the inventory 
    private Vector2 GetPosition(int i)
    {
        return new Vector2(x_Start + (x_space_Between_item * (i % number_of_Column)), y_Start + (-Y_Space_Between_Item * (i / number_of_Column)));
    }
}
