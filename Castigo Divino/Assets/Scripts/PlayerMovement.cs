using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 3f;
    //private GameMaster gm;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer GunSpriteRenderer;
    private AudioManager audioManager;
    [SerializeField] private float amountPoints;
    [SerializeField] private PointsUI pointsUI;
    public int healAmount;
    [SerializeField] private Loot loot;
    private NextStage nextStage; 
    [SerializeField] private BossMachine boss;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCoolDown = 0.7f;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private GameObject Gun;

    bool isDashing;
    bool canDash;

    private PlayerHealth playerHealth;

    private void Awake()
    {
        // Inicializa el AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("No se encontró un AudioManager en la escena.");
        }
    }

    void Start()
    {
        // Inicializa el GameMaster
        //gm = GameObject.FindGameObjectWithTag("GM")?.GetComponent<GameMaster>();
        /*f (gm == null)
        {
            Debug.LogError("No se encontró un GameMaster en la escena.");
            return;
        }*/
        //transform.position = gm.lastCheckpoint;
        playerRb = GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogError("No se encontró un componente Rigidbody2D en el jugador.");
        }
        // Inicializa otros componentes
        nextStage = FindObjectOfType<NextStage>();
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GunSpriteRenderer = Gun?.GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
        pointsUI = FindObjectOfType<PointsUI>();
        canDash = true;

        // Verifica si los componentes clave están asignados
        if (playerRb == null || playerAnimator == null || spriteRenderer == null || GunSpriteRenderer == null || trail == null)
        {
            Debug.LogError("Uno o más componentes no están asignados correctamente.");
        }

        if (pointsUI == null)
        {
            Debug.LogError("No se encontró un componente PointsUI en la escena.");
        }

        if (playerHealth == null)
        {
            Debug.LogError("No se encontró un componente PlayerHealth en el jugador.");
        }

        if (boss == null)
        {
            Debug.LogError("No se encontró un componente BossMachine en el jugador.");
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Horizontal", moveX);
            playerAnimator.SetFloat("Vertical", moveY);
            playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
        }

        if (isDashing)
        {
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (GunSpriteRenderer != null && spriteRenderer != null)
            {
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
            }
            trail.emitting = false;
        }
        else
        {
            if (GunSpriteRenderer != null && spriteRenderer != null)
            {
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
            }
            trail.emitting = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
            trail.emitting = true;
        }
    }

    private void FixedUpdate()
    {
        if (playerRb == null)
        {
            Debug.LogError("playerRb no está asignado.");
            return;
        }

        if (isDashing)
        {
            playerRb.velocity = moveInput * dashSpeed;
        }
        else
        {
            playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("dashing");
        canDash = false;
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            Destroy(other.gameObject); // Destruir la moneda
            pointsUI.takePoints(amountPoints);
        }
        if (other.gameObject.CompareTag("heart"))
        {
            Debug.Log("Corazon");
            if (playerHealth != null)
            {
                Debug.Log("Recolectado: " + loot.lootName);
                playerHealth.HealHealth(healAmount);
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("altarVida"))
        {
            Debug.Log("salud recuperada");
            //playerHealth.HealHealth(4);
        }
        if (other.gameObject.CompareTag("key"))
        {
            GameEvents.KeyCollected();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("entrada"))
        {
            GameEvents.ClosedDoor();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("entradaBoss"))
        {
            GameEvents.ClosedDoor();
            Debug.Log("Activando jefe");
            if (boss != null)
            {
                boss.OnActive();
            }
            else
            {
                Debug.LogError("BossMachine no está asignado");
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Level1"))
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
