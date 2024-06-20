using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootAI : MonoBehaviour
{
    [SerializeField] private GameObject proyectilePrefab;
    [SerializeField] private float timeBetweenShoots;
    void Start()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        while (true) 
        {
            yield return new WaitForSeconds (timeBetweenShoots);
            Instantiate (proyectilePrefab, transform.position, Quaternion.identity);

        }
    }
}
