using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GridLayoutGroup gridLayerGroup; // Agrega una referencia al GridLayerGroup
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private PowerUpManager playerPowerUpManager;
    private Vector2 originalPosition;
    public string messeage;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        playerPowerUpManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpManager>();
        // canvas.gameObject.SetActive(false);
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Verifica si el ítem está en el grid (fuera de cualquier ItemSlot)
        if (IsInGrid())
        {
            RemovePowerUpEffect();
            rectTransform.SetParent(transform);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ToolTipManager.instance.SetAndShowToolTip(messeage);
        Debug.Log("OnPointerDown");
    }

    private bool IsInGrid()
    {
        RectTransform gridRectTransform = gridLayerGroup.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(gridRectTransform, rectTransform.position, null);
    }

    private void RemovePowerUpEffect()
    {
        PowerUp powerUp = GetComponent<PowerUp>();
        if (powerUp != null)
        {
            playerPowerUpManager.RemovePowerUp(powerUp.powerUpEffect);
        }
    }
}
