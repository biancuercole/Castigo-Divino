using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public void Start()
    {
        // Verifica que el array de spawnPoints tenga el tamaño correcto
        if (spawnPoints.Length > 0)
        {
            // Spawnear enemigos en diferentes puntos
            if (spawnPoints.Length > 0) SpawnEnemyAtPosition("Enemy1", spawnPoints[0].position);
            if (spawnPoints.Length > 1) SpawnEnemyAtPosition("Enemy2", spawnPoints[1].position);
        }
        else
        {
            Debug.LogWarning("No spawn points assigned in the inspector.");
        }
    }

    public void SpawnEnemyAtPosition(string enemyType, Vector3 position)
    {
        GameObject enemy = EnemyPool.Instance.GetEnemy(enemyType);
        if (enemy != null)
        {
            enemy.transform.position = position;
            enemy.transform.rotation = Quaternion.identity;
        }
    }
}