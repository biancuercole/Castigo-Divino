using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShrineHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float health;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int indiceNivel;

    private Coroutine damageCoroutine;

    void Start()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator GetDamage(float damage)
    {
        health -= damage;
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
            Debug.Log("Santuario destruido.");
            SceneManager.LoadScene(indiceNivel);
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
