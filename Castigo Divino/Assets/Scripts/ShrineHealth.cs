
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ShrineHealth : MonoBehaviour
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
        Debug.Log("Recibiendo da�o: " + damage);
        float damageDuration = 0.1f;
        health -= damage;
        if (health > 0)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(damageDuration);
            spriteRenderer.color = Color.white;
        }
        else
        {
            Debug.Log("Santuario destruido.");
            SceneManager.LoadScene(indiceNivel);
            
        }

        Debug.Log("Vida altar: " + health);
    }
}