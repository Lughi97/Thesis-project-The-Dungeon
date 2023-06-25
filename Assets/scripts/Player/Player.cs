using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This is the sript of the movement of the player 
/// Plus the damage recived from the enem
/// Attack behaviour of the player
/// Here is passed the sats of the player.
/// </summary>
public class Player : MonoBehaviour
{
    //variable for the movement
    [SerializeField]
    private float _speed = 2.0f;// base speed of the player
    private Transform _GFX;// the graphycs of the player
    private float _flipX;// to flip the character in the x-axis
    public Vector2 _targetPos; // where we want to move
    private LayerMask _obstacleMask;// the obstacle in wich the player cannot pass through
    private LayerMask _enemyMask;// used to initiate the attack behaviour
    private bool _isMoving;// check if the player is moving
      // attack variable 
   
    public bool _boolAttackEn;
    private float _canAttack=-1f;
   

    // references of other script
    [SerializeField]
    private GameManager _gameManager;
    private EnemyManager _enemyManager;
    private GameOver _gameOver;
    private CanvasManager _canvas;
    [HideInInspector] public PlayerStats _playerStats;
    public SpecialAttackScripts _spk;


    // The variable pass for the playerStats

    // modify the payer class system and transfer in the player class
    //[SerializeField]
    public float currentHp;
    [SerializeField]
    private float _currentStamina;
    //[SerializeField]
    //private float _maxStamina;
    [SerializeField]
    public float playerAttack;
    [SerializeField]
    public float playerDefence;
    [SerializeField]
    private float _playerMana;
    //[SerializeField]
    public float playerPower;
   // [SerializeField]
    public int playerLevel;
    [SerializeField]
    private float _playerAttackRate;
    [SerializeField]
    private float strainStamina = 1;


    //position around the player for only the roundSpecialAttack;
    public Vector2 up,down,left,right ;
    //bool potion

    public bool staminaBoostActive = false;
    public bool attackBoostActive = false;
    public bool defenceBoostActive = false;
    // the textPopUp
    public GameObject textPopUp;

    //audio
    public AudioClip walkAudio;
    public AudioClip[] attackSound;
    private AudioSource _audioSource;


    public InventoryObject inventory;
  
    
    
  //  private Animator _animator;
  
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        _GFX = GetComponentInChildren<SpriteRenderer>().transform;
        _flipX = _GFX.localScale.x;// get the scale of the x to flip the player
        _obstacleMask = LayerMask.GetMask("Wall", "Enemy");
        _enemyMask = LayerMask.GetMask("Enemy");

        _audioSource = GetComponent<AudioSource>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
      
        _playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
        _spk = GameObject.Find("Explosion").GetComponent<SpecialAttackScripts>();
        _gameOver = GameObject.Find("GameManager").GetComponent<GameOver>();
        _canvas = GameObject.Find("Canvas").GetComponent<CanvasManager>();

      //  explosion= GetComponentsInChildren<>


        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = walkAudio;
        }
        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager on the player is NULL");
        }
        else
        {
            currentHp = _gameManager.MaxPlayerHealt;
            _currentStamina = _gameManager.MaxPlayerStamina;
            //_maxStamina= _gameManager.MaxPlayerStamina;
            _playerMana = _gameManager.MaxPlayerMana;
           
        }
     
    
        playerDefence = _playerStats.defence;
        playerAttack = _playerStats.attack;
        playerLevel = _playerStats.levelPlayer;
        playerPower = _playerStats.power;
        StartCoroutine(CurrentStamina());
        StartCoroutine(ManaRefill());
        StartCoroutine(timeLimit());
        StartCoroutine(delayUpdate());
    }
  

    void Update()
    {

     



    }
   IEnumerator delayUpdate()
    {
        while(true)
        {
            if (_gameManager._setUp == false && _gameOver._isGameOver == false)
            {
                PlayerStats();
                PlayerMovement();
               if (_spk.activeExplosion==false)
                    ExplosionSpAttack();

            }

            yield return null;
        }
    }
    
    void  PlayerStats()
    {

        if (_playerStats.equiped[1] == true)
            _currentStamina = _gameManager.currentStamina;
        if (_playerStats.equiped[2] == true)
            playerPower = _playerStats.power;
        if (_playerStats.equiped[3] == true)
            playerDefence = _playerStats.defence;
        if (_playerStats.equiped[5] == true)
            playerAttack = _playerStats.attack;
        if (_playerStats.equiped[6] == true)
            _playerAttackRate = _playerStats.attackRate;
        if (_playerStats.equiped[7] == true)
            strainStamina = _playerStats.strainStamina;

        if (_playerStats.equiped[1] == false)
        {
          
           if(_currentStamina>=_gameManager.MaxPlayerStamina)
                _currentStamina = _gameManager.MaxPlayerStamina;

        }
        if (_playerStats.equiped[2] == false)
            playerPower = _playerStats.power;
        if (_playerStats.equiped[3] == false)
            playerDefence = _playerStats.defence;
        if (_playerStats.equiped[5] == false)
            playerAttack = _playerStats.attack;
        if (_playerStats.equiped[6] == false)
            _playerAttackRate = _playerStats.attackRate;
        if (_playerStats.equiped[7] == false)
            strainStamina = _playerStats.strainStamina;
        if (_playerStats.levelUp == true)
        {
            playerDefence = _playerStats.defence;
            playerAttack = _playerStats.attack;
            playerLevel = _playerStats.levelPlayer;
            playerPower = _playerStats.power;
            _currentStamina = _gameManager.MaxPlayerStamina;
            currentHp = _gameManager.MaxPlayerHealt;
         

        }

    }
    

    IEnumerator timeLimit()// the health goes down over time 
    {

        while (currentHp > 0)
        {   if(currentHp>=_gameManager.currPlayerHealt)
            {
                currentHp = _gameManager.currPlayerHealt;
            }
            if (_gameManager._setUp == false && _gameManager._gameOver._isGameOver==false)
            {
                currentHp--;
                _gameManager.updateHp(currentHp);
            }
            if (currentHp <= 0.2)
            {
                currentHp = 0;
                _gameManager.GameOver();
            }
            yield return new WaitForSeconds(2f);
        }

    }
    

    IEnumerator ManaRefill()
    {
        while (_playerMana <= _gameManager.MaxPlayerMana)
        {
            if (_gameManager._setUp == false && _gameManager._gameOver._isGameOver == false)
            {
                if (_playerMana< _gameManager.MaxPlayerMana)
                {
                    _playerMana += 2;
                    _gameManager.UpdateMana(_playerMana);
                }
            }


            yield return new WaitForSeconds(3f);
        }
    }
    // the main movement of the player
    void PlayerMovement()
    {
        //System.Math.Sign  return the value of 1 when positve and -1 negative
        float horizontal = System.Math.Sign(Input.GetAxisRaw("Horizontal"));

        float vertical = System.Math.Sign(Input.GetAxis("Vertical"));

        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            if (Mathf.Abs(horizontal) > 0)// get the absoulte value of the horizontal value
            {
                _GFX.localScale = new Vector2(_flipX * horizontal, _GFX.localScale.y);// x=1 or -1;
            }
            if (!_isMoving)// if the characther is stil to calculate the movement form one square to another.
            {
                if (Mathf.Abs(horizontal) > 0)
                {
                    _targetPos = new Vector2(transform.position.x + horizontal, transform.position.y);
                    
                }
                else if (Mathf.Abs(vertical) > 0)
                {
                    _targetPos = new Vector2(transform.position.x, transform.position.y + vertical);
                  

                }
                Vector2 hitSize=  Vector2.one*0.8f;
                Collider2D hit = Physics2D.OverlapBox(_targetPos, hitSize,0, _obstacleMask);
                //check for collison
                if (!hit)
                {
                    // transform.Translate(new Vector3(horizontal, vertical, 0) * _speed * Time.deltaTime);
                    StartCoroutine(SmoothMove());
                }
                
                if(CollisionPE(hitSize))
                    PlayerCanAttack();
            }
            
        }
    }
   public Collider2D CollisionPE(Vector2 _hitSize)
    {
        Collider2D hit= Physics2D.OverlapBox(_targetPos, _hitSize, 0, _enemyMask);

        return hit;
    }
   
    /*void InputAnim()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A))
        {
            _animator.Play("Player_Move_anim");
            Debug.Log("keyPreessed");
        } else  //Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A))
        {
            _animator.SetTrigger("IdleTrigger");
        }
    }
    */

   IEnumerator SmoothMove()// gradual movement form one square to the next
    {
        _isMoving = true;
        _audioSource.clip = walkAudio;
        while (Vector2.Distance(transform.position, _targetPos) > 0.01f)// take the distance between the current player position and the target position that ew want to get to
        {
            if (Input.GetKey(KeyCode.LeftShift)&& _currentStamina>0)
             _speed = 4f;
            else {
                _speed = 2f;
            }
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);
           
            yield return null; // to skip to the next frame
        }
        _audioSource.Play();
        transform.position = _targetPos;
        _isMoving = false;
    }
  
    
    // deminsh the current stamina throug attack and fast movement 
    IEnumerator CurrentStamina()
    {
        while (true)
        {
            if (_gameManager._setUp == false && _gameManager._gameOver._isGameOver == false)
            {
                yield return new WaitForSeconds(1f);
                _boolAttackEn = false;
                // Debug.Log("CurrentStamina");
                if ((!_isMoving || _speed == 2) && _currentStamina < _gameManager.MaxPlayerStamina && _boolAttackEn == false)
                {
                    _currentStamina += 2;
                    _gameManager.UpdateStamina(_currentStamina);
                    yield return new WaitForSeconds(0.5f);//every 0.8f seconds the stamina refill
                }
                else if (_isMoving && _currentStamina >= 0 && Input.GetKey(KeyCode.LeftShift))
                {
                    _currentStamina -= 2;
                    _gameManager.UpdateStamina(_currentStamina);
                    yield return new WaitForSeconds(0.5f);//every 0.5f seconds the stamina deminish
                }

            }

            yield return null;// simulate the same behaviour of the update function but slower

        }
    }
   
    /*
   IEnumerator CurrentMana()
    {
       
        while (true)
        {
            if (_playerMana < 1)
            {
                _playerMana = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }

    }
    */

    void PlayerCanAttack()
    {
        int roll = (Random.Range(0, 101));// variable to decide if the attack miss or not
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canAttack && _currentStamina > strainStamina && !_isMoving)
        {
            _boolAttackEn = true;

            if (roll < 100)
            {
                float hitDamage = Mathf.Round(playerAttack * (Mathf.Log(_currentStamina, 2)));// the damage varies
                _currentStamina -= strainStamina;
                if (_currentStamina < 0)
                    _currentStamina = 0;

                
                _gameManager.UpdateStamina(_currentStamina);
                //PlayerAttack();

                int attNum = Random.Range(0, 3);
                _canAttack = Time.time + _playerAttackRate;

                //to selecet the attack sound
                switch (attNum)
                {
                    case 0:
                        _audioSource.clip = attackSound[0];
                        _audioSource.Play();
                        break;
                    case 1:
                        _audioSource.clip = attackSound[1];
                        _audioSource.Play();
                        break;
                    case 2:
                        _audioSource.clip = attackSound[2];
                        _audioSource.Play();
                        break;


                }
                _enemyManager.ContorlSpecificEnemy(hitDamage, playerPower, playerLevel);

            }
            else
            {
                _currentStamina -= strainStamina;
                _gameManager.UpdateStamina(_currentStamina);

                //Debug.Log("AttackMissed");
            }

            // _audioSource.clip = _attackSound;
            //_animator.SetTrigger("AttackTrigger");

            //Debug.Log("Attack");
        }
       // _boolAttackEn = false;
    }
    // the specials attack of the player (3 of them ( in the future unlock with the level)


    void ExplosionSpAttack()
    {
        if ( Input.GetKey(KeyCode.G) && _spk.activeExplosion == false && _playerMana>=_spk.usedMana && _playerMana != 0)
        {
            //activeExplosion = true;
         _spk.gameObject.SetActive(true);
            Debug.Log("Pressed G: activeExplosion");
            _spk.ActivateAttack();
            _playerMana -= _spk.usedMana;
            if (_playerMana < 0)
                _playerMana = 0;
        
            _gameManager.UpdateMana(_playerMana);
          
           
        }
      
    }
   


    public void PlayerDamage(float damage, int enemyLevel, float enemyPower)
    {
        float realDamage = Mathf.Round(_gameManager.DAMAGE(damage, enemyLevel, playerDefence, enemyPower, playerPower));
        currentHp -= realDamage;
        if (currentHp <= 0)
            currentHp = 0;
        _gameManager.updateHp(currentHp);
        if (textPopUp && currentHp>0)
        {
            ShowText( realDamage);
        }
       // Debug.Log("Enemy damage: " + realDamage);
     
    

    }


    // show the text when the player is hit by the enemy
    void ShowText(float textFloat)
    {
        var textGo = Instantiate(textPopUp, transform.position, Quaternion.identity);
        textGo.GetComponent<TextMesh>().text =string.Format("{0:0.00}",textFloat);
    }


    //potions and powerUps
    public void getHealtPotion(float amount)
    {
        //Debug.Log(amount);
        currentHp += amount;
        if (currentHp > _gameManager.MaxPlayerHealt)
            currentHp = _gameManager.MaxPlayerHealt;

       // Debug.Log(healtBar);
        ShowText(amount);
        _gameManager.updateHp(currentHp);
    }

    public void attackBoost(float amount,float time)
    {
        attackBoostActive = true;
        playerAttack *= amount;
     
      
        StartCoroutine(PowerDownAttack(time, amount));
    }
    public void defenceBoost(float amount, float time)
    {
        defenceBoostActive = true;
        playerDefence *= amount;
      
        StartCoroutine(PowerDownDefence(time, amount));
    }
    public void getStaminaPotion(float amount)
    {
        _currentStamina += amount;
        if (_currentStamina > _gameManager.MaxPlayerStamina)
            _currentStamina = _gameManager.MaxPlayerStamina;

        // Debug.Log(healtBar);
        ShowText(amount);
        _gameManager.UpdateStamina(_currentStamina);
    }
    public void getManaPotion(int amount)
    {
        _playerMana += amount;
        if (_playerMana > _gameManager.MaxPlayerMana)
            _playerMana = _gameManager.MaxPlayerMana;

        // Debug.Log(healtBar);
        ShowText(amount);
        _gameManager.UpdateMana(_playerMana);
    }

    IEnumerator PowerDownAttack(float time,float amount)
    {
        while (time > 0.01f)
        {
            _canvas.ShowCooldDownAttack(time);
            time--;
            if (_gameOver._isGameOver == true)
                break;
            yield return new WaitForSeconds(1f);

        }
        _canvas.AttackBoost.gameObject.SetActive(false);
        playerAttack /=amount;
       
        attackBoostActive = false;
        
    }
    IEnumerator PowerDownDefence(float time, float amount)
    {
        while (time > 0.01f)
        {
            _canvas.ShowCoolDownDefence(time);
            time--;
            if (_gameOver._isGameOver == true)
                break;
          
             
            yield return new WaitForSeconds(1f);

        }
        _canvas.defenceBoost.gameObject.SetActive(false);
        playerDefence /= amount;
        defenceBoostActive = false;
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<GroundItem>();
        if(item)
        {
            Item Item = new Item(item._item);
            if (inventory.AddItem(Item, 1))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
       
    }

  
}
