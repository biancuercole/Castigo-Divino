using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private GameMaster gm;

    [SerializeField] private int checkpointCoins = 0; // Monedas para este checkpoint

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gm.SetCheckpoint(transform.position, checkpointCoins);
            Debug.Log("Checkpoint actualizado: " + gm.lastCheckpoint + ", Monedas: " + gm.checkpointCoins);
        }
    }
}
