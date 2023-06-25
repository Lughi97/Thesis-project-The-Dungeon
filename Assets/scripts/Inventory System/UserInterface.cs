using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// only gives the visuall display
public abstract class UserInterface : MonoBehaviour // use to extend other classes
{
    //public MouseItem mouseItem = new MouseItem();

    public InventoryObject inventory;

   
    // return the slot that is linked to that game object (link to the data rappresentation of the slot 
    public Dictionary<GameObject, InventorySlot> slotOnInterface = new Dictionary<GameObject, InventorySlot>();// dictornary have 2 type //dispaly the solt that item goes in
    // use for collection of value (the first one is the key and the second is the value store in that key
    // Start is called before the first frame update
    void Start()
    {
       
        for (int i = 0; i < inventory.GetSlots.Length; i++)// loop trhow all the itmes inside the interface
        {
            // when unity start and a new interface is created
            inventory.GetSlots[i].parent = this;//link the items to this interface o then to figure out wich interface the slot blongs to
        }
        CreateSlot();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });

    }

    // Update is called once per frame
    void Update()
    {
        // this  is a rappresentation of what's is happening in the inventory
        slotOnInterface.UpdateSlotDisplay();
    }

    
    
    //create the image that are going to be placed in the inventory
    public abstract void CreateSlot();

    // use this function to render simple the code any time is created a new a event ( liked to the inventory slot)
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
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
    // is only visual rappresentation
    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoverdOver = obj;//set the mouse hover obj to the slot to know wich objec our ouse is over 
      

    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoverdOver = null;


    }
    // to check if te over object attaced to the mouse in over the interface of the invenotry or the equipment
    public void OnEnterInterface(GameObject obj)
    {
     MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();


    }
    public void OnExitInterface(GameObject obj)
    {

        MouseData.interfaceMouseIsOver = null;

    }
    public void OnDragStart(GameObject obj)
    {
        
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
        
    }
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotOnInterface[obj].item.Id >= 0)// if the obj exist in the inventory
        {
            //have a reference to this mouseObject for dragging
            tempItem = new GameObject(); ;// instantiate an empty object to the scene
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 100);
            tempItem.transform.SetParent(transform.parent);
            var image = tempItem.AddComponent<Image>();
            image.sprite = slotOnInterface[obj].ItemObject.uiDisplay;
            image.raycastTarget = false;
        }
        return tempItem;
    }
    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);// destiy the item on the mouse
        //can open the interface if you want to dropp the item
        if(MouseData.interfaceMouseIsOver==null)
        {
            slotOnInterface[obj].removeItem();
            return;// stop this to not swapped the item
        }
        if(MouseData.slotHoverdOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotOnInterface[MouseData.slotHoverdOver];
            inventory.SwapItem(slotOnInterface[obj],mouseHoverSlotData);// swapped the object dragged with the empty slot or with an item that the mouse is hover over
        }
        
    }
    public void OnDrag(GameObject obj)
    {
       
        //continu totorial equipping system minutes 24:00;
        if (MouseData.tempItemBeingDragged != null)
        {
           MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }




}
// to call to the other class without istatiating 
//to hold the data of the mouse
public static class MouseData // the static class cannot be instatiated
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;// the item being dragged
    public static GameObject slotHoverdOver;
}

// enable to 'add' methods to existing type without creating new derived type
//learn what are extension method!!
public static class ExtensionMethods
{
    //all extension method need to be static
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotOnInterface)
        {
            if (_slot.Value.item.Id >= 0)
            {
                // display item
                _slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<Text>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                // clear slo to display no items
                _slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<Text>().text = "";

            }
        }
    }
}