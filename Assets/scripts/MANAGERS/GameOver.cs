using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// When the player dies we Eneable the game over image 
/// </summary>
public class GameOver : MonoBehaviour
{
    public bool restart = false;
   // private bool canRestart = false;
    public Text gameOverText;
    public GameObject gameOverImage;
    public GameObject Menu;
    public GameObject Restart;
    public bool _isGameOver = false;
    private GameManager _gameManger;
    public CanvasManager _canvas;
   
    // Start is called before the first frame update
    void Start()
    {
        gameOverImage = GameObject.Find("GameOver");
        _gameManger = GameObject.Find("GameManager").GetComponent<GameManager>();
        _canvas = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        gameOverText = GameObject.Find("GameOver").GetComponentInChildren<Text>();
        Menu.SetActive(false);
        Restart.SetActive(false);
        gameOverImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()// rudimental restart behaviour
    {

    }
    public void restartGame()
    {
        if (_isGameOver ==true)
        {
            _gameManger.levelField = 1;
            _gameManger.levelEnemy = 1;
            _gameManger.totalTileCount = 100;
            _gameManger.currPlayerHealt = _gameManger.MaxPlayerHealt;
            restart = true;
            Menu.SetActive(false);
            Restart.SetActive(false);
            //gameOverImage.SetActive(false);

            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            StartCoroutine(LoadAsynScene());
            _isGameOver = false;
            gameOverImage.SetActive(false);
        }   
    }
   public void goToMenu()
    {
        
      
        StartCoroutine(LoadAsynSceneMenu());
    }
    IEnumerator LoadAsynSceneMenu()
    {
        Destroy(_gameManger.gameObject);
        Destroy(_canvas.gameObject);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        while (!asyncLoad.isDone)
            yield return null;


    }
    IEnumerator LoadAsynScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while (!asyncLoad.isDone)
            yield return null;
        
        
    }
        public void  Show()// show the game over image
    {   if (_isGameOver == false)
        {
            gameOverImage.SetActive(true);
            _isGameOver = true;
            gameOverText.text = "Game Over! " + "You died at level " + _gameManger.levelField + "!";
            StartCoroutine(showButton());
        }
          
        
    }
    IEnumerator showButton()
    {

        yield return new WaitForSeconds(2f);
        Menu.SetActive(true);
        Restart.SetActive(true);

    }
    
    
}
