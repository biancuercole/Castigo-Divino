using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private Portals portal;
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private HealthBar healthBar;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int indiceNivel;
    private Coroutine damageCoroutine;
    public BossMachine bossMachine;
    public GameObject damageParticle;
    public GameObject explosionParticle;
    void Start()
    {
        bossMachine = GetComponent<BossMachine>();
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

            // Cambia el estado si la salud ha disminuido al menos 2.5 puntos desde la última vez
            if ((maxHealth - health) >= 2.5f)
            {
                bossMachine.StateMachine(); // Esta línea debe ser solo una llamada a método, sin asignación
                health = health + 1; 
                maxHealth = health; // Actualiza el máximo temporal
            }
        }
        else
        {
            CameraMovement.Instance.MoveCamera(5, 5, 1f);
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
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
        Color customColor;
        ColorUtility.TryParseHtmlString("#FFBD00", out customColor);
        Instantiate(damageParticle, transform.position, Quaternion.identity);
        float damageDuration = 0.15f;
        spriteRenderer.color = customColor;
        yield return new WaitForSeconds(damageDuration);
        spriteRenderer.color = Color.white;
    }
}
