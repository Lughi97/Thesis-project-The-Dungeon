/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// only gives the visuall display
public class DispalyInventory : MonoBehaviour
{
    public MouseItem mouseItem= new MouseItem();

    public InventoryObject inventory;

    public GameObject InventoryPrefab;
    public int x_Start;
    public int y_Start;
    public  bool isDisplay = false;
    public int x_space_Between_item;
    public int number_of_Column;
    public int Y_Space_Between_Item;

    // return the slot that is linked to that game object (link to the data rappresentation of the slot 
    Dictionary< GameObject,InventorySlot> itemDisplay = new Dictionary<GameObject,InventorySlot>();// dictornary have 2 type
    // use for collection of value (the first one is the key and the second is the value store in that key
    // Start is called before the first frame update
    void Start()
    {
        CreateSlot();
    }

    // Update is called once per frame
    void Update()
    {
        // this  is a rappresentation of what's is happening in the inventory
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemDisplay)
        {
            if(_slot.Value.ID>=0)
            {
                // display item
                _slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1,1);
                _slot.Key.GetComponentInChildren<Text>().text = _slot.Value.amount == 1 ? "": _slot.Value.amount.ToString("n0");
            }
            else
            {
                // clear slo to display no items
                _slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,0);
                _slot.Key.GetComponentInChildren<Text>().text ="";

            }
        }
    }
    //create the image that are going to be placed in the inventory
    public void CreateSlot()
    {
        itemDisplay = new Dictionary<GameObject, InventorySlot>();// use for not have errors ;i++
        for (int i = 0; i < inventory.Container.Items.Length ; i++)
        {
            var obj = Instantiate(InventoryPrefab, Vector2.zero, Quaternion.identity, transform); // the transform in wich the parent of the ob is attached to 
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            //delegate is used when we call a function in a call function and this function has a paramater in it
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemDisplay.Add(obj, inventory.Container.Items[i]);
        }
    }


    // use this function to render simple the code any time is created a new a event ( liked to the inventory slot)
    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();//get the event trugger
        // create new event trigger
        var eventTrigger = new EventTrigger.Entry();// this is a entry to the trigger
        eventTrigger.eventID = type;// add a type of an event
        eventTrigger.callback.AddListener(action);//
        trigger.triggers.Add(eventTrigger);//add the event trigger that is created
    }
  
    // the function event needed ot swap the slots
    // to clic drag and drop an obj create a copy of what is dragging and moving around 
    // is only vsuaal rappresentation
    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObject = obj;//set the mouse hover obj to the slot to know wich objec our ouse is over 
        if (itemDisplay.ContainsKey(obj))// check if the obj is a obj form the dictionary itedisplay
        {
            mouseItem.hoverSlot = itemDisplay[obj];//set the mouseOverITem to that obj(the slot)

        }

    }
    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObject = null;// wich slot we are over with the mouse 
       
        mouseItem.hoverSlot = null;// make sure that is an item of the display obj dictionary

        
    }
    public void OnDragStart(GameObject obj)
    {
        //have a reference to this mouseObject for dragging
        var mouseObject = new GameObject();// instantiate an empty object to the scene
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if(itemDisplay[obj].ID>=0)
        {
            var image = mouseObject.AddComponent<Image>();
            image.sprite= inventory.database.GetItem[itemDisplay[obj].ID].uiDisplay;
            
            image.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemDisplay[obj];
    }
    public void OnDragEnd(GameObject obj)
    {
        if(mouseItem.hoverObject)
        {
            inventory.MoveItem(itemDisplay[obj], itemDisplay[mouseItem.hoverObject]);
        }
        else
        {
            //delete the item
            inventory.RemoveItem(itemDisplay[obj].item);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }
    public void OnDrag(GameObject obj)
    {
        if(mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }


    // place the image in the inventory 
    public Vector2 GetPosition(int i)
    {
        return new Vector2(x_Start+( x_space_Between_item * (i % number_of_Column)), y_Start+(-Y_Space_Between_Item * (i / number_of_Column)));
    }
  
    
}


*/