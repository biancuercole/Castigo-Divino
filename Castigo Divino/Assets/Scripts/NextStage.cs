using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    public enum MachineState { Open, Close }
    public MachineState currentState;

    public Transform player;

    private int enemiesCount;
    private int keyCount;

    private int enemiesNeeded;
    private int keysNeeded;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    private Collider2D doorCollider;

    [SerializeField] private string puertaTag; // Nueva variable para especificar el tag de la puerta en el Inspector

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        enemiesCount = 0;
        keyCount = 0;
        currentState = MachineState.Close;

        // Asignar las variables según el tag del GameObject
        if (puertaTag == "Puerta1")
        {
            enemiesNeeded = 1;
            keysNeeded = 1;
        }
        else if (puertaTag == "Puerta2")
        {
            enemiesNeeded = 2;
            keysNeeded = 1;
        }
        else if (puertaTag == "Puerta3")
        {
            enemiesNeeded = 3;
            keysNeeded = 1;
        }

        // Suscribirse a eventos estáticos
        GameEvents.OnEnemyDefeated += EnemyDefeated;
        GameEvents.OnKeyCollected += CollectKey;

        StartCoroutine(StateMachine());
    }

    private void OnDestroy()
    {
        // Desuscribirse de eventos estáticos
        GameEvents.OnEnemyDefeated -= EnemyDefeated;
        GameEvents.OnKeyCollected -= CollectKey;
    }

    public void EnemyDefeated()
    {
        enemiesCount++;
        Debug.Log("Enemigos derrotados: " + enemiesCount);
        CheckState();
    }

    public void CollectKey()
    {
        keyCount++;
        Debug.Log("Llaves colectadas: " + keyCount);
        CheckState();
    }

    private void Update()
    {
        switch (currentState)
        {
            case MachineState.Close:
                Close();
                break;
            case MachineState.Open:
                Open();
                break;
        }
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            yield return null; // Espera un frame
        }
    }

    private void CheckState()
    {
        if (enemiesCount == enemiesNeeded && keyCount == keysNeeded)
        {
            currentState = MachineState.Open;
        }
        else
        {
            currentState = MachineState.Close;
        }
    }

    private void Close()
    {
        doorCollider.enabled = false;
        spriteRenderer.sprite = closedSprite;
    }

    private void Open()
    {
        doorCollider.enabled = true;
        spriteRenderer.sprite = openSprite;
    }
}

public static class GameEvents
{
    public static System.Action OnEnemyDefeated;
    public static System.Action OnKeyCollected;

    public static void EnemyDefeated()
    {
        OnEnemyDefeated?.Invoke();
    }

    public static void KeyCollected()
    {
        OnKeyCollected?.Invoke();
    }
}
