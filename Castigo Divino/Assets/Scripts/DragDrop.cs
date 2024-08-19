using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerClickHandler
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
        ToolTipManager.instance.SetAndShowToolTip(message);
    }
}





