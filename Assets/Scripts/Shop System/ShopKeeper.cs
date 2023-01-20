using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList shopItemsHeld;
    [SerializeField] private ShopSystem shopSystem;

    private ShopSaveData shopSaveData;

    public ShopSaveData ShopSaveData
    {
        get { return shopSaveData; }
    }

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    private string id;

    private void Awake()
    {
        shopSystem = new ShopSystem(shopItemsHeld.Items.Count, shopItemsHeld.MaxAllowedGold, shopItemsHeld.BuyMarkUp, shopItemsHeld.SellMarkUP);

        foreach (var item in shopItemsHeld.Items)
        {
            shopSystem.AddToShop(item.itemData, item.amount);
        }

        id = GetComponent<UniqueID>().ID;
        shopSaveData = new ShopSaveData(shopSystem);
    }

    private void Start()
    {
        if (SaveGameManager.data.shopKeeperDictionary.ContainsKey(id)) SaveGameManager.data.shopKeeperDictionary.Add(id, shopSaveData);
    }

    private void OnEnable()
    {
        SaveLoad.onLoadGame += LoadInventory;
    }

    private void LoadInventory(SaveData data)
    {
        ShopSaveData _shopSaveData;
        
        if (data.shopKeeperDictionary.TryGetValue(id, out _shopSaveData)) return;
        Debug.Log(_shopSaveData);
        shopSystem = shopSaveData.ShopSystem;
        shopSaveData = _shopSaveData;
        
        
    }

    private void OnDisable()
    {
        SaveLoad.onLoadGame -= LoadInventory;
    }

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public void Interact(Interactor interactor, out bool interactSuccesfull)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null)
        {
            OnShopWindowRequested?.Invoke(shopSystem, playerInv);
            interactSuccesfull = true;
        }
        else
        {
            interactSuccesfull = false;
            Debug.LogError("Player inventory not found");
        }
    }

    public void EndInteraction()
    {
        
    }
}

[System.Serializable]
public class ShopSaveData
{
    public ShopSystem ShopSystem;

    public ShopSaveData(ShopSystem shopSystem)
    {
        ShopSystem = shopSystem;

    }
}
