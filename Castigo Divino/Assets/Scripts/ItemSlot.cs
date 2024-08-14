using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler
{

    [SerializeField] private Canvas canvas;
    public GameObject currentItem;
    private PowerUpManager PowerUpManager;
    private Vector2 originalPosition;
    private void Start()
    {
        PowerUpManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpManager>();
        canvas.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // Check if the current slot already has an item
            if (currentItem != null)
            {
                RemovePowerUpEffect(currentItem);
            }

            // Set the new item and apply its power-up effect
            currentItem = eventData.pointerDrag;
            ApplyPowerUpEffect(currentItem);

            // Set the position of the dragged item to the position of this slot
            RectTransform itemRectTransform = currentItem.GetComponent<RectTransform>();
            itemRectTransform.SetParent(transform); // Ensure the item is a child of the slot
            itemRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            itemRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            itemRectTransform.pivot = new Vector2(0.5f, 0.5f);
            itemRectTransform.anchoredPosition = Vector2.zero; // Center it in the slot
        }
    }

    public void RemoveCurrentItem()
    {
        if (currentItem != null)
        {
            RemovePowerUpEffect(currentItem);
            currentItem = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            RemovePowerUpEffect(currentItem);
            currentItem.GetComponent<RectTransform>().SetParent(canvas.transform);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // If the item is dropped outside of any slot, return it to its original position
        if (currentItem != null)
        {
            RectTransform rectTransform = currentItem.GetComponent<RectTransform>();
            if (rectTransform.parent == canvas.transform)
            {
                rectTransform.anchoredPosition = originalPosition;
                rectTransform.SetParent(GameObject.Find("Grid").transform); // Replace with the actual grid object name
                ApplyPowerUpEffect(currentItem); // Apply the effect if it's still in the original slot
            }
        }

    }

    private void ApplyPowerUpEffect(GameObject item)
    {
        PowerUp powerUp = item.GetComponent<PowerUp>();
        if (powerUp != null)
        {
            PowerUpManager.ApplyPowerUp(powerUp.powerUpEffect);
        }
    }

    public void RemovePowerUpEffect(GameObject item)
    {
        PowerUp powerUp = item.GetComponent<PowerUp>();
        if (powerUp != null)
        {
            PowerUpManager.RemovePowerUp(powerUp.powerUpEffect);
        }
    }

    public void Show()
    {
        canvas.gameObject.SetActive(true);
    }
    public void Hide()
    {
        canvas.gameObject.SetActive(false);
    }
}

