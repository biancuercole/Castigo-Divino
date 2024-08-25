using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int indiceNivel;
    [SerializeField] private float inmunidadDuracion = 2.0f; // Duración de la inmunidad en segundos
    [SerializeField] private float parpadeoIntervalo = 0.2f; // Intervalo de parpadeo

    public int maxHealth;
    public int health;
    private bool esInmune = false;

    public UnityEvent<int> changeHealth;

    private SpriteRenderer spriteRenderer;

    private KnocKBack KnocKBack;

    public ManagerData managerData;

    public GameObject damageParticle;

    private NextStage nextStage;

    private HeartsUI heartsUI;
    void Start()
    {
        nextStage = FindObjectOfType<NextStage>();
       // health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        KnocKBack = GetComponent<KnocKBack>();
        heartsUI = GetComponent<HeartsUI>();
        managerData.LoadPoints();
       // maxHealth = managerData.LoadMaxHealth();
        health = managerData.health;
        if (health <= 0)
        {
            health = maxHealth;
        }

        // Inicializar la UI de corazones con el maxHealth actual
       /* heartsUI.InitializeHeartsUI(maxHealth);

        // Actualizar la UI para reflejar la vida actual del jugador
        heartsUI.UpdateHeartsUI(health);*/
        changeHealth.Invoke(health);
    }

    public void GetDamage(int damage, GameObject damageSource)
    {
        if (!esInmune)
        {
            CameraMovement.Instance.MoveCamera(5, 5, 2f);
            Instantiate(damageParticle, transform.position, Quaternion.identity);
            KnocKBack.KnockBacK(damageSource);

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
            managerData.AddHealth(health);

            if (temporaryHealth <= 0)
            {
                passLevel(indiceNivel);
                managerData.ResetPoints();
                nextStage.enemiesCount = 0;
                nextStage.keyCount = 0;
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
        managerData.AddHealth(health);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        health = maxHealth; 
        changeHealth.Invoke(maxHealth);
        managerData.AddHealth(health);
        //managerData.AddMaxHealth(maxHealth);
        heartsUI.UpdateHeartsUI(maxHealth);
    }

    private void passLevel(int indice)
    {
        managerData.AddHealth(health);
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
