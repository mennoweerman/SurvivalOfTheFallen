using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MouseItemData : MonoBehaviour
{
    public Image itemSprite;
    public TextMeshProUGUI itemCount;
    public InventorySlot assignedInventorySlot;

    private Transform playerTransform;
    public float dropOffset = 0.5f;
    private void Awake()
    {
        itemSprite.color = Color.clear;
        itemSprite.preserveAspect = true;
        itemCount.text = "";

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (playerTransform == null)
        {
            Debug.Log("Player not found");
        }
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        assignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }

    public void UpdateMouseSlot()
    {
        itemSprite.sprite = assignedInventorySlot.Itemdata.icon;
        itemCount.text = assignedInventorySlot.StackSize.ToString();
        itemSprite.color = Color.white;
    }

    private void Update()
    {
        if (assignedInventorySlot.Itemdata != null) //If it has an item, follow the mouse position
        {
            transform.position = Mouse.current.position.ReadValue();
            
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                if (assignedInventorySlot.Itemdata.ItemPrefab != null)
                {
                    Instantiate(assignedInventorySlot.Itemdata.ItemPrefab, playerTransform.position + playerTransform.forward * dropOffset, Quaternion.identity);
                }

                if (assignedInventorySlot.StackSize > 1)
                {
                    assignedInventorySlot.AddToStack(-1);
                    UpdateMouseSlot();
                }
                else
                {
                    ClearSlot();
                }
            }
        }
    }

    public void ClearSlot()
    {
        assignedInventorySlot.ClearSlot();
        itemCount.text = "";
        itemSprite.color = Color.clear;
        itemSprite.sprite = null;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
