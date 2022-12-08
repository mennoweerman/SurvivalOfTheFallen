using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InventoryUIController : MonoBehaviour
{
	[FormerlySerializedAs("chestPanel")] public DynamicInventoryDisplay inventoryPanel;
	public DynamicInventoryDisplay playerBackPackPanel;

	private void Awake()
	{
		inventoryPanel.gameObject.SetActive(false);
		playerBackPackPanel.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
		PlayerInventoryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;
	}

	private void OnDisable()
	{
		InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
    }


	void Update()
	{
		if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			inventoryPanel.gameObject.SetActive(false);
		}

		if (playerBackPackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			playerBackPackPanel.gameObject.SetActive(false);
		}
	}
	void DisplayInventory(InventorySystem invToDisplay, int offset)
	{
		inventoryPanel.gameObject.SetActive(true);
		inventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
	}

    void DisplayPlayerInventory(InventorySystem invToDisplay, int offset)
    {
        playerBackPackPanel.gameObject.SetActive(true);
        playerBackPackPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
}
