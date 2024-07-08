using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] private float amountPoints;
    [SerializeField] private PointsUI pointsUI;
    [SerializeField] private Loot loot;

    void Start()
    {
        pointsUI = FindObjectOfType<PointsUI>();
        if (pointsUI == null)
        {
            Debug.LogError("No se encontr� un componente PointsUI en la escena.");
        }
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
       
        Debug.Log("Points Triggered");
        if (other.CompareTag("Player") && loot.lootName == "coin")
        {
            Debug.Log("Recolectado: " + loot.lootName);
            pointsUI.takePoints(amountPoints);
            Destroy(gameObject);
        }
    }*/
}
