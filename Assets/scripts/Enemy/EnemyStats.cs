using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
/// <summary>
/// the enemy statistic that are going to be updated if the player get a new level for the game to be engaiging or
/// Make each level harder than the last one.
/// </summary>
public class EnemyStats : MonoBehaviour
{
   // public string name;
    public float hp;
    //public int stamina;
    public float defence;
    public Vector2 attack;
    public float power;
    public int level = 1;
    public float expHold;
    
    
    //public EnemyData data;
   // public Image artWork;

    // keep track of the level of the floor 
    private GameManager _gameManager;

    private void Start()
    {
        
        
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //artWork.sprite = data.artWorkImage;

        StartCoroutine(EnemyLevelUp());
    }
    IEnumerator EnemyLevelUp()
    {
        while (true)
        {
            if (_gameManager.enemyLevelUp == true)
            {

                level = _gameManager.levelEnemy;
                UpdateStats();


                yield return new WaitForSeconds(0.3f);
                _gameManager.enemyLevelUp = false;
            }
           // Debug.Log(level);
            yield return null;
         
        }

    }
  
    void UpdateStats()
    {
        hp += 10;
        defence += 10;
        attack.x += 10;
        attack.y +=10;
        power+=10;
        expHold += 4*Mathf.Pow(level,3)/5;
      //  Debug.LogError("LevelUp Enemy: "+ name );
      
    }

   
}
