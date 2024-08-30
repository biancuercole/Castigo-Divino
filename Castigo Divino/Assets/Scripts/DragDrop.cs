using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ItemSlot itemSlot;
    public string message;

    private void Start()
    {
        itemSlot = FindObjectOfType<ItemSlot>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        itemSlot.OnItemClick(gameObject);
        // Aquí puedes mostrar el mensaje de error temporal si es necesario
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipManager.instance.ShowItemToolTip(message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.instance.HideToolTip();
    }
}


