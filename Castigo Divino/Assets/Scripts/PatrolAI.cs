using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    [SerializeField] private Transform[] WayPoints;
    [SerializeField] private float speed;
    [SerializeField] private float time; 
    [SerializeField] private Transform player;
    [SerializeField] private float minDistance;
    [SerializeField] public float damage;

    private bool followPlayer = false; 
    private bool isWaiting;
    private int currentWaypoint;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < minDistance)
        {
            followPlayer = true; 
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }else
        {
            followPlayer = false;
        }

        if(transform.position != WayPoints[currentWaypoint].position && !followPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, WayPoints[currentWaypoint].position, speed * Time.deltaTime);
        }
        else if (!isWaiting)
        {
            StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            isWaiting = true;
            yield return new WaitForSeconds(time);
            currentWaypoint++;
            if(currentWaypoint == WayPoints.Length)
            {
                currentWaypoint = 0;
            }
            isWaiting = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            StartCoroutine(playerHealth.GetDamage(damage));
        }
    }
}
