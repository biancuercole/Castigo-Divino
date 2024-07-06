using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; 
    private float health;
    private SpriteRenderer spriteRenderer;
    private NextStage nextStage;
    private bool isDead = false; // Flag para controlar si el enemigo está muerto

    NavMeshAgent agent;
    AudioManager audioManager;
    private Animator animator; // Referencia al Animator
    private Collider2D enemyCollider; // Referencia al Collider

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextStage = FindObjectOfType<NextStage>(); // Encuentra el objeto con el script NextStage
        animator = GetComponent<Animator>(); // Obtén el componente Animator
        enemyCollider = GetComponent<Collider2D>(); // Obtén el componente Collider
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(GetDamage(damage));
    }

    private IEnumerator GetDamage(float damage)
    {
        health -= damage;
        if (health > 0)
        {
            float damageDuration = 1f;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(damageDuration);
            spriteRenderer.color = Color.white;
        }
        if (health <= 0 && !isDead)
        {
            isDead = true; // Marcar al enemigo como muerto para evitar que se procese varias veces

            agent.isStopped = true;
            audioManager.playSound(audioManager.enemyDeath);
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            nextStage.EnemyDefeated(); // Llama al método de NextStage cuando el enemigo sea derrotado

            // Desactivar el Collider del enemigo
            enemyCollider.enabled = false;

            // Reproduce la animación de explosión
            animator.SetTrigger("Explode");

            // Esperar a que la animación de explosión termine
            yield return new WaitForSeconds(1.0f); // Ajusta el tiempo según la duración de la animación

            Destroy(gameObject);
        }
    }
}