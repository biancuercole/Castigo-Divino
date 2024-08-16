using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMachine : MonoBehaviour
{
    public enum BossState { Charge, Attack, TripleAttack, MinionRelease }
    public BossState currentState;
    public MinionsBoss minionBoss;
    public BossHealth bossHealth;

    private NavMeshAgent agent;

    private Animator bossAnimator;
    private Vector2 moveInput;

    public GameObject bulletPrefab;
    public GameObject triplePrefab;
    public Transform bulletSpawnPoint;
    public Transform player;
    public float chargeSpeed = 20f;
    private bool isCharging = false;
    private bool isShooting = false;
    private float shootCooldown = 5f;
    private float nextShootTime = 0f;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        bossHealth = GetComponent<BossHealth>();
        minionBoss = FindObjectOfType<MinionsBoss>();
        agent = GetComponent<NavMeshAgent>();
        currentState = BossState.Charge;
        StateMachine();
        bossAnimator = GetComponent<Animator>();
    }

    public void OnActive()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        Vector3 velocity = agent.velocity;
        Vector2 moveDirection = new Vector2(velocity.x, velocity.z).normalized;

        bossAnimator.SetFloat("Horizontal", moveDirection.x);
        bossAnimator.SetFloat("Vertical", moveDirection.y);
        bossAnimator.SetFloat("Speed", moveDirection.sqrMagnitude);

        switch (currentState)
        {
            case BossState.Attack:
                Attack();
                break;
            case BossState.Charge:
                Charge();
                break;
            case BossState.TripleAttack:
                TripleAttack();
                break;
            case BossState.MinionRelease:
                MinionRelease();
                break;
        }

        // Asegurarse de que el jefe no gire en los ejes X e Y
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = 0f;
        eulerRotation.y = 0f;
        eulerRotation.z = 0f;
        transform.eulerAngles = eulerRotation;
    }

    public void StateMachine()
    {
        currentState = (BossState)(((int)currentState + 1) % System.Enum.GetValues(typeof(BossState)).Length);
    }

    public void MinionRelease()
    {
        minionBoss = FindObjectOfType<MinionsBoss>();
        minionBoss.ActivateMinions();
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

    private void TripleAttack()
    {
        agent.isStopped = true;
        if (Time.time >= nextShootTime && !isShooting)
        {
            StartCoroutine(TripleShoot());
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

        agent.speed = 3.5f; // Vuelve a la velocidad normal después de la embestida
        isCharging = false;
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

    IEnumerator TripleShoot()
    {
        isShooting = true;
            // Disparar 3 balas en diferentes direcciones
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            ShootProyectile(directionToPlayer);
            ShootProyectile(Quaternion.Euler(0, 0, 45) * directionToPlayer); // 45° arriba
            ShootProyectile(Quaternion.Euler(0, 0, -45) * directionToPlayer); // 45° abajo

            yield return new WaitForSeconds(5f);
        isShooting = false;
    }

    void ShootProyectile(Vector2 direction)
    {
        GameObject proyectile = Instantiate(triplePrefab, transform.position, Quaternion.identity);
        TripleBullet proyectileScript = proyectile.GetComponent<TripleBullet>();
        proyectileScript.SetDirection(direction);
    }
}
