using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public static bool _gameIsPaused = false;
    public  bool _displayInventory = false;
    public GameObject pauseMenuUI;
    private GameManager _gameManger;
    public GameObject inventory;
    public GameObject equipmentPlayer;
    public GameObject heroStats;
    public CanvasManager _canvas;
   //private Inventory _inventory;
    // Start is called before the first frame update
    void Start()
    {
        _gameManger = GameObject.Find("GameManager").GetComponent<GameManager>();
        _canvas = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        //_inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
        
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && _gameManger._setUp==false )
        {
            if (_gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }else if(Input.GetKeyDown(KeyCode.I) && _gameManger._setUp==false)
        {
            if (_displayInventory==false)
            {
                OpenInventory();
            }
            else
            {
                CloseInenvotry();
            }
        }
    }

    void Pause()
    {

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // freze the game; or slow motion
        _gameIsPaused = true;
    }
    public void Resume()
    {
       
        Debug.Log("Resume");
        Time.timeScale = 1f;
        _gameIsPaused = false;
        pauseMenuUI.SetActive(false);
    }
    public void OpenInventory()
    {
        //Debug.Log("Invenotry open");
        Time.timeScale = 0f; // freze the game; or slow motion
        inventory.SetActive(true);
        equipmentPlayer.SetActive(true);
        heroStats.SetActive(true);
        _displayInventory = true;
    }
    public void CloseInenvotry()
    {
        Time.timeScale = 1f; // freze the game; or slow motion
        inventory.SetActive(false);
        equipmentPlayer.SetActive(false);
        heroStats.SetActive(false);
        _displayInventory = false;
    }
    public void LoadMenu()
    {
       // SceneManager.LoadScene(0);
      
        StartCoroutine(LoadAsynScene());
    }
    IEnumerator LoadAsynScene()
    {
        Destroy(_gameManger.gameObject);
        Destroy(_canvas.gameObject);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
       
        while (!asyncLoad.isDone)
            yield return null;


    }
    public void LoadInventory()
    {
        Debug.Log("Load Inventory");
        // _inventory.openInventory();

        if (_displayInventory==false)
        {
            OpenInventory();
        }
        else
        {
            CloseInenvotry();
        }
    }
    public void Quit()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
