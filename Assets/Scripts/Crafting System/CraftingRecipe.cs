using static UnityEditor.Progress;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<InventoryItemData> ingredients;
    public InventoryItemData result;

    public bool CanCraft(InventorySystem inventory)
    {
        // check if the player has all of the required ingredients
        foreach (var ingredient in ingredients)
        {
            List<InventorySlot> slots;
            if (!inventory.containsItem(ingredient, out slots))
            {
                return false;
            }
        }
        return true;
    }

    public void Craft(InventorySystem inventory)
    {
        // check if the player has the required ingredients again
        if (!CanCraft(inventory))
        {
            Debug.Log("Missing ingredients");
            return;
        }


        // deduct ingredients from the player's inventory
        foreach (var ingredient in ingredients)
        {
            inventory.RemoveItemsFromInventory(ingredient, 1);
        }

        // instantiate the resulting item and add it to the player's inventory
        var resultItem = Instantiate(result);
        inventory.addToInventory(resultItem, 1);
    }
}