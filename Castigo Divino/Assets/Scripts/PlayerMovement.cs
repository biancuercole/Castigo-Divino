using System;
using System.Collections;
using TMPro;
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
    private NextStage nextStage; 
    [SerializeField] private BossMachine boss;
  
    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCoolDown = 1f;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private GameObject Gun;
    bool isDashing;
    bool canDash;


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
        canDash = true;

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

    // Invertir el sprite cuando el personaje se mueve a la izquierda
    if (moveX < 0)
    {
        transform.localScale = new Vector3(-1, 1, 1); // Voltea el sprite horizontalmente
    }
    else if (moveX > 0)
    {
        transform.localScale = new Vector3(1, 1, 1); // Mantiene el sprite normal
    }

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
    }   else
        {
            GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
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
            trail.emitting = true;
        }
        else
        {
            playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
            trail.emitting = false;
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
