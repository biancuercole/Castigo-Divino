using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 3f;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer GunSpriteRenderer;
    [SerializeField] private float amountPoints;
    [SerializeField] private PointsUI pointsUI;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCoolDown = 0.7f;

    [SerializeField] private GameObject Gun;

    bool isDashing;
    bool canDash;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GunSpriteRenderer = Gun.GetComponent<SpriteRenderer>();
        canDash = true;
        pointsUI = FindObjectOfType<PointsUI>();
        if (pointsUI == null)
        {
            Debug.LogError("No se encontr� un componente PointsUI en la escena.");
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        playerAnimator.SetFloat("Horizontal", moveX);
        playerAnimator.SetFloat("Vertical", moveY);
        playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);

        if (isDashing)
        {
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        }
        else
        {
            GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
        }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("coin"))
        {
            Destroy(other.gameObject); // Destruir la moneda
            pointsUI.takePoints(amountPoints);
        }
    }

}
