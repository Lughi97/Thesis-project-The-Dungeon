using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Here is manage the collision deteciton for the player to atack the enemy( one of the many in  the array enemy)
/// In future for the special attack see different enemy attack at the same time 
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public Enemy[] enemy;
    private LayerMask _enemyMask;
    public Player _player;
    private PlayerStats _playerStats;
    private float _enemyExp;
    [SerializeField]
    // private HealthBar _enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        _enemyMask = LayerMask.GetMask("Enemy");
        StartCoroutine(FindEnemyScript());
    }
    // find all the enemy in the map
    IEnumerator FindEnemyScript()
    {
        yield return new WaitForSeconds(1f);
        enemy = GetComponentsInChildren<Enemy>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
        yield return null;
    }
  //  public void damageSpecialAttackToEnemy(float spkdamage, float pPower, int pLevel)
    //{
      
    //}
   
    // give to a single enemy the damage not to multiple targets
    public void ContorlSpecificEnemy(float damage, float pPower, int pLevel)
    {
        
        for (int i = 0; i < enemy.Length; i++)
        {
                if (enemy[i]._currentPos == _player._targetPos)
                {
                    enemy[i].DamageToEnemy(damage, pPower, pLevel);
                }
                

                if (enemy[i].enemyDead == true)
                {
                    enemy[i].enemyDead = false;
                    _enemyExp = enemy[i]._expGain;
                    _playerStats.UpdateExp(_enemyExp);

                    
                }
       
        }
    }
}
