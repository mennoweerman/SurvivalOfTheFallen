using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveData
{
    public List<string> collectedItems;
    public SerializableDictionary<string, ItemPickUpSaveData> activeItems;

    public SerializableDictionary<string, InventorySaveData> chestDictionary;
    public SerializableDictionary<string, ShopSaveData> shopKeeperDictionary; //Make this a struct so shop can be loaded

    public InventorySaveData playerInventory;
    
    
    
    public SaveData()
    {
        collectedItems = new List<string>();
        activeItems = new SerializableDictionary<string, ItemPickUpSaveData>();
        chestDictionary = new SerializableDictionary<string, InventorySaveData>();
        playerInventory = new InventorySaveData();
        shopKeeperDictionary = new SerializableDictionary<string, ShopSaveData>(); //shopKeeperDictionary where shopsavedata is
        //Debug.Log(ShopSaveData);
    }
}
