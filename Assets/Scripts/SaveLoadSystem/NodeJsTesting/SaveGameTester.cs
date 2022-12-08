using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameTester : MonoBehaviour
{
    public void SaveGame()
    {
        SaveLoadSystem.SaveGameManager.SaveGame();
    }

    public void LoadGame()
    {
        SaveLoadSystem.SaveGameManager.LoadGame();
    }
}
