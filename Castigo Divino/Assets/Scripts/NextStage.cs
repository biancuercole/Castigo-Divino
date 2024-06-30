using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private int enemyCount; // Total de enemigos en la escena
    [SerializeField] private int levelIndex;

    [Header("Sprites")]
    [SerializeField] private Sprite closedSprite; // Sprite cerrada
    [SerializeField] private Sprite openSprite; // sprite abierta
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Inicializar el SpriteRenderer
    }

    // Método para reducir el contador de enemigos
    public void EnemyDefeated()
    {
        enemyCount--;
        Debug.Log("Enemigos restantes: " + enemyCount);
    }

    void Update()
    {
        if(enemyCount == 0)
        {
            spriteRenderer.sprite = openSprite;
        } else
        {
            spriteRenderer.sprite = closedSprite;
        }
    }
    private void OnCollisionEnter2D(Collision2D Player)
    {
        if(enemyCount == 0)
        {
            changeLevel(levelIndex);
        }
    }

    public void changeLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
