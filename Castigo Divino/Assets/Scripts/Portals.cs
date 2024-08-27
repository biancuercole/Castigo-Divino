using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portals : MonoBehaviour
{
    private PlayerMovement player;
    private Collider2D portalCollider;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();

        // Obtén el collider del portal
        portalCollider = GetComponent<Collider2D>();

        // Desactiva el collider y cambia la capa a 0 para que no se vea
        portalCollider.enabled = false;
        gameObject.layer = 0;
    }

    public void EnablePortal()
    {
        portalCollider.enabled = true;
        gameObject.layer = 3; // Cambia "Default" por la capa que desees
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            SceneManager.LoadScene("PacificZone");
            Debug.Log("Pasaron Datos");
        }
    }
}
