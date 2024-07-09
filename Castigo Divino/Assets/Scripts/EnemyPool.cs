using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    public GameObject[] enemyPrefabs;
    public int poolSize = 10;

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var prefab in enemyPrefabs)
        {
            // Usar el nombre del prefab como clave
            string prefabName = prefab.name;

            // Verificar si la clave ya existe en el diccionario
            if (poolDictionary.ContainsKey(prefabName))
            {
                Debug.LogWarning($"Prefab {prefabName} already exists in the pool dictionary. Skipping.");
                continue;
            }

            Queue<GameObject> enemyPool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Instantiate(prefab);
                enemy.SetActive(false);
                enemyPool.Enqueue(enemy);
            }

            // Agregar al diccionario usando el nombre del prefab como clave
            poolDictionary.Add(prefabName, enemyPool);
        }
    }


    public GameObject GetEnemy(string enemyType)
    {
        if (!poolDictionary.ContainsKey(enemyType))
        {
            Debug.LogWarning("Enemy type not found in pool dictionary");
            return null;
        }

        if (poolDictionary[enemyType].Count == 0)
        {
            Debug.LogWarning("No enemies left in the pool, consider increasing the pool size");
            return null;
        }

        GameObject enemy = poolDictionary[enemyType].Dequeue();
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        poolDictionary[enemy.name].Enqueue(enemy);
    }
}

