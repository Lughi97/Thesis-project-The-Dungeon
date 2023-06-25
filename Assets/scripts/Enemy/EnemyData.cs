using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Holds all the data needed to define a enemy 
/// </summary>
[CreateAssetMenu(fileName ="EnemyType", menuName ="Enemy/standard")]
public class EnemyData : ScriptableObject
{
    public new string name;
    public string description;
    //public Sprite artWorkImage;
    // the stats of the enemy pass to EnemyStats;
    public float hp;
    public float defence;
    public Vector2 attack;
    public float power;
    public int level;
    public float expHold;
    //public float speed;


 

}
