using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevel : MonoBehaviour
{
    [SerializeField] private Transform[] points; 
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int totalRounds = 3; 
    //[SerializeField] private Portals portal;
    private AudioManager audioManager;
    public bool startMinions = false;
    
    private bool hasStartedSpawning = false; // Control para evitar múltiples inicios
    private int currentRound = 0;
    public int defeatedEnemies = 0;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        // Iniciar la generación de enemigos solo si startMinions es true y no ha comenzado ya
        if (startMinions && !hasStartedSpawning)
        {
            hasStartedSpawning = true; // Evitar que se vuelva a llamar varias veces
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (currentRound < totalRounds)
        {
            defeatedEnemies = 0; // Reiniciar el contador de enemigos derrotados al comenzar una nueva ronda

            // Esperar antes de que empiece la siguiente ronda
            yield return new WaitForSeconds(2f); // Puede ajustar este tiempo según prefieras

            // Generar enemigos en los puntos correspondientes
            for (int i = 0; i < points.Length; i++)
            {
                int numEnemy = Random.Range(0, enemies.Length);
                Instantiate(enemies[numEnemy], points[i].position, Quaternion.identity);
            }

            // Esperar hasta que todos los enemigos de la ronda sean derrotados
            while (defeatedEnemies < points.Length)
            {
                yield return null; // Espera en cada frame hasta que los enemigos sean derrotados
            }

            currentRound++;
            Debug.Log("Ronda " + currentRound + " completada");
        }

        // Cuando todas las rondas terminan, invocamos el evento de rondas completadas
        GameEvents.AllRoundsCompleted();

        // Cambiar la música o activar el portal cuando todas las rondas terminen
        audioManager.ChangeBackgroundMusic(audioManager.gameMusic);
        yield return new WaitForSecondsRealtime(2f);
        audioManager.playSound(audioManager.portalSound);

        // Destruir el objeto que controla la generación de enemigos al final
        Destroy(gameObject);
    }

    public void EnemyDefeated()
    {
        defeatedEnemies++;
        Debug.Log("Enemigos derrotados " + defeatedEnemies);
    }
}
