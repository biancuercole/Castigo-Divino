using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private Portals portal;
   /* [SerializeField] private float maxHealth = 10f; 
    private float health;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false; // Flag para controlar si el enemigo está muerto

    private Collider2D bossCollider; // Referencia al Collider

    void Start()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossCollider = GetComponent<Collider2D>(); // Obtén el componente Collider
    }*/

   /* public void TakeDamage(float damage)
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
            if (portal != null)
            {
            portal.EnablePortal();
            Debug.Log("muerto");
            isDead = true; // Marcar al enemigo como muerto para evitar que se procese varias veces
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            // Esperar a que la animación de explosión termine
            yield return new WaitForSeconds(1.0f); // Ajusta el tiempo según la duración de la animación
            } else
            {
                Debug.Log("depuracion");
            }
        }
    }*/

    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private HealthBar healthBar;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int indiceNivel;
  private Coroutine damageCoroutine;

    void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(maxHealth, health);
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        }
        else
        {
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
        float damageDuration = 1f;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageDuration);
        spriteRenderer.color = Color.white;
    }
}
