using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private ShopSlot assignedItemSlot;

    public ShopSlot AssignedItemSlot => assignedItemSlot;

    [SerializeField] private Button addItemToCartButton;
    [SerializeField] private Button removeItemFromCartButton;

    private int tempAmount;

    public ShopKeeperDisplay ParentDisplay { get; private set; }
    public float MarkUp { get; private set; }

    private void Awake()
    {
        itemSprite.sprite = null;
        itemSprite.preserveAspect = true;
        itemSprite.color = Color.clear;
        itemName.text = "";
        itemCount.text = "";

        addItemToCartButton?.onClick.AddListener(AddItemToCart);
        removeItemFromCartButton?.onClick.AddListener(RemoveItemFromCart);
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    public void Init(ShopSlot slot, float markUp)
    {
        assignedItemSlot = slot;
        MarkUp = markUp;
        tempAmount = slot.StackSize;
        UpdateUISlot();
    }

    private void UpdateUISlot()
    {
        if (assignedItemSlot.Itemdata != null)
        {
            itemSprite.sprite = assignedItemSlot.Itemdata.icon;
            itemSprite.color = Color.white;
            itemCount.text = $"x{assignedItemSlot.StackSize.ToString()}";
            var modifiedPrice = ShopKeeperDisplay.GetModifiedPrice(assignedItemSlot.Itemdata, 1, MarkUp);

            itemName.text = $"{assignedItemSlot.Itemdata.displayName} - {modifiedPrice}G";
        }
        else
        {
            itemSprite.sprite = null;
            itemSprite.color = Color.clear;
            itemName.text = "";
            itemCount.text = "";
        }
    }

    private void AddItemToCart()
    {
        if (tempAmount <= 0) return;
        
        tempAmount--;
        ParentDisplay.AddItemToCart(this);
        itemCount.text = $"x{tempAmount.ToString()}";
        
    }

    private void RemoveItemFromCart()
    {
        if (tempAmount == assignedItemSlot.StackSize) return;

        tempAmount++;
        ParentDisplay.RemoveItemFromCart(this);
        itemCount.text = $"x{tempAmount.ToString()}";
    }

}
