using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum itemType // the diffrent type of item
{
    Potions,
    Helmet,
    Wepon,
    Shield,
    Boots,
    Chest,
    Default,
}
public enum AttributesObj
{
    
    //Intelect,// add mana to the player
    Health,
    Defence,
    Strenght,// the damage that the item holds
    Stamina,
    Mana,
    Attack,
    AttackRate,
    StaminaDrain
    // AttackRate,// how fast is the attack
    // ManaRevover,
    // HealtRecover,
    //AttackBoost,
    // DefenceBoost,

}
[CreateAssetMenu(fileName ="New Item", menuName ="Inventory System/Items/item")]
public class ItemObject : ScriptableObject// this is not the class to create the object 
                                                   // its a tamplate for the other object 
{
  // the item obj
    public Sprite uiDisplay;
   
    public bool isStacable;
    public itemType type;
    [TextArea(15, 20)]// to make esly visible in unty 
    public string description;

    public Item data = new Item();// the data of the new item
   


    public Item CreateItem()// to create an item usign a function on the object
    {
        Item newItem = new Item(this);
        return newItem;
    }


}
[System.Serializable]// to be visible within the editior

public class Item// is the item of the inventory
{
    public ItemBuff[] buffs;// copy the buffs form the itemobj to the item class
    public string Name;
    public int Id=-1;
    
    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(ItemObject item)// the consturctor: is a function type that is called when the object is created ( the class)
    {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];// copy the itemobj into the item class ( use new to modyfy the buffs of the item class)
        for(int i=0; i<buffs.Length; i++)
        {
           
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max);
            buffs[i].attribute = item.data.buffs[i].attribute;
        }
    }
}
[System.Serializable]
public class ItemBuff:FModifiers// extend the class
{
    public AttributesObj attribute;
    public float value;// the current value of the item
    public float min;
    public float max;


    public ItemBuff(float _min, float _max)// the constructor of the itemBuff
    {
        min = _min;
        max = _max;

        GenerateValue();


    }

    public void GenerateValue()
    {
        value =UnityEngine.Random.Range(min, max);
    }
    
  
  
    public void AddValue(ref float baseValue)
    {
        baseValue += value;
    }
}