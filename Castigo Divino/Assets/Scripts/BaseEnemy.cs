using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseEnemy : MonoBehaviour, IDamageType
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject damageParticle;
    [SerializeField] private GameObject explosionParticle;
    private float health;
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
    private int enemyIndex;
    private Manual manual;

    private float lastHealthThreshold;

    protected virtual void Start()
    {
        manual = FindObjectOfType<Manual>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        audioManager = FindObjectOfType<AudioManager>();
        enemyCollider = GetComponent<Collider2D>();
        enemyLevel = FindObjectOfType<EnemyLevel>();
        managerData = FindObjectOfType<ManagerData>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        bossMachine = GetComponent<BossMachine>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastHealthThreshold = maxHealth;
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
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation()
    {
        animator.SetTrigger("Explode");
        yield return new WaitForSecondsRealtime(0.7f);
        DeathEnd();
    }

    protected virtual void DeathEnd()
    {
        string tag = gameObject.tag;
        switch (tag)
        {
            case "fuego":
                enemyIndex = 0;
                break;
            case "tronco":
                enemyIndex = 1;
                break;
            case "Humo":
                enemyIndex = 2;
                break;
            default:
                enemyIndex = -1;
                break;
        }

        if (manual != null && enemyIndex != -1)
        {
            manual.ShowDefeatedEnemies(enemyIndex);
        }

        Destroy(gameObject);
        CameraMovement.Instance.MoveCamera(5, 5, 1.5f);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        GameEvents.EnemyDefeated();
        enemyLevel.EnemigoEliminado();
        enemyCollider.enabled = false;
    }

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

            UpdateHealthBoss();
        }
        else
        {
            minionsBoss.KillAllMinions();
            audioManager.ChangeBackgroundMusic(audioManager.gameMusic);
            managerData.level1Finished = true;
            CameraMovement.Instance.MoveCamera(7, 5, 3f);
            animator.SetTrigger("Explode");
            yield return new WaitForSecondsRealtime(1f);
            Destroy(gameObject);
            healthBar.HideBar();
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            audioManager.playSound(audioManager.portalSound);
            portal.EnablePortal();
        }

        yield return null;
    }

    private IEnumerator FlashDamage()
    {
        Color customColor;
        ColorUtility.TryParseHtmlString("#FFBD00", out customColor);
        Instantiate(damageParticle, transform.position, Quaternion.identity);
        animator.SetBool("Damage", true);
        spriteRenderer.color = customColor;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.white;
        animator.SetBool("Damage", false);
    }

    public void UpdateHealthBoss()
    {
        if ((lastHealthThreshold - health) >= 10f)
        {
            bossMachine.StateMachine();
            lastHealthThreshold = health;
            healthBar.UpdateHealthBar(maxHealth, health);
        }
    }
}
