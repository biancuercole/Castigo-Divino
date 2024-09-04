using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    private EnemyLevel enemyLevel;
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private HealthBar healthBar;
    private float health;
    private SpriteRenderer spriteRenderer;
    private NextStage nextStage;
    private bool isDead = false; // Flag para controlar si el enemigo está muerto

    NavMeshAgent agent;
    AudioManager audioManager;
    private Animator animator; // Referencia al Animator
    private Collider2D enemyCollider; // Referencia al Collider
    public GameObject damageParticle;
    public GameObject explosionPaticle;
    
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        enemyLevel = FindObjectOfType<EnemyLevel>();
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
            Color customColor;
            ColorUtility.TryParseHtmlString("#FFFFF", out customColor);
            Instantiate(damageParticle, transform.position, Quaternion.identity);
            float damageDuration = 0.5f;
            spriteRenderer.color = customColor;
            yield return new WaitForSeconds(damageDuration);
            spriteRenderer.color = Color.white;
        }
        if (health <= 0 && !isDead)
        {
            CameraMovement.Instance.MoveCamera(5, 5, 1f);
            Instantiate(explosionPaticle, transform.position, Quaternion.identity);
            isDead = true; // Marcar al enemigo como muerto para evitar que se procese varias veces
            agent.isStopped = true;
            audioManager.playSound(audioManager.enemyDeath);
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            GameEvents.EnemyDefeated(); // Llama al método de NextStage cuando el enemigo sea derrotado
            if (SceneManager.GetActiveScene().name == "EnemyLevel")
            {
                enemyLevel.EnemyDefeated();
            }

            // Desactivar el Collider del enemigo
            enemyCollider.enabled = false;

            // Reproduce la animación de explosión
            /*if (!gameObject.CompareTag("Humo"))
            {
                animator.SetTrigger("Explode");
            }*/
            // Esperar a que la animación de explosión termine
           // yield return new WaitForSeconds(1.0f); // Ajusta el tiempo según la duración de la animación

            Destroy(gameObject);
            healthBar.HideBar();
        }
    }
}