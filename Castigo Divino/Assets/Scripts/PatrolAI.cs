using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatroll : MonoBehaviour
{
    [SerializeField] Transform target; 
    [SerializeField] private float minDistance; 
    [SerializeField] private float speed; //velocidad para persecución
    [SerializeField] private float time;
    [SerializeField] Transform[] WayPoints;
    [SerializeField] private int currentWaypoint;
    [SerializeField] public int damage;

    NavMeshAgent agent;
    private bool isWaiting;
    private bool isFollowing; 

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        isWaiting = false;
        isFollowing = false;
        agent.SetDestination(WayPoints[currentWaypoint].position);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        
        if (distanceToTarget < minDistance)
        {
            isFollowing = true;
            agent.SetDestination(target.position);
        }
        else
        {
            if (isFollowing)
            {
                // Si estaba siguiendo al jugador y ahora se alejó, volver al waypoint
                isFollowing = false;
                agent.SetDestination(WayPoints[currentWaypoint].position);
            }

            // Patrullar waypoints
            if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                StartCoroutine(Wait());
            }
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
        yield return new WaitForSeconds(time);
        
        // Solo cambiar al siguiente waypoint si no está siguiendo al jugador
        if (!isFollowing)
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
          playerHealth.GetDamage(damage);
        }
    }
}



