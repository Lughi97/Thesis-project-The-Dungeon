using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   
    public static SoundManager instance = null; // create only a signle instance of the Game Manager 
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this; // Instantiate the game manager when there is no game Manager;
        else if (instance != this) Destroy(this.gameObject);// if the instance of gamemanager AlreadyExist then Destory GameManager 
        DontDestroyOnLoad(this.gameObject);// this funtcion permits to mantian the Same Game Manager;


    }

    // Update is called once per frame
    void Update()
    {

    }

}
