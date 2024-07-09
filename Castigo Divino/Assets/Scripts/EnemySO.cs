using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewEnemy", menuName ="ScriptableObjects/Enemy")] 
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float health;
    public float speed;
    public Sprite enemySprite;
    public GameObject enemyPrefab;
}
