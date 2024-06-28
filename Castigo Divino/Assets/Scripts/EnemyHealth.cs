using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth; 
    private float health;
    private SpriteRenderer spriteRenderer;
    private NextStage nextStage;

    void Start()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextStage = FindObjectOfType<NextStage>(); // Encuentra el objeto con el script NextStage
    }

    public IEnumerator GetDamage(float damage)
    {
        float damageDuration = 0.5f;
        health -= damage;
        if (health > 0)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(damageDuration);
            spriteRenderer.color = Color.white;
        }
        else
        {
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            nextStage.EnemyDefeated(); // Llama al método de NextStage cuando el enemigo sea derrotado
           
            Destroy(gameObject);
        }
    }
}
