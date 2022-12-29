using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperDisplay : MonoBehaviour
{
    [SerializeField] private ShopSlotUI shopSlotPrefab;
    [SerializeField] private ShoppingCartItemUI shoppingCartItemPrefab;

    [SerializeField] private Button buyTab;
    [SerializeField] private Button sellTab;

    [Header("Shopping Cart")]
    [SerializeField] private TextMeshProUGUI basketTotalText;
    [SerializeField] private TextMeshProUGUI playerGoldText;
    [SerializeField] private TextMeshProUGUI shopGoldText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buyButtonText;

    [Header("Item Preview Section")]
    [SerializeField] private Image itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI itemPreviewName;
    [SerializeField] private TextMeshProUGUI itemPreviewDescription;

    [SerializeField] private GameObject itemListContentPanel;
    [SerializeField] private GameObject shoppingCartContentPanel;

    private int basketTotal;

    private bool isSelling;

    private ShopSystem shopSystem;
    private PlayerInventoryHolder playerInventoryHolder;

    private Dictionary<InventoryItemData, int> shoppingCart = new Dictionary<InventoryItemData, int>();
    private Dictionary<InventoryItemData, ShoppingCartItemUI> shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();

    public void DisplayShopWindow(ShopSystem _shopSystem, PlayerInventoryHolder _playerInventoryHolder)
    {
        shopSystem = _shopSystem;
        playerInventoryHolder = _playerInventoryHolder;

        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        if (buyButton != null)
        {
            buyButtonText.text = isSelling ? "Sell Items" : "Buy Items";
            buyButton.onClick.RemoveAllListeners();
            if (isSelling) buyButton.onClick.AddListener(SellItems);
            else buyButton.onClick.AddListener(BuyItems);
        }

        ClearSlots();
        ClearItemPreview();

        basketTotalText.enabled = false;
        buyButton.gameObject.SetActive(false);
        basketTotal = 0;
        playerGoldText.text = $"Player Gold: {playerInventoryHolder.PrimaryInventorySystem.Gold}";
        shopGoldText.text = $"Shop Gold: {shopSystem.AvailableGold}";

        if (isSelling) DisplayPlayerInventory();
        else DisplayShopInventory();
    }

    private void BuyItems()
    {
        if (playerInventoryHolder.PrimaryInventorySystem.Gold < basketTotal) return;

        if (!playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(shoppingCart)) return;

        foreach (var kvp in shoppingCart)
        {
            shopSystem.PurchaseItem(kvp.Key, kvp.Value);

            for (int i = 0; i < kvp.Value; i++)
            {
                playerInventoryHolder.PrimaryInventorySystem.addToInventory(kvp.Key, 1);
            }
        }

        shopSystem.GainGold(basketTotal);
        playerInventoryHolder.PrimaryInventorySystem.SpendGold(basketTotal);

        RefreshDisplay();
    }

    private void SellItems()
    {
        if (shopSystem.AvailableGold < basketTotal) return;

        foreach (var kvp in shoppingCart)
        {
            var price = GetModifiedPrice(kvp.Key, kvp.Value, shopSystem.SellMarkUp);

            shopSystem.SellItem(kvp.Key, kvp.Value, price);

            playerInventoryHolder.PrimaryInventorySystem.GainGold(price);
            playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(kvp.Key, kvp.Value);
        }

        RefreshDisplay();
    }

    private void ClearSlots()
    {
        shoppingCart = new Dictionary<InventoryItemData, int>();
        shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();

        foreach (var item in itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        foreach (var item in shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    private void DisplayShopInventory()
    {
        foreach (var item in shopSystem.ShopInventory)
        {
            if (item.Itemdata == null) continue;

            var shopSlot = Instantiate(shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.Init(item, shopSystem.BuyMarkUp);
        }
    }

    private void DisplayPlayerInventory()
    {
        foreach (var item in playerInventoryHolder.PrimaryInventorySystem.GetAllItemsHeld())
        {
            var tempSlot = new ShopSlot();
            tempSlot.AssignItem(item.Key, item.Value);

            var shopSlot = Instantiate(shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.Init(tempSlot, shopSystem.SellMarkUp);
        }
    }

    public void RemoveItemFromCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.Itemdata;
        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);

        if (shoppingCart.ContainsKey(data))
        {
            shoppingCart[data]--;
            var newString = $"{data.displayName} ({price}G) x{shoppingCart[data]}";
            shoppingCartUI[data].SetItemText(newString);

            if (shoppingCart[data] <= 0)
            {
                shoppingCart.Remove(data);
                var tempObj = shoppingCartUI[data].gameObject;
                shoppingCartUI.Remove(data);
                Destroy(tempObj);
            }
        }

        basketTotal -= price;
        basketTotalText.text = $"Total: {basketTotal}G";

        if (basketTotal <= 0 && basketTotalText.IsActive())
        {
            basketTotalText.enabled = false;
            buyButton.gameObject.SetActive(false);
            ClearItemPreview();
            return;
        }

        CheckCartVsAvailableGold();
    }

    private void ClearItemPreview()
    {
        itemPreviewSprite.sprite = null;
        itemPreviewSprite.color = Color.clear;
        itemPreviewName.text = "";
        itemPreviewDescription.text = "";
    }

    public void AddItemToCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.Itemdata;

        UpdateItemPreview(shopSlotUI);
            
        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);

        
        if (shoppingCart.ContainsKey(data))
        {
            shoppingCart[data]++;
            var newString = $"{data.displayName} ({price}G) x{shoppingCart[data]}";
            shoppingCartUI[data].SetItemText(newString);
        }
        else
        {
            shoppingCart.Add(data, 1);

            var shoppingCartTextObj = Instantiate(shoppingCartItemPrefab, shoppingCartContentPanel.transform);
            var newString = $"{data.displayName} ({price}G) x1";
            shoppingCartTextObj.SetItemText(newString);
            shoppingCartUI.Add(data, shoppingCartTextObj);
        }
        
        basketTotal += price;
        basketTotalText.text = $"Total: {basketTotal}G";

        if (basketTotal > 0 && !basketTotalText.IsActive())
        {
            basketTotalText.enabled = true;
            buyButton.gameObject.SetActive(true);
        }

        CheckCartVsAvailableGold();
    }

    private void CheckCartVsAvailableGold()
    {
        var goldToCheck = isSelling ? shopSystem.AvailableGold : playerInventoryHolder.PrimaryInventorySystem.Gold;
        basketTotalText.color = basketTotal > goldToCheck ? Color.red : Color.white;

        if (isSelling || playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(shoppingCart)) return;

        basketTotalText.text = "Not enough room in inventory!";
        basketTotalText.color = Color.red;
    }

    public static int GetModifiedPrice(InventoryItemData data, int amount, float markUp)
    {
        var baseValue = data.goldValue * amount;

        return Mathf.FloorToInt(baseValue + baseValue * markUp);
    }

    private void UpdateItemPreview(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.Itemdata;

        itemPreviewSprite.sprite = data.icon;
        itemPreviewSprite.color = Color.white;
        itemPreviewName.text = data.displayName;
        itemPreviewDescription.text = data.description;
    }

    public void OnBuyTabPressed()
    {
        isSelling = false;
        RefreshDisplay();
    }

    public void OnSellTabPressed()
    {
        isSelling = true;
        RefreshDisplay();
    }

    
}
