using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private Portals portal;
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
