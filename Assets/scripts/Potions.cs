using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PotionType {Health, Mana , Stamina, Attack, Defence, RandomEff};
public enum PotionCapacity { small, medium, big, giaiant};
public class Potions : MonoBehaviour
{
    public PotionType potion;
    public PotionCapacity size;
    private GameManager _gameMan;
   
    //for attack
   private float incriese;
    private float decrise;
  //  public bool _isAdecrement = false;
    private float cooldown ;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _gameMan = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.name=="Player")
        {
            SelectType();
            this.gameObject.SetActive(false);
        }
    }
   
    void SelectType()
    {
        switch (potion)
        {
            case PotionType.Health:
                Debug.Log("Health");
                ReplenishHealth();
                break;
            case PotionType.Mana:
                Debug.Log("Mana");
                ReplenishMana();
                break;
            case PotionType.Attack:
                Debug.Log("Attack");
                boosterAttack();
                break;
            case PotionType.Defence:
                Debug.Log("Defence");
                boosterDefence();
                break;

            case PotionType.Stamina:
                Debug.Log("Stamina");
                refillStamina();
                break;
            case PotionType.RandomEff:
                Debug.Log("Random Effect");
                Buff_Wekness();
                break;
        }
    }

    // if the player has pickup the potionj already add to the inventory
    void ReplenishHealth()
    {
        int baseAmount;
        switch (size)
        {
            case PotionCapacity.small:
                Debug.Log("Small");
                baseAmount = 5 + _gameMan.baseValuePotion;
                //Debug.LogError(_gameMan.baseValuePotion);
                _player.getHealtPotion(baseAmount);
                break;
            case PotionCapacity.medium:
                Debug.Log("Medium");
                 baseAmount =10 + _gameMan.baseValuePotion;
               // Debug.LogError(_gameMan.baseValuePotion);
                _player.getHealtPotion(baseAmount);
                break;
            case PotionCapacity.big:
                Debug.Log("Big");
                baseAmount = 15 + _gameMan.baseValuePotion;
                //Debug.LogError(_gameMan.baseValuePotion);
                _player.getHealtPotion(baseAmount);
                break;
            case PotionCapacity.giaiant:
                baseAmount = 20 + _gameMan.baseValuePotion;
                _player.getHealtPotion(baseAmount);
                break;

        };
      
    }
    void boosterAttack()
    {
        
        switch (size)
        {
            case PotionCapacity.small:
                Debug.Log("Small");
                incriese = 1.1f;
                cooldown = 20f;
                if (_player.attackBoostActive == false)
                {
                    _player.attackBoost(incriese, cooldown);
                }
                break;
            case PotionCapacity.medium:
                Debug.Log("Medium");
                 incriese = 1.3f;
                 cooldown = 15f;
                if (_player.attackBoostActive == false)
                {
                    _player.attackBoost(incriese, cooldown);
                }
             
                break;
            case PotionCapacity.big:
                Debug.Log("Big");
                 incriese = 1.5f;
                 cooldown = 12f;
                if (_player.attackBoostActive == false)
                {
                    _player.attackBoost(incriese, cooldown);
                }
                break;
            case PotionCapacity.giaiant:
                Debug.Log("Giaiant");
                 incriese = 2f;
                 cooldown = 10f;
                if (_player.attackBoostActive == false)
                {
                    _player.attackBoost(incriese, cooldown);
                }
                break;

        }
    }
    void boosterDefence()
    {
        switch (size)
        {
            case PotionCapacity.small:
                Debug.Log("Small");
                incriese = 1.1f;
                cooldown = 20f;
                if (_player.defenceBoostActive == false)
                {
                    _player.defenceBoost(incriese, cooldown);
                }
                break;
            case PotionCapacity.medium:
                Debug.Log("Medium");
                incriese = 1.3f;
                cooldown = 15f;
                if (_player.defenceBoostActive == false)
                {
                    _player.defenceBoost(incriese, cooldown);
                }

                break;
            case PotionCapacity.big:
                Debug.Log("Big");
                incriese = 1.5f;
                cooldown = 12f;
                if (_player.defenceBoostActive == false)
                {
                    _player.defenceBoost(incriese, cooldown);
                }
                break;
            case PotionCapacity.giaiant:
                Debug.Log("Giaiant");
                incriese = 2f;
                cooldown = 10f;
                if (_player.defenceBoostActive == false)
                {
                    _player.defenceBoost(incriese, cooldown);
                }
                break;

        }
    }
    void refillStamina ()
    {
        int baseAmount;
        switch (size)
        {
            case PotionCapacity.small:
                Debug.Log("Small");
                baseAmount = 5 + _gameMan.baseValuePotion;
               // Debug.LogError(_gameMan.baseValuePotion);
                _player.getStaminaPotion(baseAmount);
                break;

            case PotionCapacity.big:
                Debug.Log("Big");
                baseAmount = 15 + _gameMan.baseValuePotion;
                //Debug.LogError(_gameMan.baseValuePotion);
                _player.getStaminaPotion(baseAmount);
                break;


        }
    }
    void Buff_Wekness()
    {
        int baseValue;
        int chance = Random.Range(1, 10);
        switch(chance)
        {
            case 1:
                //healt()
                Debug.Log("Incriese HP");
                baseValue = 25 + _gameMan.baseValuePotion;
                _player.getHealtPotion( baseValue);
                break;
            case 2:
                //Mana()
                baseValue = 25 + _gameMan.baseValuePotion;
                Debug.Log("Incriese MP");
                break;
            case 3:
                //attBoost()
                Debug.Log("Incriese ATTK");
                incriese = 1.3f;
                cooldown = 15f;
                if (_player.attackBoostActive == false)
                {
                    _player.attackBoost(incriese, cooldown);
                }
                break;
            case 4:
                //defecneBooost
                Debug.Log("Incriese DEFENCE");
                incriese = 1.3f;
                cooldown = 15f;
                if (_player.defenceBoostActive == false)
                {
                    _player.defenceBoost(incriese, cooldown);
                }
                break;
            case 5:
                //StaminaBoost
                Debug.Log("Incriese STAM");
                break;
            case 6:
                //lose healt
                Debug.Log("Decrise HP");
                baseValue = -25- _gameMan.baseValuePotion;
                _player.getHealtPotion(baseValue);
                break;
            case 7:
                Debug.Log("Decrise MP");
                //lose mana
                break;
            case 8:
                //attloose()
                Debug.Log("Decrise ATTK");
                decrise = 0.5f;
               
                cooldown = 15f;
                if (_player.attackBoostActive == false)
                {
                    _player.attackBoost(decrise, cooldown);
                }
                break;
            case 9:
                //loosedefemce
                Debug.Log("Decrise DEFENCE");
                decrise = 0.5f;
             
                cooldown = 15f;
                if (_player.defenceBoostActive == false)
                {
                    _player.defenceBoost(decrise, cooldown);
                }
                break;


        }
    }
    void ReplenishMana()
    {
        int baseAmount;
        switch (size)
        {
            case PotionCapacity.small:
                Debug.Log("Small");
                baseAmount = 5 + _gameMan.baseValuePotion;
                //Debug.LogError(_gameMan.baseValuePotion);
                _player.getManaPotion(baseAmount);
                break;
            case PotionCapacity.medium:
                Debug.Log("Medium");
                baseAmount = 10 + _gameMan.baseValuePotion;
                // Debug.LogError(_gameMan.baseValuePotion);
                _player.getManaPotion(baseAmount);
                break;
            case PotionCapacity.big:
                Debug.Log("Big");
                baseAmount = 15 + _gameMan.baseValuePotion;
                //Debug.LogError(_gameMan.baseValuePotion);
                _player.getManaPotion(baseAmount);
                break;
            case PotionCapacity.giaiant:
                baseAmount = 20 + _gameMan.baseValuePotion;
                _player.getManaPotion(baseAmount);
                break;

        };

    }


}
