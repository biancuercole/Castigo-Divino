using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMachine : MonoBehaviour
{
    public enum BossState { Patrol, Attack, Charge }
    public BossState currentState;

    public Transform[] waypoints;
    private int currentWaypointIndex;
    private NavMeshAgent agent;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Transform player;
    public float chargeSpeed = 20f;
    private bool isCharging = false;
    private bool isWaiting;
    private bool isShooting = false;
    private float shootCooldown = 5f; // Tiempo entre disparos
    private float nextShootTime = 0f; // Próximo tiempo en el que puede disparar

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = BossState.Patrol;
        currentWaypointIndex = 0;
        StartCoroutine(StateMachine());
        isWaiting = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case BossState.Patrol:
                Patrol();
                break;
            case BossState.Attack:
                Attack();
                break;
            case BossState.Charge:
                Charge();
                break;
        }

        // Asegurarse de que el jefe no gire en los ejes X e Y
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = 0f;
        eulerRotation.y = 0f;
        transform.eulerAngles = eulerRotation;
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            currentState = (BossState)(((int)currentState + 1) % 3);
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0)
            return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Patrullar waypoints
        if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            StartCoroutine(Wait());
        }
    }

    private void Attack()
    {
        agent.isStopped = true;

        // Solo dispara si ha pasado suficiente tiempo desde el último disparo
        if (Time.time >= nextShootTime && !isShooting)
        {
            StartCoroutine(Shoot());
            nextShootTime = Time.time + shootCooldown;
        }
    }

    private void Charge()
    {
        agent.isStopped = false;
        if (!isCharging)
        {
            StartCoroutine(ChargeCoroutine());
        }
    }

    private IEnumerator ChargeCoroutine()
    {
        isCharging = true;
        agent.speed = chargeSpeed;
        agent.destination = player.position;

        while (agent.remainingDistance > 1f)
        {
            yield return null;
        }

        // Aquí puedes implementar la lógica de colisión y daño al jugador
        agent.speed = 3.5f; // Vuelve a la velocidad normal después de la embestida
        isCharging = false;
    }

    private IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1f);
        
        // Solo cambiar al siguiente waypoint si no está siguiendo al jugador
        currentWaypointIndex++;
        if (currentWaypointIndex == waypoints.Length)
        {
            currentWaypointIndex = 0;
        }
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        isWaiting = false;
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        // Verifica que bulletPrefab no sea null
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("bulletPrefab o bulletSpawnPoint no asignado");
        }
            
        yield return new WaitForSeconds(0.5f); // Tiempo de espera para simular la animación del disparo
        isShooting = false;
    }
}
