using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<inventorySlot_UI, InventorySlot> slotDictionary;
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<inventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem invToDisplay);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) //Slot value -  the "under the hood" inventory slot.
            {
                slot.Key.UpdateUISlot(updatedSlot); //Slot key - the UI representation of the value.
            }
        }
    }

    public void SlotClicked(inventorySlot_UI clickedUISlot)
    {
        

        if (clickedUISlot.AssignedInventorySlot.Itemdata != null && mouseInventoryItem.assignedInventorySlot.Itemdata == null)
        {
            //If player is holding shift key? split stack

            mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
            clickedUISlot.ClearSlot();
            return;
        }

        //Clicked slot doesn't have an item - mouse does have an item - place mouse item into empty slot.
        if(clickedUISlot.AssignedInventorySlot.Itemdata == null && mouseInventoryItem.assignedInventorySlot.Itemdata != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.assignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
        }

        //Both slots have an item - decide what to do....
        //Are both items the same? If so combine them.
        //Is the slot stack size + mouse stack size > the slot Max Size? If so, take from mouse.
        //If different items, then swap the items.
    }
}
