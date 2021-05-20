using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStateData
{
    public bool[] finishedLevels;

    public GameStateData(bool[] currentLevels)
    {
        for(int i = 0; i< currentLevels.Length; i++)
        {
            finishedLevels[i] = currentLevels[i];
        }
    }
}
