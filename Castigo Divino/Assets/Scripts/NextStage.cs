using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private int enemyCount; // Total de enemigos en la escena
    [SerializeField] private int levelIndex;
    [SerializeField] private GameObject[] machines; 
    public int machineCount; 
    private GameObject door;
    private Collider2D doorCollider;

    [Header("Sprites")]
    [SerializeField] private Sprite closedSprite; // Sprite cerrada
    [SerializeField] private Sprite openSprite; // sprite abierta
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        door = GameObject.Find("Puerta1");
        doorCollider = door.GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Inicializar el SpriteRenderer
    }


    // Método para reducir el contador de enemigos
    public void EnemyDefeated()
    {
        enemyCount--;
        Debug.Log("Enemigos restantes: " + enemyCount);
    }

    public void destroyMachine()
    {
        machineCount ++; 
        Debug.Log("Maquina destruida");
    }

    void Update()
    {
        if(machineCount == 1)
        {
            doorCollider.enabled = false;
            spriteRenderer.sprite = openSprite;
        } else
        {
            spriteRenderer.sprite = closedSprite;
        }
    }
    /*private void OnCollisionEnter2D(Collision2D Player)
    {
        if(enemyCount == 0)
        {
            doorCollider.enabled = false;
        }
    }*/

    public void changeLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
