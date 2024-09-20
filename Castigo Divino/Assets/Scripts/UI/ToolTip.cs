using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public string messeage;

    private void OnMouseEnter()
    {
        ToolTipManager.instance.SetAndShowToolTip(messeage);
        Debug.Log("Enter tooltip");
    }

    private void OnMouseExit()
    {
        ToolTipManager.instance.HideToolTip();
        Debug.Log("Exit tooltip");
    }
}
