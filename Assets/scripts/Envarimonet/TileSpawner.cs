using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{/// <summary>
///  This is the tile Spwaner: is a single withe tile (cube) that is going to do : first in dongmanager, create the envarioment and then, 
///  in the script  put the first the floor tiles and then place the wall around the floor and then destory the tile object
/// </summary>
    private DungeonManager _dungeonMan;
   
    private void Awake()// first create the floor 
    {
        _dungeonMan = GameObject.Find("Environment").GetComponent<DungeonManager>();
        GameObject goFloor = Instantiate(_dungeonMan.floorPrefab, transform.position, Quaternion.identity) ;// instantiate the floor in the posiiton of this TilsSPawner
        goFloor.name = _dungeonMan.floorPrefab.name;// to remove the clone name in the editor
        goFloor.transform.SetParent(_dungeonMan.transform);
    
        if(transform.position.x> _dungeonMan.maxX)
        {
            _dungeonMan.maxX = transform.position.x;
        }else if (transform.position.x<_dungeonMan.minX )
        {
            _dungeonMan.minX = transform.position.x;
        }
        if(transform.position.y> _dungeonMan.maxX)
        {
            _dungeonMan.maxY = transform.position.y;
        }else if(transform.position.y<_dungeonMan.minY)
        {
            _dungeonMan.minY = transform.position.y;
        }
    }
    void Start()// create the wall around the floor
    {
        LayerMask envMask = LayerMask.GetMask("Floor", "Wall");// this is the layer mask
        Vector2 hitSize = Vector2.one * 0.8f;
        //cover the area around the floor tile
        for(int x=-1; x<=1;x++)
        {
            for(int y=-1; y<=1;y++)
            {
                Vector2 targetPos = new Vector2(transform.position.x + x, transform.position.y + y);
                Collider2D hit= Physics2D.OverlapBox(targetPos, hitSize, 0, envMask);
                if(!hit)// if does't hit anything add a wall
                {
                    GameObject goWall = Instantiate(_dungeonMan.wallPrefab, targetPos, Quaternion.identity);
                    goWall.name = _dungeonMan.wallPrefab.name;
                    goWall.transform.SetParent(_dungeonMan.transform);
                }
            }
        }

        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()// draw a withe cube in the center position where start to spwan the tiles, to see where we out the floor
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

}
