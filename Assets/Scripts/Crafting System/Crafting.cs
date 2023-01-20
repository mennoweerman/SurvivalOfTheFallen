using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Crafting : MonoBehaviour
{
   /* public CraftingRecipe[] recipes;

    private void Update()
    {
        foreach (CraftingRecipe recipe in recipes)
        {
            bool canCraft = true;
            foreach (Item ingredient in recipe.ingredients)
            {
                if (!PlayerInventory.instance.HasItem(ingredient))
                {
                    canCraft = false;
                    break;
                }
            }

            if (canCraft)
            {
                // Display recipe as a craftable option to the player
                // Add button to UI or use console to print recipe name
                Debug.Log("Can craft " + recipe.result.name);
            }
        }
    }

    public void Craft(CraftingRecipe recipe)
    {
        bool canCraft = true;
        foreach (Item ingredient in recipe.ingredients)
        {
            if (!PlayerInventory.instance.RemoveItem(ingredient))
            {
                canCraft = false;
                break;
            }
        }

        if (canCraft)
        {
            PlayerInventory.instance.AddItem(recipe.result);
            // Update UI
            Debug.Log("Crafted " + recipe.result.name);
        }

        if (!craftingStation.isNear)
        {
            Debug.Log("Player must be near a crafting station to craft this item.");
            return;
        }
        if (!craftingLimit.CheckLimit(1))
        {
            return;
        }
    }*/
}
