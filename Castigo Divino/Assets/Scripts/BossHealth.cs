using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*public class BossHealth : MonoBehaviour
{
    [SerializeField] private Portals portal;
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private HealthBar healthBar;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int indiceNivel;
    private Coroutine damageCoroutine;
    public BossMachine bossMachine;
    public GameObject damageParticle;
    public GameObject explosionParticle;
    private PlayerMovement playerMovement; 
    private ManagerData managerData;
    private AudioManager audioManager;

    private float lastHealthThreshold; // Nueva variable

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        managerData = FindObjectOfType<ManagerData>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        bossMachine = GetComponent<BossMachine>();
        health = maxHealth;
        healthBar.UpdateHealthBar(maxHealth, health);
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastHealthThreshold = maxHealth; // Inicializar con la salud máxima
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(GetDamage(damage));
    }

    public IEnumerator GetDamage(float damage)
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

            UpdateHealthBoss(); // Llama a la función para verificar si se debe cambiar el estado
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
            GetComponent<LootBag>().InstantiateLoot(transform.position);
        }

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
    }*/
    
    public class BossHealth : BaseEnemy
    {

        public override void TakeDamage(float damage, BulletType bulletType)
        {
            switch (bulletType)
            {
            case BulletType.Fire:
                damage *= 1f;
                break;
            case BulletType.Water:
                damage *= 1f;
                break;
            case BulletType.Air:
                damage *= 1f;
                break;
            case BulletType.Earth:
                damage *= 1f;
                break;
            case BulletType.GodPower:
                damage *= 1f;
                break;
        }

             base.TakeDamageBoss(damage, bulletType);
           /// StartCoroutine(GetDamage(damage));

        }
    }

