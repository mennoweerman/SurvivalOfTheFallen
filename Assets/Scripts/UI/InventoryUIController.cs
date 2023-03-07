using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class InventoryUIController : MonoBehaviour
{
	[FormerlySerializedAs("chestPanel")] public DynamicInventoryDisplay inventoryPanel;
	public DynamicInventoryDisplay playerBackPackPanel;
    public PlayerCam _Playercam;


    //public PlayerCam cam; //Need to make when bool is true the cam doesnt follow the mouse

    private void Awake()
	{
		inventoryPanel.gameObject.SetActive(false);
		playerBackPackPanel.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
		PlayerInventoryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;
        //EventManager.OnItemAdded += UpdateUI;
        //EventManager.OnItemRemoved += UpdateUI;
    }

	private void OnDisable()
	{
		InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
        //EventManager.OnItemAdded -= UpdateUI;
        //EventManager.OnItemRemoved -= UpdateUI;
    }


	void Update()
	{
		if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.oKey.wasPressedThisFrame)
		{
            _Playercam.inMenu = false;
            inventoryPanel.gameObject.SetActive(false);
		}

		if (playerBackPackPanel.gameObject.activeInHierarchy && Keyboard.current.oKey.wasPressedThisFrame)
		{
            _Playercam.inMenu = false;
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
