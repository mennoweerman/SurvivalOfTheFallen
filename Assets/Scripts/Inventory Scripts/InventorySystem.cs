using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size) //Constructor that sets the amount of slots
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
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
}
