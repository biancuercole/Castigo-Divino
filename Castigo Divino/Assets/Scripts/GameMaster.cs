using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public Vector2 lastCheckpoint;
    public int checkpointCoins; // Añadido para guardar las monedas del checkpoint

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (lastCheckpoint == Vector2.zero)
        {
            lastCheckpoint = new Vector2(2400, -1125); // Reemplaza (0, 0) por la posición inicial deseada del jugador
        }
       // checkpointCoins = 0; // Inicializa con 0 monedas por defecto
    }

    public void SetCheckpoint(Vector2 position, int coins)
    {
        lastCheckpoint = position;
       // checkpointCoins = coins;
       //Debug.Log("Checkpoint actualizado: " + lastCheckpoint + ", Monedas: " + checkpointCoins);
    }
}
