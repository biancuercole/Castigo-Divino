using System.Collections;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public abstract class BaseEnemy : MonoBehaviour, IDamageType
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject damageParticle;
    [SerializeField] private GameObject explosionParticle;
    public float health;
    private bool isDead = false;
    private AudioManager audioManager;
    private Collider2D enemyCollider;
    private EnemyLevel enemyLevel;
    [SerializeField] private Portals portal;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int indiceNivel;
    private Coroutine damageCoroutine;
    public BossMachine bossMachine;
    private Animator animator;
    [SerializeField] private MinionsBoss minionsBoss;
    private PlayerMovement playerMovement;
    private ManagerData managerData;

    private float lastHealthThreshold; // Nueva variable

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        health = maxHealth;
        audioManager = FindObjectOfType<AudioManager>();
        enemyCollider = GetComponent<Collider2D>();
        enemyLevel = FindObjectOfType<EnemyLevel>();
        managerData = FindObjectOfType<ManagerData>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        bossMachine = GetComponent<BossMachine>();
        health = maxHealth;
        //healthBar.UpdateHealthBar(maxHealth, health);
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastHealthThreshold = maxHealth; // Inicializar con la salud m�xima
    }

    public virtual void TakeDamage(float damage, BulletType bulletType)
    {
        StartCoroutine(GetDamage(damage));
    }

    public virtual void TakeDamageBoss(float damage, BulletType bulletType)
    {
        StartCoroutine(GetDamageBoss(damage));
    }

    private IEnumerator GetDamage(float damage)
    {
        health -= damage;

        if (health > 0)
        {
            Instantiate(damageParticle, transform.position, Quaternion.identity);
            animator.SetBool("Damage", true);
            yield return new WaitForSeconds(1.0f);
            animator.SetBool("Damage", false);

        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            HandleDeath();
        }
    }

    protected virtual void HandleDeath()
    {
        StartCoroutine(deathAnimation());
    }

    private IEnumerator deathAnimation()
    {
        animator.SetTrigger("Explode");
        yield return new WaitForSecondsRealtime(0.7f);
        deathEnd();
    }

    protected virtual void deathEnd()
    {
        Destroy(gameObject);
        //Instantiate(explosionParticle, transform.position, Quaternion.identity);
        CameraMovement.Instance.MoveCamera(5, 5, 1.5f);
    
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        GameEvents.EnemyDefeated(); // Llama al m�todo de NextStage cuando el enemigo sea derrotado
        enemyLevel.EnemigoEliminado();
        enemyCollider.enabled = false;
        // healthBar.HideBar(); 
    }

    //Todo lo de abajo es el manejo de daño y muerte del boss
    private IEnumerator GetDamageBoss(float damage)
    {
        healthBar.ShowBar();
        health -= damage;
        healthBar.UpdateHealthBar(maxHealth, health);

        if (health > 0)
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
            damageCoroutine = StartCoroutine(FlashDamage());

            UpdateHealthBoss(); // Llama a la funci�n para verificar si se debe cambiar el estado
        }
        else
        {
            minionsBoss.KillAllMinions();
            audioManager.ChangeBackgroundMusic(audioManager.gameMusic);
            managerData.level1Finished = true; // Asigna directamente el booleano
            CameraMovement.Instance.MoveCamera(7, 5, 3f);
            //Instantiate(explosionParticle, transform.position, Quaternion.identity);
            animator.SetTrigger("Explode");
            yield return new WaitForSecondsRealtime(1f);
            Destroy(gameObject);
            healthBar.HideBar();
            //Debug.Log("muerto");
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            audioManager.playSound(audioManager.portalSound);
            portal.EnablePortal();
        }

        //Debug.Log("Vida JEFE " + health);
        yield return null;
    }

    private IEnumerator FlashDamage()
    {
        Color customColor;
        ColorUtility.TryParseHtmlString("#FFBD00", out customColor);
        Instantiate(damageParticle, transform.position, Quaternion.identity);
        animator.SetBool("Damage", true);
        float damageDuration = 0.15f;
        spriteRenderer.color = customColor;
        yield return new WaitForSeconds(damageDuration);
        spriteRenderer.color = Color.white;
        animator.SetBool("Damage", false);
    }

    public void UpdateHealthBoss()
    {
        if ((lastHealthThreshold - health) >= 10f)
        {
            bossMachine.StateMachine(); // Cambiar el estado del jefe
            lastHealthThreshold = health; // Actualiza el umbral de salud
            healthBar.UpdateHealthBar(maxHealth, health);
        }
    }
}


