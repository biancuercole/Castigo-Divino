using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    [SerializeField] private int enemyCount; // Total de enemigos en la escena

    private void Start()
    {
        // Inicializar enemyCount con el número total de enemigos en la escena al inicio
        enemyCount = FindObjectsOfType<EnemyHealth>().Length;
    }

    // Método para reducir el contador de enemigos
    public void EnemyDefeated()
    {
        enemyCount--;
        Debug.Log("Enemigos restantes: " + enemyCount);

        if (enemyCount <= 0)
        {
            Debug.Log("Todos los enemigos han sido derrotados. ¡Siguiente etapa!");
            Destroy(gameObject);
            // Aquí puedes añadir la lógica para pasar a la siguiente etapa
        }
    }
}
