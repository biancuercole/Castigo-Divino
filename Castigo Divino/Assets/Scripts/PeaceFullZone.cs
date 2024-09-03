using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceFullZone : MonoBehaviour
{
    [SerializeField] private GameObject itemToActivate;
    [SerializeField] private GameObject itemToActivate2;
    [SerializeField] private GameObject itemToActivate3;
    void Start()
    {
        // Verificamos si el objeto fue recogido
        if (/*ManagerData.Instance != null &&*/ ManagerData.Instance.isBulletPowerUpCollected)
        {
            itemToActivate.SetActive(true); // Activamos el objeto
        }

        if (ManagerData.Instance.isSpeedBulletBought)
        {
            itemToActivate2.SetActive(true);
        }

        if (ManagerData.Instance.isDamageBulletBought)
        {
            itemToActivate3.SetActive(true);
        }
    }
}