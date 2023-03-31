using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int jumps;
    public Player player;

    public GameData()
    {
        jumps = 50;
    }
}
