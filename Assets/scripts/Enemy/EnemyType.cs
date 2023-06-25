using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy Data", menuName ="Enemy Stats")]
public class EnemyType : ScriptableObject
{
    public string enemyName;
    public string enemyDescription;
    public float hp;
    //public int stamina;
    public float defence;
    public Vector2 attack;
    public float power;
    public int level = 1;
    public float expHold;
}
