using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UIshop uiShop;
    [SerializeField] private ItemSlot item;
    [SerializeField] private Entrances entrances;
    [SerializeField] private Portals portals;
    [SerializeField] private Dialogue dialogue;
    public string message;
    public ManagerData managerData;
    public HeartsUI heartsUI;
    private int currentHealth;

    private bool playerInRange = false;
    private bool shopOpen = false;
    private bool itemOpen = false;
    public bool dialogueStartTrigger = false;
    public bool interactions = false;
    public Vector3 messeagePosition;

    private void Start()
    {
        uiShop.Hide();  
        item.Hide();
    }

    private void Update()
    {

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction(); // metodo que maneja las interacciones
        }
    }

    private void HandleInteraction()
    {
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

    public void ToggleShop()
    {
        if (dialogueStartTrigger)
        {
            if (shopOpen)
            {
                uiShop.Hide();
                dialogue.didDialogueStart = false;
                Debug.Log("Tienda cerrada");
            }
            else
            {
                dialogue.didDialogueStart = true;
                uiShop.Show();
                ToolTipManager.instance.HideToolTip();
                Debug.Log("Tienda abierta");
            }
            shopOpen = !shopOpen;
        }
        dialogueStartTrigger = false;
    }

    private void ToggleItemManagement()
    {
        if (itemOpen)
        {
            item.Hide();
            Debug.Log("Gesti�n de mejoras cerrada");
        }
        else
        {
            item.Show();
            ToolTipManager.instance.HideToolTip();
            Debug.Log("Gesti�n de mejoras abierta");
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
            dialogue.didDialogueStart = false;
            Debug.Log("Tienda cerrada al salir");
        }

        if (itemOpen)
        {
            item.Hide();
            itemOpen = false;
            Debug.Log("Gesti�n de mejoras cerrada al salir");
        }

        interactions = false;
    }
}