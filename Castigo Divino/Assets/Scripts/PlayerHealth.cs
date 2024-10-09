using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Nueva variable para controlar si el jugador ya ha muerto
    private bool yaMuerto = false;

    [SerializeField] private int indiceNivel;
    [SerializeField] private float inmunidadDuracion = 2.0f;
    [SerializeField] private float parpadeoIntervalo = 0.2f;

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
    private AudioManager audioManager;
    public UnityEvent OnBegin, OnDone;
    void Start()
    {
        gunSprite = Gun.GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        transition = FindObjectOfType<TransicionEscena>();
        redTint.gameObject.SetActive(false);
        nextStage = FindObjectOfType<NextStage>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        KnocKBack = GetComponent<KnocKBack>();
        heartsUI = GetComponent<HeartsUI>();
        managerData.LoadPoints();
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        health = managerData.health;
        if (health <= 0)
        {
            health = maxHealth;
        }
        changeHealth.Invoke(health);
    }

    public void GetDamage(int damage, GameObject damageSource)
    {
        audioManager.playSound(audioManager.damage);
        Debug.Log("Inmunidad "+ esInmune);
        if (!esInmune  && !yaMuerto)
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

            if (temporaryHealth <= 0 && !yaMuerto)
            {
                yaMuerto = true;  // Marcar que el jugador ha muerto
                Gun.SetActive(false);
                animator.SetTrigger("Muerte");
                OnBegin?.Invoke();
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
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("isDamaged", false);
    }

    private IEnumerator HandleLevelTransition()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        CameraMovement.Instance.MoveCamera(5, 5, 1.5f);
        StartCoroutine(audioManager.FadeOut(audioManager.musicSource, 0.7f));
        yield return new WaitForSecondsRealtime(0.5f);

        if(sceneName == "GameScene")
        {   
            transition.SiguienteNivel("GameScene"); 
        } 
        else if (sceneName == "EnemyLevel")
        {
            transition.SiguienteNivel("EnemyLevel");
        }

        ManagerData.Instance.ResetPoints();
        ManagerData.Instance.ResetCurrentPower();
        nextStage.enemiesCount = 0;
        nextStage.keyCount = 0;
        yield return new WaitForSecondsRealtime(1.5f);
        OnDone?.Invoke();
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

    public IEnumerator InmunidadCoroutine()
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
