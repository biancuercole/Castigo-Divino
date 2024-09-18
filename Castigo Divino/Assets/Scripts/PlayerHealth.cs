using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int indiceNivel;
    [SerializeField] private float inmunidadDuracion = 2.0f; // Duración de la inmunidad en segundos
    [SerializeField] private float parpadeoIntervalo = 0.2f; // Intervalo de parpadeo

    [SerializeField] private UnityEngine.UI.Image redTint;
    public int maxHealth;
    public int health;
    public bool esInmune = false;

    public UnityEvent<int> changeHealth;
    private SpriteRenderer spriteRenderer;
    private KnocKBack KnocKBack;
    public ManagerData managerData;
    public GameObject damageParticle;
    private NextStage nextStage;
    private HeartsUI heartsUI;
    private TransicionEscena transition;
    private Animator animator; 
    [SerializeField] private GameObject Gun;
    private SpriteRenderer[] gunSprite;

    void Start()
    {
        gunSprite = Gun.GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        transition = FindObjectOfType<TransicionEscena>();
        redTint.gameObject.SetActive(false);
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

            if(temporaryHealth > 0)
            {
             StartCoroutine(DañoAnimation());
            }

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
                Gun.SetActive(false);
                animator.SetTrigger("Muerte");
                CameraMovement.Instance.MoveCamera(5, 5, 2f);
                redTint.gameObject.SetActive(true);
                StartCoroutine(HandleLevelTransition());
            }
            else
            {
                StartCoroutine(InmunidadCoroutine());
            }
        }
    }

    private IEnumerator DañoAnimation()
    {
        animator.SetBool("isDamaged", true);

        // Esperar hasta que la animación de "Daño" haya terminado.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetBool("isDamaged", false);
    }

    private IEnumerator HandleLevelTransition()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        // Mueve la cámara durante 1 segundo antes de pausar
        CameraMovement.Instance.MoveCamera(5, 5, 1.5f);
        
        // Espera en tiempo real para permitir que la vibración ocurra
        yield return new WaitForSecondsRealtime(0.5f);

        // Reinicia el nivel y carga las monedas del checkpoint
        if(sceneName == "GameScene")
        {
            transition.SiguienteNivel("GameScene"); 
        } else if (sceneName == "EnemyLevel")
        {
            transition.SiguienteNivel("EnemyLevel");
        }
        ManagerData.Instance.ResetPoints(); // Reinicia las monedas a 0
        ManagerData.Instance.ResetCurrentPower();
        nextStage.enemiesCount = 0;
        nextStage.keyCount = 0;
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
            // Alternar el sprite del jugador
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // Alternar los sprites del arma
            foreach (SpriteRenderer gunPart in gunSprite)
            {
                gunPart.enabled = !gunPart.enabled;
            }

            // Esperar antes de alternar nuevamente
            yield return new WaitForSeconds(parpadeoIntervalo);
        }

        // Asegurarse de que todos los sprites estén visibles después de la inmunidad
        spriteRenderer.enabled = true;
        foreach (SpriteRenderer gunPart in gunSprite)
        {
            gunPart.enabled = true;
        }

        esInmune = false;
    }
}
