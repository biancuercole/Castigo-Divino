using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 3f;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Vector3 originalScale;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCoolDown = 0.7f;

    bool isDashing;
    bool canDash;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        canDash = true;
    }

    // Update is called once per frame
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

        if (moveX != 0)
        {
            // Flip the character by modifying the X scale
            transform.localScale = new Vector3(originalScale.x * (moveX > 0 ? -1 : 1), originalScale.y, originalScale.z);
        }

        Input.GetMouseButtonDown(0);

        if (Input.GetKeyDown(KeyCode.Space)&& canDash)
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
        //playerRb.velocity = new Vector2 (moveInput.x * dashSpeed, moveInput.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}
