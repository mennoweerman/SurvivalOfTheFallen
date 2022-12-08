using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem primaryInventoryHolder;
    [SerializeField] protected int offset = 10;

    public int Offset => offset;

    public InventorySystem PrimaryInventorySystem => primaryInventoryHolder;

    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested; //Inv System To Display, amount to offset display by.

    protected virtual void Awake()
    {
        SaveLoad.onLoadGame += LoadInventory;

        primaryInventoryHolder = new InventorySystem(inventorySize);
    }

    protected abstract void LoadInventory(SaveData saveData);
}

[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem invSystem;
    public Vector3 position;
    public Quaternion rotation;

    public InventorySaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation)
    {
        invSystem = _invSystem;
        position = _position;
        rotation = _rotation;
    }

    public InventorySaveData(InventorySystem _invSystem)
    {
        invSystem = _invSystem;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }
}
