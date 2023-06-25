using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Here is managed the level of the game and how the tiles  and the number and strenght of th enemy incrises (enemy still to do it )
/// Also here is manage the damage that both player and enemy recive
/// This link the bars pf the UI to the player healt to make the player see how much healt or stamina he has left
/// A game over prototipe when the player is dead
/// There is only 1 instance of this GameManager to have save some date in it without calling it 
/// every newGame (in this case if we Call for the same scene while we hit the exit or die from an enemy)
/// </summary>
public class GameManager : MonoBehaviour
{
    //manage the player stats
    [HideInInspector]
    public HealthBar _healthBars;
    [HideInInspector]
    private ManaBar _manaBar;
    [HideInInspector]
    private StaminaBar _staminaBar;
    [HideInInspector]
    public float MaxPlayerHealt;
    [HideInInspector]
    public float currPlayerHealt;
    [HideInInspector]
    public float MaxPlayerMana;
    [HideInInspector]
    public float currentMana;
    [HideInInspector]
    public float MaxPlayerStamina;
    [HideInInspector]
    public float currentStamina;
    
    private PlayerStats _playerStats;
    //[HideInInspector]
    public int baseValuePotion=0;




    // manage the level
    public int levelField=1;
    public int levelEnemy = 1;
    public bool enemyLevelUp =false;
    public GameObject _levelImage;
    public Text _levelText;
    public bool _setUp;
   
    public Text Health;
    public Text Stamina;
    public Text Attack;
    public Text Defence;
    [Range(50, 5000)] public int totalTileCount = 100;
    public static GameManager instance = null; // create only a signle instance of the Game Manager 

    //Game Over 
    //public Text gameOverText;
    public Text loadingText;
    [HideInInspector]public GameOver _gameOver;
    public bool goToMenu = false;
   
    // Awake is called immidiatley when the game start
    void Awake()
    {
        if (instance == null) instance = this; // Instantiate the game manager when there is no game Manager;
        else if (instance != this) Destroy(this.gameObject);// if the instance of gamemanager AlreadyExist then Destory GameManager 
        DontDestroyOnLoad(this.gameObject);// this funtcion permits to mantian the Same Game Manager;
        _playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
      
       

        _levelImage = GameObject.Find("LoadingScreen");
        _levelText = GameObject.Find("LevelText").GetComponent<Text>();
     
        loadingText = GameObject.Find("LoadingText").GetComponent<Text>();
        _gameOver =GameObject.Find("GameManager").GetComponent<GameOver>();
        //_pauseMenu = GetComponent<MenuPause>();
       
    }

    private void Start()
    {
        MaxPlayerHealt = _playerStats.hp;
        MaxPlayerMana = _playerStats.mana;
        MaxPlayerStamina = _playerStats.stamina;

        _healthBars = GameObject.Find("Canvas").GetComponentInChildren<HealthBar>();
        _manaBar = GameObject.Find("Canvas").GetComponentInChildren<ManaBar>();
        _staminaBar = GameObject.Find("Canvas").GetComponentInChildren<StaminaBar>();

        _healthBars.setMaxHealt(MaxPlayerHealt);
        _manaBar.setMaxMana(MaxPlayerMana);
        _staminaBar.setMaxStamina(MaxPlayerStamina);


        LoadLevel();
        currPlayerHealt = MaxPlayerHealt;
        currentMana = MaxPlayerMana;
        currentStamina = MaxPlayerStamina;
        _staminaBar.setStamina(currentStamina,MaxPlayerStamina);
        _healthBars.SetHealt(currPlayerHealt, MaxPlayerHealt);
        _manaBar.SetMana(currentMana, MaxPlayerMana);
        // StartCoroutine(timeLimit());
        //StartCoroutine(ManaRefill());


    }
    // Update is called once per frame
    void Update()
    {
        if (_gameOver.restart == true && _gameOver._isGameOver==false )
        {
            LoadLevel();
            // StartCoroutine(ManaRefill());
            MaxPlayerHealt = _playerStats.hp;
            MaxPlayerMana = _playerStats.mana;
            MaxPlayerStamina = _playerStats.stamina;
            currPlayerHealt = MaxPlayerHealt;
            currentMana = MaxPlayerMana;
            currentStamina = MaxPlayerStamina;
            _gameOver.restart = false;
        }
        checkHpStmMana();
        checkKey();

    }

    

    private void checkHpStmMana()
    {
       
        if (_playerStats.levelUp == true)
        {
            MaxPlayerHealt = _playerStats.hp;
            currPlayerHealt = MaxPlayerHealt;
            MaxPlayerMana = _playerStats.mana;
            currentMana = MaxPlayerMana;
            MaxPlayerStamina = _playerStats.stamina;
            currentStamina = MaxPlayerStamina;
            _healthBars.setMaxHealt(MaxPlayerHealt);
            _staminaBar.setMaxStamina(MaxPlayerStamina);
            _manaBar.setMaxMana(MaxPlayerMana);

            _staminaBar.setStamina(currentStamina, MaxPlayerStamina);
            _manaBar.SetMana(currentMana, MaxPlayerMana);
            _healthBars.SetHealt(currPlayerHealt, MaxPlayerHealt);
            _playerStats.levelUp = false;

          
        }
        if (_playerStats.equiped[0] == true)
        {
            MaxPlayerHealt = _playerStats.hp;
            _healthBars.setMaxHealt(MaxPlayerHealt);
            _healthBars.SetHealt(currPlayerHealt, MaxPlayerHealt);
        }
        if (_playerStats.equiped[1] == true)
        {
            MaxPlayerStamina = _playerStats.stamina;
            _staminaBar.setMaxStamina(MaxPlayerStamina);
            _staminaBar.setStamina(currentStamina, MaxPlayerStamina);
        }
        if (_playerStats.equiped[4] == true)
        {
            MaxPlayerMana = _playerStats.mana;
            _manaBar.setMaxMana(MaxPlayerMana);
            _manaBar.SetMana(currentMana, MaxPlayerMana);
        }


        if (_playerStats.equiped[0] == false)
        {
            MaxPlayerHealt = _playerStats.hp;
            if (currPlayerHealt > MaxPlayerHealt)
                currPlayerHealt = MaxPlayerHealt;
            _healthBars.setMaxHealt(MaxPlayerHealt);
            _healthBars.SetHealt(currPlayerHealt, MaxPlayerHealt);
        }

        if (_playerStats.equiped[1] == false)
        {
            MaxPlayerStamina = _playerStats.stamina;
            if (currentStamina > MaxPlayerStamina)
                currentStamina = MaxPlayerStamina;
            _staminaBar.setMaxStamina(MaxPlayerStamina);
            _staminaBar.setStamina(currentStamina, MaxPlayerStamina);
        }
        if (_playerStats.equiped[4] == false)
        {
            MaxPlayerMana = _playerStats.mana;
            if (currentMana > MaxPlayerMana)
                currentMana = MaxPlayerMana;
            _manaBar.setMaxMana(MaxPlayerMana);
            _manaBar.SetMana(currentMana, MaxPlayerMana);
        }
        updateText();

    }
    private void checkKey()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("SaveData");
            Player player = GameObject.Find("Player").GetComponent<Player>();
            _playerStats.equipment.Save();
            player.inventory.Save();

        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            Debug.Log("Load data");
            Player player = GameObject.Find("Player").GetComponent<Player>();
            _playerStats.equipment.Load();
            player.inventory.Load();
        }
    }
    /*
    IEnumerator ManaRefill()
    {
        while (currentMana <= MaxPlayerMana)
        {
            if (_setUp == false)
            {
                if (currentMana < MaxPlayerMana)
                {
                    currentMana+=2;
                    _manaBar.SetMana(currentMana, MaxPlayerMana);
                }
            }
          
            
            yield return new WaitForSeconds(3f);
        }
    }
    */
    //not define fully the main damage function
    public float DAMAGE(float attack, int level, float defence, float powerAttacker, float powerDefender)// int effect and mofifictation to add 
    {
        int N = Random.Range(1, 3);
        float m = (2 * level + 10)*attack*powerAttacker;
        float l = (250 * defence);
        float DamageRecived= ((m/l)+2)*N;
        return DamageRecived;

    }
    public void updateHp(float hp)
    {
        currPlayerHealt = hp;
        _healthBars.SetHealt(currPlayerHealt, MaxPlayerHealt);
        if (currPlayerHealt < 1)
        {
            currPlayerHealt = 0;
            _healthBars.SetHealt(currPlayerHealt, MaxPlayerHealt);
            GameOver();
        }
        else if(currPlayerHealt>= MaxPlayerHealt)
        {
            currPlayerHealt = MaxPlayerHealt;
            _healthBars.SetHealt(currPlayerHealt, MaxPlayerHealt);
        }
    }
    public void UpdateLevel()
    {
        enemyLevelUp = true;
        baseValuePotion += 5;
        levelField++;
        levelEnemy++;
        totalTileCount += ((totalTileCount/100)*50);
        Debug.LogError("TotalTileCount" + totalTileCount);
        currPlayerHealt = MaxPlayerHealt;
        _healthBars.SetHealt(currPlayerHealt, MaxPlayerHealt);
        currentStamina = MaxPlayerStamina;
        _staminaBar.setStamina(currentStamina,MaxPlayerStamina);
        
        LoadLevel();
        Debug.Log("Level " + levelField);

    }
    public void UpdateMana(float amount)
    {
        currentMana = amount;
        _manaBar.SetMana(currentMana, MaxPlayerMana);

    }
    public void UpdateStamina(float amount)
    {
        currentStamina = amount;
        
        _staminaBar.setStamina(currentStamina,MaxPlayerStamina);
    }

    public void GameOver()
    {
        baseValuePotion = 0;
          _gameOver.Show();

    }
    void LoadLevel()
    {
        _setUp = true;

        _levelImage.SetActive(true);
        _levelText.text = "Level " + levelField;
        Invoke("StartLevel",2);
    }
    void StartLevel()
    {
        _setUp = false;
        _levelImage.SetActive(false);
    }
    void updateText()
    {
        Health.text = "Health: " + currPlayerHealt;
        Stamina.text = "Stamina: " + currentStamina;
        Attack.text = "Attack: " + _playerStats.attack;
        Defence.text = "Defence: " + _playerStats.defence;
    }
}

    

   
