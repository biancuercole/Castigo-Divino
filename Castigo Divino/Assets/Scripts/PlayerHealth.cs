 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int indiceNivel;

    public int maxHealth;
    private int health;

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


    private void passLevel(int indice)
    {
        SceneManager.LoadScene(indice);
    }
}
