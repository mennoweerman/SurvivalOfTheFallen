using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> shopInventory;
    [SerializeField] private int availableGold;
    [SerializeField] private float buyMarkUp;
    [SerializeField] private float sellMarkUp;

    public List<ShopSlot> ShopInventory => shopInventory;
    public int AvailableGold => availableGold;
    public float BuyMarkUp => buyMarkUp;
    public float SellMarkUp => sellMarkUp;

    public ShopSystem(int size, int gold, float BuyMarkUp, float SellMarkUp)
    {
        availableGold = gold;
        buyMarkUp = BuyMarkUp;
        sellMarkUp = SellMarkUp;


        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {
        shopInventory = new List<ShopSlot>(size);
    
        for (int i = 0; i < size; i++)
        {
            shopInventory.Add(new ShopSlot());
        }
    }

    public void AddToShop(InventoryItemData data, int amount)
    {
        if (containsItem(data, out ShopSlot shopSlot))
        {
            shopSlot.AddToStack(amount);
            return;
        }

        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);
    }

    private ShopSlot GetFreeSlot()
    {
        var freeSlot = shopInventory.FirstOrDefault(i => i.Itemdata == null);

        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            shopInventory.Add(freeSlot);
        }

        return freeSlot;
    }

    public bool containsItem(InventoryItemData itemToAdd, out ShopSlot shopSlot) //Do any of our slots have the item?
    {
        shopSlot = shopInventory.Find(i => i.Itemdata == itemToAdd);
        return shopSlot != null;
    }

    public void PurchaseItem(InventoryItemData data, int amount)
    {
        if (!containsItem(data, out ShopSlot slot)) return;

        slot.RemoveFromStack(amount);
    }

    public void GainGold(int basketTotal)
    {
        availableGold += basketTotal;
    }

    public void SellItem(InventoryItemData kvpKey, int kvpValue, int price)
    {
        AddToShop(kvpKey, kvpValue);
        ReduceGold(price);
    }

    private void ReduceGold(int price)
    {
        availableGold -= price;
    }
}


