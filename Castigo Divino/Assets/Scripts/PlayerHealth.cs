using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth; 
    private float health;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int indiceNivel;

    void Start()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            passLevel(indiceNivel);
       
        }
    }

    private void passLevel(int indice)
    {
        SceneManager.LoadScene(indice);
    }
}
