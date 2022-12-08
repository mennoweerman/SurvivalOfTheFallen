using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSaveData : MonoBehaviour
{
    public int currentHealth;
    private PlayerData myData = new PlayerData();

    // Update is called once per frame
    void Update()
    {
        myData.playerPosition = transform.position;
        myData.playerRotation = transform.rotation;
        myData.currentHealth = currentHealth;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SaveLoadSystem.SaveGameManager.currentSaveData.playerData = myData;
            SaveLoadSystem.SaveGameManager.SaveGame();
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            SaveLoadSystem.SaveGameManager.LoadGame();
            myData = SaveLoadSystem.SaveGameManager.currentSaveData.playerData;
            transform.position = myData.playerPosition;
            transform.rotation = myData.playerRotation;
            currentHealth = myData.currentHealth;
        }
    }
}

[System.Serializable]
public struct PlayerData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public int currentHealth;
}
