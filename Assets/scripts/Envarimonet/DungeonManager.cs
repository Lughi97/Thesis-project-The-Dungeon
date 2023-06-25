using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Here is created the enviroment of the game 
/// We use the random Walker Procedural alghoritm to randomly creates different rooms, caverns and placement of the enemy
/// Is used a tile ( a cube 3d) to layout the ground work of the game and then with the tileSpawner are replaeced with floor and wall prefab
/// Then the enemy and object(not done ) are placed in the ennviroment randomly
/// </summary>
public enum DungeonType{Cave,Room,CrossRoom,VerticalCrossRoom,BossRoom};
public class DungeonManager : MonoBehaviour
{
    public  DungeonType _dungeonType;
    private GameManager _gameMan;
    [HideInInspector]public float minX, maxX, minY, maxY;// this is the value of the poisiton of the max position and min position of tile 

    
    public GameObject[] _randomEnemy,items, wepon;
    public GameObject wallPrefab, floorPrefab, tilePrefab, exitPrefab,entrancePrefab;// all prefab needed for the map ( the tile prefab is the one used for the random walker
    public Player _player;
    private List<Vector3> _listPositions = new List<Vector3>();// the list of position that are going to create the map
    [SerializeField]
    //[Range(50, 2000)]
    private int _totalTileCount=0;

    private LayerMask _floorMask;
    private LayerMask _wallMask;

    [Range(0, 100)] public int enemyPercenage;
    [Range(0, 100)] public int itmesPercentage;
    [Range(0, 100)] public int weponsPercentage;


   
    // Start is called before the first frame update
    void Start()
    {
        //mask for the movement the enemy
        _floorMask = LayerMask.GetMask("Floor");
        _wallMask = LayerMask.GetMask("Wall");

        _gameMan = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _totalTileCount = _gameMan.totalTileCount;
        
        //decide on the editor witch type of map to use
        switch (_dungeonType)
        {
            case DungeonType.Cave:
                RandomWalkerCaverns();
                break;
            case DungeonType.Room:
                RoomWalker();
                break;
            case DungeonType.BossRoom:
                BossRoomWalker();
                break;
            case DungeonType.CrossRoom:
                CrossRoomWalker();
                break;
            case DungeonType.VerticalCrossRoom:
                VerticalCorssWalker();
                break;
        }
        
       // BossRoomWalker();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // The different Map Styles

    void RandomWalkerCaverns()// create a cavern like map
    {
      Vector3 currentPos = Vector3.zero;// starting point of the walker;
        //set a floor tile in the current position
        _listPositions.Add(currentPos);
        while (_listPositions.Count < _totalTileCount)
        {
            currentPos += RandomDirection();
           
            if (!InTileList(currentPos))// if current postion is not in the floor list add the position to the floor list
            {
                _listPositions.Add(currentPos);
            }
        }
       
        //add the exitdoor
        StartCoroutine(DelayProgress());// after the tiles destoried run the exit door function
    }

    void RoomWalker()// create Castle layout
    {

        Vector3 currentPos = new Vector3(0,0,0);
        while (_listPositions.Count < _totalTileCount)
        {
            currentPos = HallwayLenght(currentPos);
            RandomRoom(currentPos);
        }
        StartCoroutine(DelayProgress());

    }

    void CrossRoomWalker()//  cross room
    {
        Vector3 currentPos = Vector3.zero;
        while (_listPositions.Count < _totalTileCount)
        {
            currentPos = HallwayLenght(currentPos);
            RandomCrossRoom(currentPos);
        }
        StartCoroutine(DelayProgress());
    }
    void VerticalCorssWalker()// vertical corss room
    {
        Vector3 currentPos = Vector3.zero;
        while (_listPositions.Count < _totalTileCount)
        {
            currentPos = HallwayLenght(currentPos);
            RandomVerticalCrossRoom(currentPos);
        }
        StartCoroutine(DelayProgress());
    }
    void BossRoomWalker()
    {

        Vector3 currentPos = Vector3.zero;
        while (_listPositions.Count < _totalTileCount)
        {

            BossRoom(currentPos);
        }
        StartCoroutine(DelayProgress());

    }
    // function that checks if there are 2 tiels in the same position
    bool InTileList(Vector3 myPos)
    {
        for (int i = 0; i < _listPositions.Count; i++)// check if there are to tiles with the same postion
        {
            if (Vector3.Equals(myPos, _listPositions[i]))
            {
                return true;// there are two in the same place 
            }
        }
        return false;
    }
    //return the direction of the walker randomly
    Vector3 RandomDirection()
    {
        switch (Random.Range(1, 5))// return with direction where to go
        {
            case 1:
                return Vector3.up;
            case 2:
                return Vector3.right;
            case 3:
                return Vector3.down;
            case 4:
                return Vector3.left;

        }
        return Vector3.zero;//the function need a return
    }
    // building the Hallway 
    Vector3 HallwayLenght(Vector3 myPos)
    {
        Vector3 walkDirection = RandomDirection();// the direction of the hallway
        int walkLenght = Random.Range(5, 12);// the lenght of the walk
        for(int i=0; i<=walkLenght;i++)
        {
            if(!InTileList(myPos+walkDirection))
            {
                _listPositions.Add(myPos + walkDirection);
            }
            myPos += walkDirection;// update the walk direction
        }
        return  myPos;
    }
     void RandomRoom(Vector3 myPos)
    {
        // the width and hieght generated form the  center of the position that we stading on
        int width = Random.Range(1, 3);
        int height = Random.Range(1, 4);

        // all the position of the room
        for (int x = -width; x <= width; x++)
        {
            for (int y = -height; y <= height; y++)
            {
                // checking all the area that the virtual walker is standing, checking if there are any floor in this positions
                Vector3 offset = new Vector3(x, y, 0);
               
                if (!InTileList(myPos + offset))
                { //if current postion is not in the floor list add the position to the floor list
                    _listPositions.Add(myPos + offset);
                }
            }
        }
    }
    void RandomCrossRoom(Vector3 myPos)
    {
        int width = Random.Range(6,10);
        int height = Random.Range(6, 10); 


        for (int x = -width; x <= width; x++)
        {
            for (int y = -height; y <= height; y++)
            {
                if (x == y ||x==y-1 ||x==y+1||x==y+2|| x+y==width-1|| x + y == width - 2||x + y == width - 3)
                {
                    // checking all the area that the virtual walker is standing, checking if there are any floor in this positions
                    Vector3 offset = new Vector3(x, y, 0);

                        if (!InTileList(myPos + offset))
                        { //if current postion is not in the floor list add the position to the floor list
                            _listPositions.Add(myPos + offset);
                        }
                }
            }

        }
    }
    void RandomVerticalCrossRoom(Vector3 myPos)
    {
        int width = Random.Range(1, 6);
        int height = Random.Range(1, 6);


        for (int x = -width; x <= width; x++)
        {
            for (int y = -height; y <= height; y++)
            {
                if (x == 0 || y==0||x==1|| y==1||x==-1||y==-1)
                {
                    // checking all the area that the virtual walker is standing, checking if there are any floor in this positions
                    Vector3 offset = new Vector3(x, y, 0);

                    if (!InTileList(myPos + offset))
                    { //if current postion is not in the floor list add the position to the floor list
                        _listPositions.Add(myPos + offset);
                    }
                }
            }

        }

    }
        




    
    void BossRoom(Vector3 myPos)
    {
        int width = Random.Range(1, 20);
        int height = Random.Range(1, 20);

        for (int x = -width; x <= width; x++)
        {
            for (int y = -height; y <= height; y++)
            {
                Vector3 offset = new Vector3(x, y, 0);
                if (!InTileList(myPos + offset))// if current postion is not in the floor list add the position to the floor list
                {
                    _listPositions.Add(myPos + offset);
                }
            }
        }
    }

    IEnumerator DelayProgress()// place the tiles in the positions snd  place the exit door and the reandom items
    {

        for (int i = 0; i < _listPositions.Count; i++)// Instantiate the tileSpawner in position of the list 
        {
            GameObject goTile = Instantiate(tilePrefab, _listPositions[i], Quaternion.identity) as GameObject;
            goTile.name = tilePrefab.name;
            goTile.transform.SetParent(transform);
        }
        // find arry of this object type o many tile there are
        EntrancePosition();
        while (FindObjectsOfType<TileSpawner>().Length>0 )// wating all the tile spawner are destoried
        {
           // place all the floor and the wall prefab in the game scene
            yield return null;
            // unitill none tile remains;
        }
        NextLevel();

        Vector2 hitSize = Vector2.one * 0.8f;
        //use minX,maX,minY,maxY t0 place the itmes;
        // minX-2 because place in the wall or autsied can happend
        for (int x = (int)minX - 2; x <= (int)maxX + 2; x++)
        {
            for (int y = (int)minY - 2; y <= (int)maxY + 2; y++)
            {
                Collider2D hitFloor = Physics2D.OverlapBox(new Vector2(x, y), hitSize, 0, _floorMask);// collide the floor mask
                if (hitFloor)
                {
                    if (!(Vector2.Equals(hitFloor.transform.position, _listPositions[_listPositions.Count - 1])))
                    {
                        // all the collide for the 4 direction against the possible  wall
                        Collider2D hitTop = Physics2D.OverlapBox(new Vector2(x, y + 1), hitSize, 0, _wallMask);
                        Collider2D hitRight = Physics2D.OverlapBox(new Vector2(x + 1, y), hitSize, 0, _wallMask);
                        Collider2D hitBottom = Physics2D.OverlapBox(new Vector2(x, y - 1), hitSize, 0, _wallMask);
                        Collider2D hitLeft = Physics2D.OverlapBox(new Vector2(x - 1, y), hitSize, 0, _wallMask  );
                        // if there is a wall on each side and there are no opposing wall on the top and the bottom and on the left and the right
                       // RandomItem(hitFloor, hitTop, hitRight, hitBottom, hitLeft);
                        RandomEnemies(hitFloor, hitTop, hitRight, hitBottom, hitLeft);
                        RandomItems(hitFloor, hitTop, hitRight, hitBottom, hitLeft);
                        RandomWepons(hitFloor, hitTop, hitRight, hitBottom, hitLeft);
                    }
                }

            }
        }
    }
    //place the potions int the Enviroment
    void RandomItems(Collider2D hitFloor, Collider2D hitTop, Collider2D hitRight, Collider2D hitBottom, Collider2D hitLeft)
    {
        if((hitTop||hitRight||hitBottom||hitLeft) && !(hitTop && hitBottom) && !(hitLeft && hitRight))
        {
            int roll = Random.Range(0, 101);
            if(roll< itmesPercentage)
            {
                int itmesIndex = Random.Range(0, items.Length);
                GameObject itmes = Instantiate(items[itmesIndex], hitFloor.transform.position , Quaternion.identity) as GameObject;
                itmes.name = items[itmesIndex].name;
            }
        }
    }
    void RandomWepons(Collider2D hitFloor, Collider2D hitTop, Collider2D hitRight, Collider2D hitBottom, Collider2D hitLeft)
    {
        if ((hitTop || hitRight || hitBottom || hitLeft) && !(hitTop && hitBottom) && !(hitLeft && hitRight))
        {
            int roll = Random.Range(0, 101);
            if (roll < weponsPercentage)
            {
                int weponIndex = Random.Range(0, wepon.Length);
                GameObject gowepon = Instantiate(wepon[weponIndex], hitFloor.transform.position, Quaternion.identity) as GameObject;
                gowepon.name = wepon[weponIndex].name;
            }
        }
    }
    //place the enemies randomly in the map
    void RandomEnemies(Collider2D hitFloor, Collider2D hitTop, Collider2D hitRight, Collider2D hitBottom, Collider2D hitLeft)
    {
        if (!hitTop && !hitRight && !hitBottom && !hitLeft)// we ae in an open space not near the wall
        {
            int roll = Random.Range(0, 101);// for how many items to spawn
            if (roll <= enemyPercenage)
            {
                int enemyIndex = Random.Range(0, _randomEnemy.Length);
                GameObject goEnemy = Instantiate(_randomEnemy[enemyIndex], hitFloor.transform.position, Quaternion.identity) as GameObject;
                goEnemy.name = _randomEnemy[enemyIndex].name;
                GameObject EnemyManager = GameObject.Find("EnemyManager");
                goEnemy.transform.SetParent(EnemyManager.transform);
            }
        }
    }
    //position the door in the first tile of the game where the player is standing
    void EntrancePosition()
    {
        Vector3 DoorPosition = _listPositions[0];// the first position of the Map
        GameObject goEntrance = Instantiate(entrancePrefab, DoorPosition, Quaternion.identity);
        goEntrance.name = entrancePrefab.name;
        goEntrance.transform.SetParent(transform);
        _player.transform.position = _listPositions[0];
        //GameObject goPlayer = Instantiate(PlayerPrefab, PlayerPosition, Quaternion.identity);
        //goPlayer.name = PlayerPrefab.name;
    }

    //place the exit door at the last position of the walker
    void NextLevel()
    {
        
            Vector3 DoorPosition = _listPositions[_listPositions.Count - 1];// the last position of the Map
            GameObject goexit = Instantiate(exitPrefab, DoorPosition, Quaternion.identity);
            goexit.name = exitPrefab.name;
            goexit.transform.SetParent(transform);
        
    }

  
}
