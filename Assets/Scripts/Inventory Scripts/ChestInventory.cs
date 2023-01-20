using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set;}

    

    private void Start()
    {
        var chestSaveData = new InventorySaveData(PrimaryInventorySystem, transform.position, transform.rotation);

        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    protected override void LoadInventory(SaveData data)
    {
        //Check the save data for this specific chests inventory, and if it exists, load it in.
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.primaryInventoryHolder = chestData.invSystem;
            this.transform.position = chestData.position;
            this.transform.rotation = chestData.rotation;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccesfull)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventoryHolder, 0);
        interactSuccesfull = true;
    }

    public void EndInteraction()
    {
        
    }
}


