using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectable : MonoBehaviour
{
    public int healAmount;
    [SerializeField] private Loot loot;

    /*private void OnTriggerEnter2D(Collider2D other)
    {
       

        Debug.Log("HeartCollectable Triggered");
        if (other.CompareTag("Player") && loot.lootName == "heart")
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Recolectado: " + loot.lootName);
                playerHealth.HealHealth(healAmount);
                Destroy(gameObject);
            }
        }
    }*/
}
