using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; 
    private float health;
    private SpriteRenderer spriteRenderer;
    private NextStage nextStage;

    void Start()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextStage = FindObjectOfType<NextStage>(); // Encuentra el objeto con el script NextStage
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
            Debug.Log($"Waiting for {damageDuration} seconds.");
            yield return new WaitForSeconds(damageDuration);
            spriteRenderer.color = Color.white;
            Debug.Log("Changing color back to white.");
        }
        else
        {
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            nextStage.EnemyDefeated(); // Llama al método de NextStage cuando el enemigo sea derrotado
            Destroy(gameObject);
        }
    }
}
