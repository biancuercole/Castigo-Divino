using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MinionsBoss : MonoBehaviour
{
    private float minX, maxX, minY, maxY;
    [SerializeField] private Transform[] points; 
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float timeEnemies;
    [SerializeField] private int maxMinions = 5;
    private float timeNextEnemy; 

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
    }

    public void ActivateMinions()
    {
        int numEnemy = Random.Range(0 , enemies.Length);
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        int currentMinions = GameObject.FindGameObjectsWithTag("Minions").Length; 

        if (timeNextEnemy >= timeEnemies && currentMinions < maxMinions)
        {
            timeNextEnemy = 0;
            Instantiate(enemies[numEnemy], randomPosition, Quaternion.identity);
        }
    }
}
