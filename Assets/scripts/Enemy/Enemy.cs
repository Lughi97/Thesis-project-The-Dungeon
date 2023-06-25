using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this scritp describe the behaviou of the enmy form patrolling to chase and attack the player 
/// </summary>
public class Enemy : MonoBehaviour
{// the life and satst of the enemy 
    private EnemyStats _enemyStats;
   // private PlayerStats _playerStats;
    public float enemyMaxHealt;
    [SerializeField]
    private float _curentEnemyHealth;
    [SerializeField]
    private Vector2 _currrentAttack;
    [SerializeField]
    private float _enemyPower;
    [SerializeField]
    private float _enemyDefence;
    [SerializeField]
    private int _enemyLevel;
   [SerializeField]
    public float _speed;
   
    public float _expGain;
    public bool enemyDead = false;

    // the layermask for the enemy to use
    private LayerMask _obastacleMask, _walkableMask;
    private List<Vector2> _availableMovementPosition = new List<Vector2>();// wich direction are available to move in 
    public Vector2 _currentPos; // the curent position of the enemy
    private bool _isMoving = false;// to prevent the update form running again the Patrol function again unitll the curutine is done 

    // Pathfinding variable t follow the player
    public float allertRange;
    private float _attackRange = 1.1f;// less than the suare root of 2
    public Vector2 waitAttack;
    public Vector2 chaseSpeed;

    // references to external scripts
    private Player _player;
    private SpecialAttackScripts _spk;
    private List<Node> _nodesList = new List<Node>();
    private GameManager _gameManager;
    public  GameObject floatingText;
    private GameOver _gameOver;


   
   
    // Start is called before the first frame update
    void Start()
    {
        _enemyStats = GetComponent<EnemyStats>();
      //  _playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // _enemyHealthBars = GameObject.Find("Canvas").GetComponentInChildren<HealthBar>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _gameOver = GameObject.Find("GameManager").GetComponent < GameOver >();
       // _floatingNumber = GameObject.Find("DamgePopUp").GetComponent<FloatingNumber>();
        enemyMaxHealt = _enemyStats.hp;
        _currrentAttack = _enemyStats.attack;
        _enemyPower = _enemyStats.power;
        _enemyLevel= _enemyStats.level;
        _enemyDefence = _enemyStats.defence;
        _curentEnemyHealth = enemyMaxHealt;
        _expGain = _enemyStats.expHold;
        //_enemyHealthBars.setMaxHealt(enemyMaxHealt);
        //_enemyHealthBars.transform.SetParent(transform);

        _obastacleMask = LayerMask.GetMask("Wall", "Enemy", "Player");// cannot pass trough
        _walkableMask = LayerMask.GetMask("Wall","Enemy");// cannpt walk in to while chasing the player
        _currentPos = transform.position;
        _player = FindObjectOfType<Player>();
        
         StartCoroutine(EnemyMovement());
        
        
       

    }
    private void Update()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    _curentEnemyHealth--;

        //  _enemyHealthBars.SetHealt(_curentEnemyHealth);
        //  }
        Updatestats();
    }
    // when the player is not near the enemty, he follow a random  path
    void Patrol()
    {
        _availableMovementPosition.Clear();// clear each update the movement list for the future list

        // checking for the collision in all direction of the enemy if there is no collision we put the direction in the list
        Vector2 hitSize = Vector2.one * 0.8f;
        Collider2D hitUp = Physics2D.OverlapBox(_currentPos + Vector2.up, hitSize, 0, _obastacleMask);
        if (!hitUp) _availableMovementPosition.Add(Vector2.up);
        Collider2D hitRight = Physics2D.OverlapBox(_currentPos + Vector2.right, hitSize, 0, _obastacleMask);
        if (!hitRight) _availableMovementPosition.Add(Vector2.right);
        Collider2D hitDown = Physics2D.OverlapBox(_currentPos + Vector2.down, hitSize, 0, _obastacleMask);
        if (!hitDown) _availableMovementPosition.Add(Vector2.down);
        Collider2D hitLeft = Physics2D.OverlapBox(_currentPos + Vector2.left, hitSize, 0, _obastacleMask);
        if (!hitLeft) _availableMovementPosition.Add(Vector2.left);

        if (_availableMovementPosition.Count > 0)
        {
            int randomIndex = Random.Range(0, _availableMovementPosition.Count);// all the elemets of the list 
            _currentPos += _availableMovementPosition[randomIndex];// add to the enemy position the new direction
        }

        StartCoroutine(SmootMovement(Random.Range(0.01f, 1.0f)));// to move the enemy 
    }

    //function of the enemy movement form a tile to the next
    IEnumerator SmootMovement(float speed)
    {
        _isMoving = true;
        while (Vector2.Distance(transform.position, _currentPos) > 0.01f)// check the distance between the enemy position to the position that was updateed
        {
            transform.position = Vector2.MoveTowards(transform.position, _currentPos, _speed * Time.deltaTime);
            yield return null;//wait 1 frame
        }
        transform.position = _currentPos;
        yield return new WaitForSeconds(speed);
        _isMoving = false;// the update can call Patrol agian
    }
    void checkNode(Vector2 checkPoint, Vector2 parent)//check if  position of the tile ( with is parent) is walkable
    {
        Vector2 size = Vector2.one * 0.5f;
        Collider2D hit = Physics2D.OverlapBox(checkPoint, size, 0, _walkableMask);
        {
            if (!hit)// if is walkable ten add the node to the list 
            {
                _nodesList.Add(new Node(checkPoint, parent));// add the nodes to the luist if the area is walkable by the enemy
            }
        }
    }
    Vector2 FindNextStep(Vector2 startPos, Vector2 targetPos)// find the clear path to the player // the start position is the current enemy position
    {
        int listIndex = 0;// to keep trak of the index of the list 
        Vector2 myPos = startPos;// keep trak of the position
        _nodesList.Clear();//first clear all the old node list 
        _nodesList.Add(new Node(startPos, startPos)); ;// add the first node we the poOition and the parent where we chaingn form the position
        while (myPos != targetPos && listIndex < 1000 && _nodesList.Count > 0)// untill the enemy position is not equal to the player position // the 1000 to not search all the map
        {
            //chek the uo down left right tiels if the tiles are walkable we can add to the node list
            checkNode(myPos + Vector2.up, myPos);
            checkNode(myPos + Vector2.right, myPos);
            checkNode(myPos + Vector2.down, myPos);
            checkNode(myPos + Vector2.left, myPos);

            listIndex++;
            if (listIndex < _nodesList.Count)// check if there is an other element to the list 
            {
                myPos = _nodesList[listIndex].position;//make it equal to theat nodeLidst position
                //untill it reach the player position// checks all the tile surrounding it 
            }
           
        }
        if (myPos == targetPos)// the player could have move in another direction in the mintime
        {
            _nodesList.Reverse();// crawll backwards to the list form
            for (int i = 0; i < _nodesList.Count; i++)
            {
                if (myPos == _nodesList[i].position)// cehck all the items if there equal to the myPos
                {
                    if (_nodesList[i].parent == startPos)// the parent of the matching node same as the starting position
                    {
                        return myPos;// return the position to walk to
                    }
                    myPos = _nodesList[i].parent;// cahnge the position to be equal to the nodes i parent 
                }
            }
        }

        return startPos;
    }
    // basic damage attack, to update 
   void Attack()
    {
        int roll = Random.Range(0, 100);
        if (roll> 15)
        {
            float damageAmounts =Random.Range(_currrentAttack.x, _currrentAttack.y);
            _player.PlayerDamage(damageAmounts,_enemyLevel, _enemyPower);
           //Debug.Log( name + " attaked force of " + damageAmounts + " points of damage!");

        }
        else
        {
           // Debug.Log(name + " attaked and miss!");
        }

    }
    //This is the main funtion for the movement behaviour of the enemy
    IEnumerator EnemyMovement()
    {
       
            while (true)// works in this case like a slower update function
            {
            
                yield return null;// or new wuatfirsecoond (0.1f)
                if (!_isMoving && _gameManager._setUp==false &&_gameOver._isGameOver==false)
                {
                    float dist = Vector2.Distance(transform.position, _player.transform.position);// set up the dist between the enemy and the player
                    if (dist <= allertRange)//*alert
                    {
                        if (dist <= _attackRange && _gameManager._setUp==false &&_gameOver._isGameOver==false)
                        {

                            Attack();
                            yield return new WaitForSeconds(Random.Range(waitAttack.x, waitAttack.y));
                        }
                        else
                        {
                            Vector2 newPos = FindNextStep(transform.position, _player.transform.position);//Is there is a clear path to the player and tells the enemy in witch tile have to go next
                            if (newPos != _currentPos)
                            {
                                //chase the player 
                                _currentPos = newPos;
                                StartCoroutine(SmootMovement(Random.Range(0.01f, 1f)));//(chaseSpeed.x,chaseSpeed.y)));
                            }
                            else
                            {
                                Patrol();
                            }
                        }
                    }
                    else
                    {
                        Patrol();// outside this range
                    }
                }

            }
        
    }
    // the enemy recive damage from the player to update
    public void DamageToEnemy(float damage, float power, int level)
    {
        float totalDamage=Mathf.Round(_gameManager.DAMAGE(damage,level,_enemyDefence,power,_enemyPower));
        _curentEnemyHealth -= totalDamage;
       // Debug.Log("Damage: "+ totalDamage);
      //  Debug.Log(transform.name+" currHealth: "+_curentEnemyHealth);
        //trigger text damage
        if (floatingText && _curentEnemyHealth>0)
        {
            ShowFloatingText(totalDamage);
        }
        
        if(_curentEnemyHealth<=0)
        {
            _curentEnemyHealth = 0;
            enemyDead = true;
          
            this.gameObject.SetActive(false);
        }
    }
    // when the enemy is hit by the player the damage create a damage text
    void ShowFloatingText(float totalDamage)
    {
        var textGo = Instantiate(floatingText, transform.position, Quaternion.identity);
        textGo.GetComponent<TextMesh>().text = string.Format("{0:0.00}", totalDamage);
    }
    void Updatestats()
    {
        if(_gameManager.enemyLevelUp==true)
        {
            _enemyDefence = _enemyStats.defence;
            _enemyPower = _enemyStats.power;
            enemyMaxHealt = _enemyStats.hp;
            _curentEnemyHealth = enemyMaxHealt;
            _currrentAttack = _enemyStats.attack;
            _expGain = _enemyStats.expHold;
        }
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Explosion")
        {
            if (_gameManager._setUp == false)
            {
                _spk = GameObject.Find("Explosion").GetComponent<SpecialAttackScripts>();
                Debug.Log("HIT SPECIAL to enemy");

                // damageSpecialAttackToEnemy(specialDamage, _player.playerPower,_player. playerLevel);

                DamageToEnemy(_spk.ExpolsionPower, _player.playerPower, _player.playerLevel);
            }
        }
  
    }
}
