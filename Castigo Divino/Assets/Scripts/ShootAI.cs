using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ShootAI : MonoBehaviour
{
    [SerializeField] private GameObject proyectilePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float shootDistance = 10f;
    [SerializeField] private Transform[] WayPoints;
    [SerializeField] private int currentWaypoint;
    [SerializeField] private float waitTime;

    NavMeshAgent agent;
    private bool isShooting; //disparo
    private bool inRange; //disparo
    private bool isPatrolling; //patrullaje waypoints
    private bool isWaiting; //espera waypoints

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        isWaiting = false;
        agent.SetDestination(WayPoints[currentWaypoint].position); //va al waypoint 1
    } 

    void Update()
    {
        float distanceToTarget = Vector2.Distance(transform.position, player.position);
        if (distanceToTarget < shootDistance)
        {
            if (!isShooting)
            {
                isShooting = true;
                inRange = true;
                agent.isStopped = true;
                StartCoroutine(Shoot()); //cuando el jugador se acerca, dispara
            }
        } else
        {
            if(inRange)
            {
                inRange = false;
                isShooting = false;
                agent.isStopped = false;
                agent.SetDestination(WayPoints[currentWaypoint].position);
            }
        }

        if(!isWaiting && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            StartCoroutine(Wait()); //cuando está en un waypoint
        }

        if (agent.velocity.x < 0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (agent.velocity.x > -0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        
        // Solo cambiar al siguiente waypoint si no está siguiendo al jugador
        if (!isShooting)
        {
            currentWaypoint++;
            if (currentWaypoint == WayPoints.Length)
            {
                currentWaypoint = 0;
            }
            agent.SetDestination(WayPoints[currentWaypoint].position);
        }

        isWaiting = false;
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
