using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
   
    public static UnityAction OnPlayerInventoryChanged;

    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;

    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(PrimaryInventorySystem);
        SaveLoad.onLoadGame += LoadInventory;
    }

    protected override void LoadInventory(SaveData data)
    {
        //Check the save data for this specific chests inventory, and if it exists, load it in.
        if (data.playerInventory.invSystem != null)
        {
            this.primaryInventoryHolder = data.playerInventory.invSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }

    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame) OnPlayerInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, offset);
    }

    public bool addToInventory(InventoryItemData data, int amount)
    {
        if (PrimaryInventorySystem.addToInventory(data, amount))
        {
            return true;
        }

        return false;
    }
}
