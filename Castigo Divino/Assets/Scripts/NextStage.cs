using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    public enum MachineState { Open, Close }
    public MachineState currentState;

    public Transform player;

    public int enemiesCount;
    public int keyCount;

    [SerializeField] public int enemiesNeeded;
    [SerializeField] public int keysNeeded;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    private Collider2D doorCollider;

    [SerializeField] private string puertaTag; 

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
        enemiesCount = 0;
        keyCount = 0;
        currentState = MachineState.Close;

        GameEvents.OnEnemyDefeated += EnemyDefeated;
        GameEvents.OnKeyCollected += CollectKey;
        GameEvents.OnClosedDoor += closeDoor;
        GameEvents.OnAllRoundsCompleted += OpenDoor; // Nos suscribimos al nuevo evento

        StartCoroutine(StateMachine());
    }

    private void OnDestroy()
    {
        GameEvents.OnEnemyDefeated -= EnemyDefeated;
        GameEvents.OnKeyCollected -= CollectKey;
        GameEvents.OnClosedDoor -= closeDoor;
        GameEvents.OnAllRoundsCompleted -= OpenDoor; // Nos desuscribimos del evento
    }

    private void OpenDoor()
    {
        currentState = MachineState.Open;
    }

    public void EnemyDefeated()
    {
        enemiesCount++;
       // Debug.Log("Enemigos derrotados: " + enemiesCount);
        CheckState();
    }

    public void CollectKey()
    {
        keyCount++;
      //  Debug.Log("Llaves colectadas: " + keyCount);
        CheckState();
    }

    public void closeDoor()
    {
        enemiesCount = 0;
        keyCount = 0;
        currentState = MachineState.Close; 
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
        if (enemiesCount >= enemiesNeeded && keyCount >= keysNeeded)
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
        doorCollider.enabled = true;
        spriteRenderer.sprite = closedSprite;
    }

    public void Open()
    {
        spriteRenderer.sprite = null;
        doorCollider.enabled = false;
    }
}

public static class GameEvents
{
    public static System.Action OnEnemyDefeated;
    public static System.Action OnKeyCollected;
    public static System.Action OnClosedDoor;
    public static System.Action OnAllRoundsCompleted; // Nuevo evento

    public static void EnemyDefeated()
    {
        OnEnemyDefeated?.Invoke();
    }

    public static void KeyCollected()
    {
        OnKeyCollected?.Invoke();
    }

    public static void ClosedDoor()
    {
        OnClosedDoor?.Invoke();
    }

    public static void AllRoundsCompleted() // Nueva función para invocar el evento
    {
        OnAllRoundsCompleted?.Invoke();
    }
}

