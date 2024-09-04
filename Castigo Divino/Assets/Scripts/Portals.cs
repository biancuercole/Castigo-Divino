using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portals : MonoBehaviour
{
    private PlayerMovement player;
    [SerializeField] private int level;
    private TransicionEscena transition;
    
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    private Collider2D portalCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        portalCollider = GetComponent<Collider2D>();

        spriteRenderer.sprite = closedSprite;
        portalCollider.enabled = false;

        transition = FindObjectOfType<TransicionEscena>();
        player = FindObjectOfType<PlayerMovement>();

        // Obtén el collider del portal
        portalCollider = GetComponent<Collider2D>();

        /* Desactiva el portal (incluye el collider y el renderer)
        gameObject.SetActive(false);*/
        Debug.Log("Portal inactivo al inicio");
    }

    public void EnablePortal()
    {
        /* Activa el portal (incluye el collider y el renderer)
        gameObject.SetActive(true);*/

        // Activa el collider y cambia la capa

        portalCollider.enabled = true;
        spriteRenderer.sprite = openSprite;
        gameObject.layer = 3; // Cambia "3" por la capa que desees
        Debug.Log("Portal activado y capa cambiada");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            transition.SiguienteNivel("PacificZone");
            Debug.Log("Pasaron Datos");
        }
    }
}
