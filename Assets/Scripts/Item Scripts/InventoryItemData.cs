using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int id = -1;
    public string displayName;
    public Sprite icon;
    [TextArea(4, 4)]
    public string description;
    public int maxStackSize;
    public int goldValue;
    public GameObject ItemPrefab;
}
