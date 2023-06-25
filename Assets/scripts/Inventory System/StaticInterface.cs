using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// In the editor is set-up the slots
/// </summary>
public class StaticInterface : UserInterface// extensions of the USerinterface class
{
    public GameObject[] slots;
   
    public override void CreateSlot()
    {
        slotOnInterface = new Dictionary<GameObject, InventorySlot>();// create a new dictiornary
        for (int i = 0; i < inventory.GetSlots.Length; i++)//loop thrw the ewquipment item database
        {
            var obj = slots[i];// obj that links the arry links to the same slots inside of the dabse
            //delegate is used when we call a function in a call function and this function has a paramater in it
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            slotOnInterface.Add(obj, inventory.GetSlots[i]);//add to the item display
        }
    }
    public new void OnDragEnd(GameObject obj)// this is to hide the OnDragEnd funtion per evitare di elimare gli oggetti dalla 
    {
        Destroy(MouseData.tempItemBeingDragged);// destiy the item on the mouse
        //can open the interface if you want to dropp the item
        if (MouseData.interfaceMouseIsOver == null)
        {
            //slotOnInterface[obj].removeItem();
            return;// stop this to not swapped the item
        }
        if (MouseData.slotHoverdOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotOnInterface[MouseData.slotHoverdOver];
            inventory.SwapItem(slotOnInterface[obj], mouseHoverSlotData);// swapped the object dragged with the empty slot or with an item that the mouse is hover over
        }

    }
}
