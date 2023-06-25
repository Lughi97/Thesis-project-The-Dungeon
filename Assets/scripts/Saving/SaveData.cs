using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
   public  PlayerData MyPlayerData { get; set; }
    public SaveData()
    {
        // whenever there is a prblem 
    }
}
[Serializable]
public class PlayerData
{
    public int MyPlayerLevel { get; set; }

    public PlayerData (int level)
    {
        this.MyPlayerLevel = level;
    }
}
