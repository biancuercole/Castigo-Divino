using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevel : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array para los distintos prefabs de enemigos
    public Transform[] spawnPoints; 
    public bool comenzarOleada = false; 
    public bool abrirPuerta = false; 
    private int enemigosRestantes; 
    public int contadorOleadas = 0; // Contador de oleadas
    public int maxOleadas = 2; // Número máximo de oleadas (en este caso, 2)

    void Update()
    {
        if (comenzarOleada && enemigosRestantes <= 0 && contadorOleadas < maxOleadas)
        {
            StartCoroutine(GenerarOleada());
        }
    }

    IEnumerator GenerarOleada()
    {
        enemigosRestantes = spawnPoints.Length;
        contadorOleadas++; // Incrementa el contador de oleadas

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // Seleccionar un prefab aleatorio del array de enemyPrefabs
            GameObject enemigoAleatorio = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Instanciar el enemigo en el punto de spawn correspondiente
            Instantiate(enemigoAleatorio, spawnPoints[i].position, spawnPoints[i].rotation);
            yield return new WaitForSeconds(0.2f);
        }

        // Espera 2 segundos antes de permitir que otra oleada comience
        yield return new WaitForSeconds(2f);
    }

    public void EnemigoEliminado()
    {
        if (comenzarOleada && enemigosRestantes > 0)
        {
            enemigosRestantes--;

            if (enemigosRestantes <= 0 && contadorOleadas < maxOleadas)
            {
                // Abre la puerta solo si aún no se ha hecho
                if (!abrirPuerta)
                {
                    abrirPuerta = true;
                    Debug.Log("Puerta abierta");
                }
                comenzarOleada = true; // Permite que la siguiente oleada comience
            }
            else if (enemigosRestantes <= 0 && contadorOleadas >= maxOleadas)
            {
                comenzarOleada = false; // No permitir más oleadas después de la última
                Debug.Log("Se completaron todas las oleadas.");
            }
        }
    }
}
