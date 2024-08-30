using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyLevel : MonoBehaviour
{
    [SerializeField] private Transform[] points; 
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int totalRounds = 3; 
    [SerializeField] private Portals portal;

    private int currentRound = 0;
    private int defeatedEnemies = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (currentRound < totalRounds)
        {
            yield return new WaitForSeconds(2f);

            for (int i = 0; i < points.Length; i++)
            {
                int numEnemy = Random.Range(0, enemies.Length);
                Instantiate(enemies[numEnemy], points[i].position, Quaternion.identity);
            }

            while (defeatedEnemies < points.Length * (currentRound + 1))
            {
                yield return null;
            }

            currentRound++;
        }
        portal.EnablePortal();
        Destroy(gameObject);
    }

    public void EnemyDefeated()
    {
        defeatedEnemies++;
    }
}
