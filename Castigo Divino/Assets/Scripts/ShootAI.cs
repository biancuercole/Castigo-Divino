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
    private Animator fireAnimator;
    NavMeshAgent agent;
    private bool isShooting; //disparo
    private bool inRange; //disparo
    private bool isPatrolling; //patrullaje waypoints
    private bool isWaiting; //espera waypoints

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }
    void Start()
    {
        fireAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        isWaiting = false;
        agent.SetDestination(WayPoints[currentWaypoint].position); //va al waypoint 1
    }

    void OnEnable()
    {
        if (agent == null || WayPoints == null || player == null)
        {
            Debug.LogError("One or more required components or references are not assigned.");
            return;
        }

        // Restablecer estados y configuraciones al activarse
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

        if (fireAnimator != null)
        {
            fireAnimator.SetFloat("Horizontal", agent.velocity.x);
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
           // Debug.Log("Shooting at target...");
           
          
            Instantiate(proyectilePrefab, transform.position, Quaternion.identity);
           
            /*GameObject proyectile = ProyectilePool.Instance.RequestProyectile();
            proyectile.transform.position = transform.position;
            proyectile.transform.rotation = Quaternion.identity;
            proyectile.GetComponent<Proyectiles>().LaunchProyectile();*/
            
            yield return new WaitForSeconds(1.5f);
        }
        isShooting = false;
    }
}
