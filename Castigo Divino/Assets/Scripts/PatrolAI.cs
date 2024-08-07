using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatroll : MonoBehaviour
{
    [SerializeField] Transform target; 
    private float minDistance = 27.0f; // Distancia mínima para iniciar la persecución
    private float chargeDistance = 3.0f; // Distancia para iniciar la embestida
    private float patrolSpeed = 10.0f; // Velocidad para patrullaje
    private float followSpeed = 12.0f; // Velocidad para persecución
    private float chargeSpeed = 45.0f; // Velocidad para embestida
    [SerializeField] private float time;
    [SerializeField] Transform[] WayPoints;
    [SerializeField] private int currentWaypoint;
    [SerializeField] public int damage;
    
    NavMeshAgent agent;
    private bool isWaiting;
    private bool isFollowing; 
    private bool isCharging = false; // Para controlar si está en embestida

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on " + gameObject.name);
        }

        // Inicializa cualquier otra variable necesaria en Awake
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        isWaiting = false;
        isFollowing = false;
        agent.SetDestination(WayPoints[currentWaypoint].position);
        agent.speed = patrolSpeed; // Establece la velocidad inicial de patrullaje
    }

    void OnEnable()
    {
        // Verifica y configura el agente y los waypoints nuevamente al activarse
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not assigned to " + gameObject.name);
            return;
        }

        // Asegúrate de que haya al menos un waypoint asignado
        if (WayPoints.Length > 0)
        {
            // Configura el destino al waypoint actual
            agent.SetDestination(WayPoints[currentWaypoint].position);
        }
        else
        {
            Debug.LogWarning("No waypoints assigned to " + gameObject.name);
        }

        // Restablecer estados al activarse
        isWaiting = false;
        isFollowing = false;
        isCharging = false;
    }

    void Update()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        
        if (distanceToTarget < chargeDistance)
        {
            if (!isCharging)
            {
                isCharging = true;
                StartCoroutine(Charge());
            }
        }
        else if (distanceToTarget < minDistance)
        {
            isFollowing = true;
            agent.speed = followSpeed; // Velocidad de persecución
            agent.SetDestination(target.position);
        }
        else
        {
            if (isFollowing)
            {
                // Si estaba siguiendo al jugador y ahora se alejó, volver al waypoint
                isFollowing = false;
                agent.speed = patrolSpeed; // Velocidad de patrullaje
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
        if (!isFollowing && !isCharging)
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

    IEnumerator Charge()
    {
        float highSpeedDuration = 3.0f;
        agent.speed = chargeSpeed; // Cambiar a velocidad de embestida
        agent.SetDestination(target.position);

        yield return new WaitForSeconds(highSpeedDuration);

        isCharging = false;
        if (isFollowing)
        {
            agent.speed = followSpeed; // Volver a velocidad de persecución
        }
        else
        {
            agent.speed = patrolSpeed; // Volver a velocidad de patrullaje
        }
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
