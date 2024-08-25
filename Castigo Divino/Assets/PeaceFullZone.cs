using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceFullZone : MonoBehaviour
{
    [SerializeField] private GameObject itemToActivate;

    void Start()
    {
        // Verificamos si el objeto fue recogido
        if (/*ManagerData.Instance != null &&*/ ManagerData.Instance.isBulletPowerUpCollected)
        {
            itemToActivate.SetActive(true); // Activamos el objeto
        }

    }
}