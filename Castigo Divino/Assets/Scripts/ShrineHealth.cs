using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class ShrineHealth : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private HealthBar healthBar;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int indiceNivel;
    private Coroutine damageCoroutine;
    [SerializeField] private ManagerData managerData;

    [SerializeField] private Image redTint;  // Imagen roja para el tinte
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;  // Referencia a la cámara de Cinemachine
    [SerializeField] private Transform waypoint;  // El waypoint al que quieres que la cámara se dirija
    public float showTimer;
    private NextStage nextStage;
    private TransicionEscena transition;
    private AudioManager audioManager;
    void Start()
    {
        animator = GetComponent<Animator>();
        transition = FindObjectOfType<TransicionEscena>();
        nextStage = FindObjectOfType<NextStage>();
        health = maxHealth;
        healthBar.UpdateHealthBar(maxHealth, health);
        spriteRenderer = GetComponent<SpriteRenderer>();
        redTint.gameObject.SetActive(false);  // Asegúrate de que el tinte rojo esté desactivado al inicio
        audioManager = FindObjectOfType<AudioManager>();
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
            // Cuando la salud llegue a cero
            StartCoroutine(DestroyShrineSequence());
        }

        //Debug.Log("Vida altar: " + health);
        yield return null;
    }

    private IEnumerator FlashDamage()
    {
        float damageDuration = 1f;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageDuration);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator DestroyShrineSequence()
    {
        animator.SetTrigger("Destroyed");
        // Mostrar tinte rojo
        redTint.gameObject.SetActive(true);

        // Mover la cámara (esto no funcionará bien con Time.timeScale = 0f a menos que esté configurado para usar tiempos reales)
        CameraMovement.Instance.MoveCamera(5, 5, 2f);

        // Esperar un momento para que el efecto rojo sea visible (sin afectar Time.timeScale)
        yield return new WaitForSecondsRealtime(2.5f); // 3.5 segundos de espera en tiempo real
        audioManager.playSound(audioManager.shrineDestroy);
        // Reiniciar el nivel
        transition.SiguienteNivel("GameScene");
        nextStage.enemiesCount = 0;
        nextStage.keyCount = 0;
        ManagerData.Instance.ResetPoints(); 
        managerData.LoadPoints();
    }
}