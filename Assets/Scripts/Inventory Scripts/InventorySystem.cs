using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Drawing;
using System;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private int gold;

    public int Gold => gold;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size) //Constructor that sets the amount of slots
    {
        gold = 0;
        CreateInventory(size);

    }

    public InventorySystem(int _size, int _gold)
    {
        gold = _gold;
        CreateInventory(_size);
    }

    private void CreateInventory(int _size)
    {
        inventorySlots = new List<InventorySlot>(_size);

        for (int i = 0; i < _size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool addToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (containsItem(itemToAdd, out List<InventorySlot> invSlot)) //Checks if item exists in inventory
        {
            foreach (var slot in invSlot)
            {
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
            
        }

        if(hasFreeSlot(out InventorySlot freeSlot)) //Gets the first available slot
        {
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                freeSlot.updateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
            //Add implementation to only take what can fill the stack, and check for another free slot to put the remainder in
        }

        return false;
    }

    public bool containsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot) //Do any of our slots have the item?
    {
        invSlot = inventorySlots.Where(i => i.Itemdata == itemToAdd).ToList(); //If they do, get a list of all of them
        return invSlot == null  ? false : true; //If they do return true, else return false
    }

    public bool hasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.Itemdata == null); //Get the first free slot
        return freeSlot == null ? false : true;
    }

    public bool CheckInventoryRemaining(Dictionary<InventoryItemData, int> shoppingCart)
    {
        var clonedSystem = new InventorySystem(this.InventorySize);

        for (int i = 0; i < InventorySize; i++)
        {
            clonedSystem.InventorySlots[i].AssignItem(this.InventorySlots[i].Itemdata, this.InventorySlots[i].StackSize);

        }

        foreach (var kvp in shoppingCart) //kvp = key value pair
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                if (!clonedSystem.addToInventory(kvp.Key, 1)) return false;
            }
        }

        return true;
    }

    public void SpendGold(int basketTotal)
    {
        gold -= basketTotal;
    }

    public Dictionary<InventoryItemData, int> GetAllItemsHeld()
    {
        var distinctItems = new Dictionary<InventoryItemData, int>();

        foreach (var item in inventorySlots)
        {
            if (item.Itemdata == null) continue;

            if (!distinctItems.ContainsKey(item.Itemdata)) distinctItems.Add(item.Itemdata, item.StackSize);
            else distinctItems[item.Itemdata] += item.StackSize;
        }

        return distinctItems;
    }

    public void GainGold(int price)
    {
        gold += price;
    }

    public void RemoveItemsFromInventory(InventoryItemData data, int amount)
    {
        if (containsItem(data, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot)
            {
                var stackSize = slot.StackSize;
                
                if (stackSize > amount) slot.RemoveFromStack(amount);
                else
                {
                    slot.RemoveFromStack(stackSize);
                    amount -= stackSize;
                }

                OnInventorySlotChanged?.Invoke(slot);
            }
        }
    }
}
