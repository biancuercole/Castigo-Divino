using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private GameObject Gun;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer GunSpriteRenderer;
    private Vector2 moveInput;
    private Rigidbody2D playerRb;

    // rotacion 
    private Vector3 targetRotation;
    public Transform player;
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GunSpriteRenderer = Gun?.GetComponent<SpriteRenderer>();
        playerRb = GetComponent<Rigidbody2D>();

        if (playerRb == null || playerAnimator == null || spriteRenderer == null || GunSpriteRenderer == null)
        {
            //Debug.LogError("Uno o más componentes no están asignados correctamente.");
        }

    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // Actualizar animaciones de movimiento
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);  // Si hay movimiento o no
        }


        // Rotación y cambio de dirección del sprite basado en la posición del mouse
        targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
     //   var angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;

        if (Mathf.Abs(targetRotation.x) > Mathf.Abs(targetRotation.y))
        {
            // El mouse está más hacia la izquierda o derecha
            if (targetRotation.x < 0)
            {
                // El mouse está a la izquierda del jugador
                playerAnimator.SetFloat("Horizontal", -1);  // Animación de mirar a la izquierda
                playerAnimator.SetFloat("Vertical", 0);  // Reseteamos el valor de Vertical
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
            }
            else
            {
                // El mouse está a la derecha del jugador    
                playerAnimator.SetFloat("Horizontal", 1);   // Animación de mirar a la derecha
                playerAnimator.SetFloat("Vertical", 0);  // Reseteamos el valor de Vertical
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
            }
        }
        else
        {
            // El mouse está más hacia arriba o abajo
            if (targetRotation.y < 0)
            {
                // El mouse está por debajo del jugador
                playerAnimator.SetFloat("Vertical", -1);  // Animación de mirar hacia abajo
                playerAnimator.SetFloat("Horizontal", 0);  // Reseteamos el valor de Horizontal
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
            }
            else
            {
                // El mouse está por encima del jugador    
                playerAnimator.SetFloat("Vertical", 1);   // Animación de mirar hacia arriba
                playerAnimator.SetFloat("Horizontal", 0);  // Reseteamos el valor de Horizontal
                GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
            }
        }

        if (moveInput.sqrMagnitude == 0)
        {
            // Si el jugador NO se está moviendo (idle)
            GunSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
        }
    }
    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }
}