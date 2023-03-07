using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Recipe
{
    public string name;
    public Item resultItem;
    public RecipeItem[] requiredItems;
}