using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
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

    private float lastHealthThreshold; // Nueva variable

    void Start()
    {
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
            CameraMovement.Instance.MoveCamera(7, 5, 3f);
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
            healthBar.HideBar();
            portal.EnablePortal();
            Debug.Log("muerto");
            GetComponent<LootBag>().InstantiateLoot(transform.position);
        }

        Debug.Log("Vida altar: " + health);
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
