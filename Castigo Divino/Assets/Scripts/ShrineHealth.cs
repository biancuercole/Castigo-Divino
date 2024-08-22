using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShrineHealth : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private HealthBar healthBar;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int indiceNivel;

    private Coroutine damageCoroutine;
    public float showTimer;
    private NextStage nextStage;

    void Start()
    {
        nextStage = FindObjectOfType<NextStage>();
        health = maxHealth;
        healthBar.UpdateHealthBar(maxHealth, health);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        showTimer -= Time.deltaTime;
        if (showTimer <= 0)
        {
            healthBar.HideBar();
        }
    }

    public IEnumerator GetDamage(float damage)
    {
        showTimer = 5;
        health -= damage;
        healthBar.UpdateHealthBar(maxHealth, health);
        if (health > 0)
        {
            healthBar.ShowBar();
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
            damageCoroutine = StartCoroutine(FlashDamage());
        }
        else
        {
           // healthBar.HideBar();
            SceneManager.LoadScene(indiceNivel);
            nextStage.enemiesCount = 0;
            nextStage.keyCount = 0;
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
