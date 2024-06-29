using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 3f;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCoolDown = 0.7f;

    [Header("Sprites")]
    [SerializeField] private Sprite defaultSprite; // Sprite por defecto
    [SerializeField] private Sprite backSprite; // Sprite "sin brazos atras"
    [SerializeField] private Sprite sidesSprite;
    [SerializeField] private Sprite sidesSprites;

    bool isDashing;
    bool canDash;

    public bool IsFacingRight { get; private set; } = true; // Property to check facing direction

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Inicializar el SpriteRenderer
        canDash = true;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        //inputs
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // Cambiar sprite si se presiona la tecla "arriba" o "W"
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            spriteRenderer.sprite = backSprite;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            spriteRenderer.sprite = sidesSprite;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            spriteRenderer.sprite = sidesSprites;
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
        }

        // Detectar posición del ratón relativa a la posición del jugador
        /*Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            IsFacingRight = false;
        }
        else
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            IsFacingRight = true;
        }*/

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
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
}
