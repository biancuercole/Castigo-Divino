using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portals : MonoBehaviour
{
    private PlayerMovement player;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        gameObject.SetActive(false);
    }

    public void EnablePortal()
    {
        gameObject.SetActive(true);
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
