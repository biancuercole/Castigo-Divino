using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private ShootAI shootAI;
    private EnemyPatroll enemyPatroll;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        shootAI = GetComponent<ShootAI>();
        enemyPatroll = GetComponent<EnemyPatroll>();

        DeactivateEnemy();
    }

    void Update()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < 30f) // Example activation distance
        {
            ActivateEnemy();
        }
        else
        {
            DeactivateEnemy();
        }
    }

    void ActivateEnemy()
    {
        if (enemyHealth != null) enemyHealth.enabled = true;
        if (shootAI != null) shootAI.enabled = true;
        if (enemyPatroll != null) enemyPatroll.enabled = true;
    }

    void DeactivateEnemy()
    {
        if (enemyHealth != null) enemyHealth.enabled = false;
        if (shootAI != null) shootAI.enabled = false;
        if (enemyPatroll != null) enemyPatroll.enabled = false;
    }

    public void OnDeath()
    {
        EnemyPool.Instance.ReturnEnemy(gameObject);
    }
}
