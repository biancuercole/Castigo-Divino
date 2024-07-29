using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private UIshop uiShop;
    [SerializeField] private ItemSlot item;
  //  [SerializeField] private ItemSlot itemSlot;

    private void Start()
    {
        uiShop.Hide();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Shop"))
            {
                uiShop.Show();
            }
            else
            {
                item.Show();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Shop"))
            {
                uiShop.Hide();
            }
            else
            {
                item.Hide();
            }
        }
    }
}
