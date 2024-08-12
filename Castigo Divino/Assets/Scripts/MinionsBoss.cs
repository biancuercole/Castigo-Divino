using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsBoss : MonoBehaviour
{
    public void DeactivateMinion()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void ActivateMinions()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
