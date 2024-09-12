using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    private GameMaster gm;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer GunSpriteRenderer;
    private AudioManager audioManager;
    private NextStage nextStage; 
    [SerializeField] private BossMachine boss;
    private Portals portals; 
    private TransicionEscena transicion; 
  
    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCoolDown = 1f;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject Gun;
    bool isDashing;
    bool canDash;
    [SerializeField] private bool dashEnabled = true;

    [Header("Special Attack")]
    public bool canSpecialAttack;
    private PowerOfGod powerOfGod;
    [SerializeField] private float attackRadius = 40f; 
    [SerializeField] private int attackDamage = 50; 


    public ManagerData managerData;

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
        transicion = FindObjectOfType<TransicionEscena>();
        powerOfGod = FindObjectOfType<PowerOfGod>();
        portals = FindObjectOfType<Portals>();
        managerData = ManagerData.Instance;
        if (managerData == null)
        {
            Debug.LogError("No se encontró ManagerData en la escena.");
            return;
        }

        // Carga la velocidad desde ManagerData, que debería haber cargado desde PlayerPrefs
        speed = managerData.speed;

        string sceneName = SceneManager.GetActiveScene().name;
        gm = GameObject.FindGameObjectWithTag("GM")?.GetComponent<GameMaster>();
        if (gm != null && gm.lastCheckpoint != Vector2.zero)
        {
            transform.position = gm.lastCheckpoint;
        } else
        {
            transform.position = new Vector2(525, -170); 
            Debug.Log("gm nulo");
        }

        if (sceneName == "PacificZone" || sceneName == "EnemyLevel")
        {
            transform.position = new Vector2(525, -170); 
            Debug.Log("ZONA PACIFICATION");
        }

        playerRb = GetComponent<Rigidbody2D>();

        nextStage = FindObjectOfType<NextStage>();
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GunSpriteRenderer = Gun?.GetComponent<SpriteRenderer>();
        canDash = true;
        canSpecialAttack = false;
        // Verifica si los componentes clave están asignados
        if (playerRb == null || playerAnimator == null || spriteRenderer == null || GunSpriteRenderer == null)
        {
            Debug.LogError("Uno o más componentes no están asignados correctamente.");
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
           // Debug.Log("Arriba");
            if (GunSpriteRenderer != null && spriteRenderer != null)
            {
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
            }
        } 
            else
            {
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                if (dashEnabled)
                {
                StartCoroutine(Dash());
                }
                else
                {
                 Debug.Log("No hay dash");
                }
            }

           if (Input.GetKey(KeyCode.F) && canSpecialAttack)
           {
               ActivateSpecialAttack();
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
            Instantiate(dashEffect, transform.position, Quaternion.identity);
        }
        else
        {
            playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
        }  
    }

    private IEnumerator Dash()
    {
      //  Debug.Log("dashing");
        canDash = false;
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    private void ActivateSpecialAttack()
    {
        CameraMovement.Instance.MoveCamera(10, 8, 2f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Aplicar daño a cada enemigo dentro del área
            enemy.GetComponent<BaseEnemy>()?.TakeDamage(10, BulletType.Air);
        }
 
        Debug.Log("Ataque especial");
  
        canSpecialAttack = false;
        ManagerData.Instance.ResetCurrentPower();
        powerOfGod.currentPower = 0;
        powerOfGod.UpdatePowerUpBar(0);
    }

    private void OnDrawGizmos()
    {
       
        if (canSpecialAttack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("entrada"))
        {
            GameEvents.ClosedDoor();
        }
        if(other.gameObject.CompareTag("entrada2"))
        {
            GameEvents.ClosedDoor();
        }
        if (other.gameObject.CompareTag("entradaBoss"))
        {
            Debug.Log("Activando jefe");
            if (boss != null)
            {
                boss.OnActive();
                audioManager.ChangeBackgroundMusic(audioManager.bossMusic);
            }
            else
            {
                Debug.LogError("BossMachine no está asignado");
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Retorno"))
        {
            transicion.SiguienteNivel("PacificZone");
            gm.lastCheckpoint = Vector2.zero;
        }
    }
}
