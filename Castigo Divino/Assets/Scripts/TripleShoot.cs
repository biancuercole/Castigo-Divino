using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TripleShoot : MonoBehaviour
{
    [SerializeField] private GameObject proyectilePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float shootDistance = 10f;
    [SerializeField] private Transform[] WayPoints;
    [SerializeField] private int currentWaypoint;
    [SerializeField] private float waitTime;

    NavMeshAgent agent;
    private bool isShooting;
    private bool inRange;
    private bool isPatrolling;
    private bool isWaiting;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        isWaiting = false;
        agent.SetDestination(WayPoints[currentWaypoint].position);
    }

    void OnEnable()
    {
        if (agent == null || WayPoints == null || player == null)
        {
            Debug.LogError("One or more required components or references are not assigned.");
            return;
        }

        agent.SetDestination(WayPoints[currentWaypoint].position);
        isShooting = false;
        inRange = false;
        isWaiting = false;
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
                StartCoroutine(Shoot());
            }
        }
        else
        {
            if (inRange)
            {
                inRange = false;
                isShooting = false;
                agent.isStopped = false;
                agent.SetDestination(WayPoints[currentWaypoint].position);
            }
        }

        if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            StartCoroutine(Wait());
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

            // Disparar 3 balas en diferentes direcciones
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            ShootProyectile(directionToPlayer);
            ShootProyectile(Quaternion.Euler(0, 0, 45) * directionToPlayer); // 45° arriba
            ShootProyectile(Quaternion.Euler(0, 0, -45) * directionToPlayer); // 45° abajo

            yield return new WaitForSeconds(3f);
        }
        isShooting = false;
    }

    void ShootProyectile(Vector2 direction)
    {
        GameObject proyectile = Instantiate(proyectilePrefab, transform.position, Quaternion.identity);
        TripleBullet proyectileScript = proyectile.GetComponent<TripleBullet>();
        proyectileScript.SetDirection(direction);
    }
}
