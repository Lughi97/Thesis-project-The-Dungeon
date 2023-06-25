using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// Here is the trigger to go from a level to the next one, not by changing scene but loading the current active scene 
/// </summary>
[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class ExitNext : MonoBehaviour
{
    private  GameManager _gameManager;
    private CanvasManager _canvas;
    private PlayerStats _playerStats;
    public float exp=50;
 
   
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _canvas = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        _playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
    }
    // reset the game form level 1
    private void Reset()
    {
        // cahnged the values via code
        GetComponent<Rigidbody2D>().isKinematic = true;
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = Vector2.one *0.1f;
        box.isTrigger = true;
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {           
            _canvas.AttackBoost.gameObject.SetActive(false);
            _canvas.defenceBoost.gameObject.SetActive(false);
            _gameManager.UpdateLevel();
            exp = exp+2*Mathf.Log(_gameManager.levelField,2);
            _playerStats.UpdateExp(exp);
            StartCoroutine(LoadAsynScene());
          //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        
        }
    }
    IEnumerator LoadAsynScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while (!asyncLoad.isDone)
            yield return null;


    }
}
