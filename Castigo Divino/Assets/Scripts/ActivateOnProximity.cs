using UnityEngine;

public class ActivateOnProximity : MonoBehaviour
{
    public Transform player; 
    public float activationDistance = 10f; // Distancia m�xima a la que se activar�n los enemigos

    void OnDrawGizmosSelected()
    {
       
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, activationDistance);
        }
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("No se ha asignado el jugador al script ActivateOnProximity.");
            return;
        }

        // Iterar a trav�s de todos los hijos de Enemies
        foreach (Transform child in transform)
        {
            // Calcular la distancia entre el jugador y cada hijo de Enemies
            float distanceToPlayer = Vector3.Distance(child.position, player.position);

            // Activar o desactivar cada hijo de Enemies basado en la distancia al jugador
            child.gameObject.SetActive(distanceToPlayer <= activationDistance);
        }
    }
}
