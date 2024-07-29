using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int indiceNivel;
    [SerializeField] private float inmunidadDuracion = 2.0f; // Duración de la inmunidad en segundos
    [SerializeField] private float parpadeoIntervalo = 0.2f; // Intervalo de parpadeo

    public int maxHealth;
    private int health;
    private bool esInmune = false;

    public UnityEvent<int> changeHealth;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        health = maxHealth;
        changeHealth.Invoke(health);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void GetDamage(int damage)
    {
        if (!esInmune)
        {
            int temporaryHealth = health - damage;

            if (temporaryHealth < 0)
            {
                health = 0;
            }
            else
            {
                health = temporaryHealth;
            }

            changeHealth.Invoke(health);

            if (temporaryHealth <= 0)
            {
                passLevel(indiceNivel);
            }
            else
            {
                StartCoroutine(InmunidadCoroutine());
            }
        }
    }

    public void HealHealth(int healAmount)
    {
        int temporaryHealth = health + healAmount;
        if (temporaryHealth > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health = temporaryHealth;
        }

        changeHealth.Invoke(health);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        health = maxHealth; 
        changeHealth.Invoke(maxHealth);
    }

    private void passLevel(int indice)
    {
        SceneManager.LoadScene(indice);
    }

    private IEnumerator InmunidadCoroutine()
    {
        esInmune = true;
        float tiempoFin = Time.time + inmunidadDuracion;

        while (Time.time < tiempoFin)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(parpadeoIntervalo);
        }

        spriteRenderer.enabled = true;
        esInmune = false;
    }
}
