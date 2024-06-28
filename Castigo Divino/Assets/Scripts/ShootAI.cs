using System.Collections;
using UnityEngine;

public class ShootAI : MonoBehaviour
{
    [SerializeField] private GameObject proyectilePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float shootDistance = 10f;

    private bool isShooting;
    private bool inRange;

    void Update()
    {
        float distanceToTarget = Vector2.Distance(transform.position, player.position);
        if (distanceToTarget < shootDistance)
        {
            if (!isShooting)
            {
                isShooting = true;
                inRange = true;
                StartCoroutine(Shoot());
            }
        }
        else
        {
            inRange = false;
        }
    }

    IEnumerator Shoot()
    {
        while (inRange)
        {
            Debug.Log("Shooting at target...");
            Instantiate(proyectilePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
        isShooting = false;
    }
}
