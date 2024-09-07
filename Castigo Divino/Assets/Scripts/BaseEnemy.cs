using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
    [SerializeField] private Portals portal;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int indiceNivel;
    private Coroutine damageCoroutine;
    public BossMachine bossMachine;

    private PlayerMovement playerMovement;
    private ManagerData managerData;


    private float lastHealthThreshold; // Nueva variable
    protected virtual void Start()
    {
        health = maxHealth;
        audioManager = FindObjectOfType<AudioManager>();
        enemyCollider = GetComponent<Collider2D>();

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

            yield return new WaitForSeconds(0.5f);

        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            HandleDeath();
        }
    }

    protected virtual void HandleDeath()
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        audioManager.playSound(audioManager.enemyDeath);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        GameEvents.EnemyDefeated();
        enemyCollider.enabled = false;


        Destroy(gameObject);
        // healthBar.HideBar(); 
    }

    //Manejo de da�o del boss
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
            audioManager.playSound(audioManager.openDoor);
            managerData.level1Finished = true; // Asigna directamente el booleano
            CameraMovement.Instance.MoveCamera(7, 5, 3f);
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
            healthBar.HideBar();
            portal.EnablePortal();
            Debug.Log("muerto");
            GetComponent<LootBag>().InstantiateLoot(transform.position);
        }

        Debug.Log("Vida JEFE " + health);
        yield return null;
    }

    private IEnumerator FlashDamage()
    {
        Color customColor;
        ColorUtility.TryParseHtmlString("#FFBD00", out customColor);
        Instantiate(damageParticle, transform.position, Quaternion.identity);
        float damageDuration = 0.15f;
        spriteRenderer.color = customColor;
        yield return new WaitForSeconds(damageDuration);
        spriteRenderer.color = Color.white;
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


