using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 3f;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void FixedUpdate()
    {
        //fisicas
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }
}
