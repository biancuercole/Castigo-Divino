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
    public float tiempoEntreOleadas = 5f; // Tiempo de espera entre oleadas

    void Update()
    {
        if (comenzarOleada && enemigosRestantes <= 0 && contadorOleadas < maxOleadas)
        {
            comenzarOleada = false; // Evita que se llame varias veces al Coroutine
            StartCoroutine(GenerarOleada());
        }
    }

    IEnumerator GenerarOleada()
    {
        // Espera antes de comenzar la oleada (solo para las oleadas después de la primera)
        if (contadorOleadas > 0) 
        {
            Debug.Log("Esperando para la siguiente oleada...");
            yield return new WaitForSeconds(tiempoEntreOleadas);
        }

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

        Debug.Log($"Oleada {contadorOleadas} comenzada.");
    }

    public void EnemigoEliminado()
    {
        if (enemigosRestantes > 0)
        {
            enemigosRestantes--;

            if (enemigosRestantes <= 0)
            {
                if (contadorOleadas < maxOleadas)
                {
                    comenzarOleada = true; // Permite que la siguiente oleada comience
                }
                else
                {
                    // Se han completado todas las oleadas
                    Debug.Log("Se completaron todas las oleadas.");
                    GameEvents.AllRoundsCompleted(); // Invoca el evento para abrir la puerta
                }
            }
        }
    }
}
