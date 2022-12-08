using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<inventorySlot_UI, InventorySlot> slotDictionary; //Pair up the UI slots with the system slots
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<inventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem invToDisplay, int offset); //Implemented in child classes

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) //Slot value -  the "under the hood" inventory slot
            {
                slot.Key.UpdateUISlot(updatedSlot); //Slot key - the UI representation of the value
            }
        }
    }

    public void SlotClicked(inventorySlot_UI clickedUISlot)
    {
        bool isCapslockPressed = Keyboard.current.capsLockKey.isPressed;

        //Does the clicked slot have item data - Does the mouse have no item data?
        if (clickedUISlot.AssignedInventorySlot.Itemdata != null && mouseInventoryItem.assignedInventorySlot.Itemdata == null)
        {
            //If player is holding capslock key? split stack
            if (isCapslockPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot)) //Split stack
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else //Pick up the item in the clicked slot
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
            
        }

        //Clicked slot doesn't have an item - mouse does have an item - place mouse item into empty slot.
        if(clickedUISlot.AssignedInventorySlot.Itemdata == null && mouseInventoryItem.assignedInventorySlot.Itemdata != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.assignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }


        //Are both items the same? If so combine them.
        //Is the slot stack size + mouse stack size > the slot Max Size? If so, take from mouse.
        //If different items, then swap the items.

        //Both slots have an item - decide what to do....
        if (clickedUISlot.AssignedInventorySlot.Itemdata != null && mouseInventoryItem.assignedInventorySlot.Itemdata != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.Itemdata == mouseInventoryItem.assignedInventorySlot.Itemdata;

            //Are both items the same? If so combine them.
            if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.assignedInventorySlot.StackSize))
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.assignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
                return;
            }
            else if(isSameItem && !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.assignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1) SwapSlots(clickedUISlot); //Stack is full so swap the items.
                else //Slot is not at max, so take what's needed from the mouse inventory.
                {
                    int remainingOnMouse = mouseInventoryItem.assignedInventorySlot.StackSize - leftInStack;
                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    var newItem = new InventorySlot(mouseInventoryItem.assignedInventorySlot.Itemdata, remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }
    private void SwapSlots(inventorySlot_UI clickedUISlot)
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.assignedInventorySlot.Itemdata, mouseInventoryItem.assignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();

        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

        clickedUISlot.ClearSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
