
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Here all the stast of the player to update if the playe levels up and give to the game manager and the player
/// Here we the exp. gain in the game is gonna power up the player further.
/// </summary>
/// 

 
public class PlayerStats : MonoBehaviour
{
  
   
    public float playerExp = 0;
    public float totalplayerExp = 0;
    public float expToNext = 100;
    private int valueUpdate;
    public int levelPlayer = 1;
    public static PlayerStats instance = null;
    public bool levelUp=false;
    public bool isDualWilding = false;
    //public float vualueToUpdate;

    // the special attack stats
    //   public float circleSlash;
    // public float usageManaCicleSlash;
    public float powerExplosion =10;
    public float UsageManaExplosion = 1;
    public float coolDown = 1;
    

    //player equipment
    public InventoryObject equipment;
    public Attribute[] attributes;// the array of the attrubutes of the equipent 


   
    //public float[] stats;
    public float defence;
    public float power;
    public float mana ;
    public float hp;
    public float stamina;
    public float attack;
    public float attackRate;
    public float strainStamina;
    public string[] names;
       

    public bool[] equiped;// bool 1 -> health, bool 2-> stamina bool3-> power bool4 ->defence bool 5-> mana

   
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
      //  Add();

    }
    // Start is called before the first frame update
    void Start()
    {
        //modifier

        //addthemodifiers
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);// set parent for all the  attraibutes 
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;// add methid when unequipped
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;// add the method when equipped

        }
        strainStamina = 1;
        attackRate = 0.3f;
        hp = stamina = power = defence = mana = attack = attributes[0].value.BaseValue;
        //float value = GetData(healtpoint);
        // Debug.Log(value);
     

    }

    // adding value when uneqiupping and equipping
    //unequip remove value 
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;// when there is no item on it 
       
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);

                            switch (j)
                            {
                                case 0:
                                    hp = attributes[j].value.ModifiedValue;

                                    equiped[j] = false;
                            
                                break;
                                case 1:
                                    stamina = attributes[j].value.ModifiedValue;
                                    equiped[j] = false;
                                    break;
                                case 2:
                                    power = attributes[j].value.ModifiedValue;

                                    equiped[j] = false;
                                    break;
                                case 3:
                                    defence = attributes[j].value.ModifiedValue;
                                    equiped[j] = false;
                                    break;
                                case 4:
                                    mana = attributes[j].value.ModifiedValue;
                                    equiped[j] = false;
                                    break;
                                case 5:
                                    attack = attributes[j].value.ModifiedValue;
                                    equiped[j] = false;
                                    break;
                                case 6:
                                    attackRate = attributes[j].value.ModifiedValue - attributes[i].value.BaseValue; 

                                    equiped[j] = false;
                                    break;
                                case 7:

                                    strainStamina = attributes[j].value.ModifiedValue - attributes[i].value.BaseValue;
                                    equiped[j] = false;
                                    break;
                                default:
                                    break;
                            }
                   



                        }



                    }
                }

                break;

            default:
                break;
        }
    }
    //equip add the value 
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));
               
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                            switch (j)
                            {
                                case 0:
                                    hp = attributes[j].value.ModifiedValue;

                                    equiped[j] = true;

                                    break;
                                case 1:
                                    stamina = attributes[j].value.ModifiedValue;
                                    equiped[j] = true;
                                    break;
                                case 2:
                                    power = attributes[j].value.ModifiedValue;

                                    equiped[j] = false;
                                    break;
                                case 3:
                                    defence = attributes[j].value.ModifiedValue;
                                    equiped[j] = true;
                                    break;
                                case 4:
                                    mana = attributes[j].value.ModifiedValue;
                                    equiped[j] = true;
                                    break;
                                case 5:
                                    attack = attributes[j].value.ModifiedValue;
                                    equiped[j] = true;
                                    break;
                                case 6:
                                    attackRate = attributes[j].value.ModifiedValue - attributes[i].value.BaseValue+0.3f;
                                    equiped[j] = true;
                                    break;
                                case 7:
                                    strainStamina = attributes[j].value.ModifiedValue - attributes[i].value.BaseValue+1;
                                    equiped[j] = true;
                                    break;
                                default:
                                    break;
                            }
                       
                            //   Debug.LogError("Equipped " + j+ ": " + equiped[j]);

                        }
                    }
                }
                
                break;

            default:
                break;
        }
    }
    // The exp 
    private void Update()
    {
       
    }
    // update the level and create a level up (to update)
    public void UpdateExp(float exp)
    {
        playerExp = playerExp + exp;
        totalplayerExp = playerExp + exp;

        // Debug.Log(playerExp);
        if (playerExp >= expToNext)
        {
            levelPlayer++;
            levelUp = true;
            // float difference = playerExp-expToNext;
            playerExp = 0;
            expToNext +=(4*Mathf.Log(4*Mathf.Pow(levelPlayer,5)+5* Mathf.Pow(levelPlayer,4)+6*levelPlayer,2));
            UpdateStats();
          
            Debug.Log("Level Up!!");
        }
      
       
     
    }


  
    private void UpdateStats()
    {
       
       // valueUpdate = 10;
      
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].value.UpdateBaseValue();
            float update= (attributes[i].value.BaseValue - attributes[i].value.temValue);
           // Debug.LogError("Equipped " + i + ": " + equiped[i]);
            switch (i)
            {
                case 0:
                    if (equiped[i] == true)
                        hp += update;
                    else
                        hp = attributes[i].value.BaseValue;
                
                    break;
                case 1:
                    if (equiped[i] == true)
                        stamina += update ;
                    else
                        stamina = attributes[1].value.BaseValue;
                    break;
                case 2:
                    if (equiped[i] == true)
                        power +=update;
                    else
                        power = attributes[2].value.BaseValue;
                    break;
                case 3:

                    if (equiped[i] == true)
                        defence += update;
                    else
                        defence += attributes[i].value.BaseValue ;
                    break;
                case 4:

                    if (equiped[i] == true)
                        mana += update;
                    else
                        mana = attributes[i].value.BaseValue;
                    break;
                case 5:
                    if (equiped[i] == true)
                        attack += update;
                    else
                        attack = attributes[i].value.BaseValue; 
                    break;
                case 6:
                  // no update the attak rate
                    break;
                case 7:
                    //no update strain stamina

                default:
                    break;
            }
           
        }
        
        // power += power / 4;// * Mathf.Exp(Mathf.Log(levelPlayer, 2)) / 2;
        // mana += mana / 2;// *Mathf.Exp(Mathf.Log(levelPlayer, 2)) / 2;
     //   circleSlash += circleSlash/4 *Mathf.Exp(Mathf.Log(levelPlayer, 2)) / 2;
        //usageManaCicleSlash += usageManaCicleSlash / 4 * Mathf.Exp(Mathf.Log(levelPlayer, 2)) / 2;
    }

    
    public void AttributeModified(Attribute attribute)// to see what value  is updated.
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        
        equipment.Clear();
    }
    
   

   
}
[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public PlayerStats parent;
    public AttributesObj type;// the attributes of the player
    public ModifiableFloat value;// get the value form Modifiable int 
    // the paret  "know" when the attribute gets updated.
    public void SetParent(PlayerStats _parent)// the parent is always the playerStats
    {
        parent = _parent;
        value = new ModifiableFloat(AttributeModified);//call the funti
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}





