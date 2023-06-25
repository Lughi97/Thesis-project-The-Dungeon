using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Text AttackBoost;
    public Text defenceBoost;

    public static CanvasManager instance = null;
    private GameManager _gameManger;
 
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        _gameManger = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void ShowCooldDownAttack(float time)
    {
        AttackBoost.gameObject.SetActive(true);
       
        AttackBoost.text = "AttackBoost: " + time;
    }
    public void ShowCoolDownDefence(float time)
    {
        defenceBoost.gameObject.SetActive(true);
        defenceBoost.text = "DefenceBoost: " + time;
    }

}
