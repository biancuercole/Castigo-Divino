using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UIshop uiShop;
    [SerializeField] private ItemSlot item;
    [SerializeField] private Entrances entrances;
    [SerializeField] private Portals portals;
    public string message;
    public ManagerData managerData;
    public HeartsUI heartsUI;
    private int currentHealth;

    private bool playerInRange = false;
    private bool shopOpen = false;
    private bool itemOpen = false;

    public Vector3 messeagePosition;

    
    private void Start()
    {
        uiShop.Hide();  
        item.Hide();
    }

    private void Update()
    {
        // Detectar cuando el jugador presiona 'F' dentro del rango
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction(); // método que maneja las interacciones
        }
    }

    private void HandleInteraction()
    {
        // Si es una tienda
        if (gameObject.CompareTag("Shop"))
        {
            ToggleShop();
        }
        if (gameObject.CompareTag("GestionMejoras"))
        {
            ToggleItemManagement();
        }
        if (gameObject.CompareTag("altarVida"))
        {
            RecoverHealth();
        }
        if (gameObject.CompareTag("Level1") || gameObject.CompareTag("Level2") || gameObject.CompareTag("Level3"))
        {
            Portals();
        }
    }

    private void ToggleShop()
    {
        if (shopOpen)
        {
            uiShop.Hide();
            Debug.Log("Tienda cerrada");
        }
        else
        {
            uiShop.Show();
            ToolTipManager.instance.HideToolTip();
            Debug.Log("Tienda abierta");
        }
        shopOpen = !shopOpen;
    }

    private void ToggleItemManagement()
    {
        if (itemOpen)
        {
            item.Hide();
            Debug.Log("Gestión de mejoras cerrada");
        }
        else
        {
            item.Show();
            ToolTipManager.instance.HideToolTip();
            Debug.Log("Gestión de mejoras abierta");
        }
        itemOpen = !itemOpen;
    }

    private void RecoverHealth()
    {
        managerData.health = 4;
        currentHealth = managerData.health;
        PlayerPrefs.SetInt("PlayerHealth", managerData.health);
        managerData.LoadPoints();
        heartsUI.UpdateHeartsUI(currentHealth);
    }

    private void Portals()
    {
        if (gameObject.CompareTag("Level1") || gameObject.CompareTag("Level2"))
        {
            entrances.ChangeScene();
        }
        else
        {
            portals.ChangeScenesPortal();
        }
        Debug.Log("se presiono E para cambiar de escena");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
            Vector3 worldPosition = transform.position + messeagePosition;
            ToolTipManager.instance.ShowTriggerToolTip(message, worldPosition);
            Debug.Log("ToolTipShow");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            ToolTipManager.instance.HideToolTip();
            CloseAllInteractions(); 
        }
    }

    private void CloseAllInteractions()
    {
        if (shopOpen)
        {
            uiShop.Hide();
            shopOpen = false;
            Debug.Log("Tienda cerrada al salir");
        }

        if (itemOpen)
        {
            item.Hide();
            itemOpen = false;
            Debug.Log("Gestión de mejoras cerrada al salir");
        }
    }
}
