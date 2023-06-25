using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelPercentage : MonoBehaviour
{
    private PlayerStats _playerStats;
    private float _currentExp;
    public Text levelText;
    public Text percentageText;
    private float totalExp;
    public float percentage;
    public Transform _ExpBar;
    // Start is called before the first frame update
    void Start()
    {
        _playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
   
        
    }

    // Update is called once per frame
    void Update()
    {
        totalExp = _playerStats.expToNext;
        _currentExp = _playerStats.playerExp;
        //print("Update");
        percentage = (_currentExp / totalExp) * 100;

        percentageText.text = string.Format("{0:0.00}", percentage)+"%"; 
        levelText.text = "" + _playerStats.levelPlayer;
        _ExpBar.GetComponent<Image>().fillAmount = percentage/100;
    }
      public void UpdateLevel()
    {
        levelText.text = _playerStats.ToString();
    }
}
