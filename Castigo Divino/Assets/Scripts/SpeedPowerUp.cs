using System.Collections;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    private bool spedUp;
    private PlayerMovement playerMovement;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Asegúrate de que es el jugador
        {
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                Debug.Log("Colisión con el jugador detectada");
                spedUp = true; // Marcar como verdadero para iniciar la corrutina
                StartCoroutine(IncreaseSpeedTemporarily()); // Iniciar la corrutina directamente
                // No destruyas el gameObject aquí, deja que la corrutina se encargue después de esperar
            }
        }
    }

    private IEnumerator IncreaseSpeedTemporarily()
    {
        Debug.Log("Corrutina iniciada");
        float originalSpeed = playerMovement.speed; // Guardar la velocidad original
        playerMovement.speed *= 1.1f; // Incrementar la velocidad en un 10%

        yield return new WaitForSeconds(5f); // Esperar 5 segundos

        playerMovement.speed = originalSpeed; // Restaurar la velocidad original
        Debug.Log("Velocidad restaurada a: " + originalSpeed); // Mensaje de depuración

        // Destruir el powerup después de restaurar la velocidad
        Destroy(gameObject);
    }
}
