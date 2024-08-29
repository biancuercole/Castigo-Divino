using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyLevel : MonoBehaviour
{
    private float minX, maxX, minY, maxY;
    [SerializeField] private Transform[] points; 
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float timeEnemies;
    private float timeNextEnemy; 
    [SerializeField] private Portals portal;
    [SerializeField] private int enemyNeeded; 
    private int enemyCount = 0; 

    private void Start()
    {
        maxX = points.Max(point => point.position.x);
        maxY = points.Max(point => point.position.y);
        minX = points.Min(point => point.position.x);
        minY = points.Min(point => point.position.y);
    }

    private void Update()
    {
        timeNextEnemy += Time.deltaTime;

        if (timeNextEnemy >= timeEnemies)
        {
            ActivateMinions();
        }
    }

    public void ActivateMinions()
    {
        int numEnemy = Random.Range(0, enemies.Length);
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        timeNextEnemy = 0;
        Instantiate(enemies[numEnemy], randomPosition, Quaternion.identity);
    }

    public void EnemyDefeated()
    {
        enemyCount ++; 
        if (enemyCount >= enemyNeeded)
        {
            portal.EnablePortal();
            Destroy(gameObject); 
        }
    }
}
