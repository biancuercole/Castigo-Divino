using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    public Vector2 lastCheckpoint;

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
        // Inicializa lastCheckpoint con una posición segura (por ejemplo, la posición inicial del jugador)
        if (lastCheckpoint == Vector2.zero)
        {
            lastCheckpoint = new Vector2(525, -170); // Reemplaza (0, 0) por la posición inicial deseada del jugador
        }
    }
}
