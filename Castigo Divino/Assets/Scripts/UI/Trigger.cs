using UnityEditor.Rendering;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UIshop uiShop;
    [SerializeField] private ItemSlot item;
    [SerializeField] private Entrances entrances;
    [SerializeField] private Portals portals;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] public GameObject targetObject;
    public string message;
    public ManagerData managerData;
    public PlayerHealth playerHealth;
    public HeartsUI heartsUI;
    private int currentHealth;

    private bool playerInRange = false;
    public bool shopOpen = false;
    private bool itemOpen = false;
    public bool dialogueStartTrigger = false;
    public bool interactions = false;

    private void Start()
    {
        uiShop.Hide();
        item.Hide();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {/*
            Debug.Log("Jugador presionó 'E'. Verificando tipo de objeto...");
            if (gameObject.CompareTag("Shop"))
            {
                if (!shopOpen && dialogue.IsDialogueFinished())
                {
                    ToggleShop();
                }
                else if (shopOpen)
                {
                    CloseShop();
                }
            }
            else
            {*/
                HandleInteraction();
            /* }*/
            if (shopOpen && Input.GetKeyDown(KeyCode.E))
            {
                dialogue.lineIndex++;
            }
        }
    }

    private void HandleInteraction()
    {
      /*  if (gameObject.CompareTag("Shop"))
        {
            ToggleShop();
        }*/
        if (gameObject.CompareTag("GestionMejoras"))
        {
            ToggleItemManagement();
        }
        if (gameObject.CompareTag("altarVida"))
        {
            RecoverHealth();
        }
        if (gameObject.CompareTag("Level1") || gameObject.CompareTag("Level2") || gameObject.CompareTag("Level3") || gameObject.CompareTag("Level1Right"))
        {
            Portals();
        }
        if(gameObject.CompareTag("instruccion"))
        {
            targetObject.SetActive(true);  
        }
    }

    public void ToggleShop()
    {
        Debug.Log("Intentando cambiar estado de la tienda...");
        if (shopOpen)
        {
            CloseShop();
        }
        else if (!shopOpen && dialogue.IsDialogueFinished())
        {
            OpenShop();
        }
        else
        {
            Debug.Log("Diálogo no ha terminado, no se puede abrir la tienda.");
        }
    }
    private void OpenShop()
    {
        Debug.Log("Tienda abierta");
        dialogue.didDialogueStart = true;  // Inicia el diálogo
        uiShop.Show();  // Muestra la tienda
        targetObject.SetActive(false);  // Oculta el objeto del jugador
        shopOpen = true;  // Marca la tienda como abierta
    }


    public void CloseShop()
    {
        uiShop.Hide();  // Oculta la tienda
        dialogue.didDialogueStart = false;  // Finaliza el diálogo
        shopOpen = false;  // Marca la tienda como cerrada
        dialogue.lineIndex = dialogue.dialogueLines.Length;
        Debug.Log("Tienda cerrada");
    }

    private void ToggleItemManagement()
    {
        if (itemOpen)
        {
            item.Hide();
            //Debug.Log("Gesti�n de mejoras cerrada");
        }
        else
        {
            item.Show();
            targetObject.SetActive(false);
            //Debug.Log("Gesti�n de mejoras abierta");
        }
        itemOpen = !itemOpen;
    }

    private void RecoverHealth()
    {
        playerHealth.health = 4;
        Debug.Log("vida" + playerHealth.health);
        managerData.health = 4;
        currentHealth = managerData.health;
        PlayerPrefs.SetInt("PlayerHealth", managerData.health);
        managerData.LoadPoints();
        heartsUI.UpdateHeartsUI(currentHealth);
    }

    private void Portals()
    {
        if (gameObject.CompareTag("Level1") || gameObject.CompareTag("Level2") || gameObject.CompareTag("Level1Right"))
        {
            entrances.ChangeScene();
        }
        else
        {
            portals.ChangeScenesPortal();
        }
        //Debug.Log("se presiono E para cambiar de escena");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
            targetObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
            targetObject.SetActive(false);
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
            //Debug.Log("Tienda cerrada al salir");
        }

        if (itemOpen)
        {
            item.Hide();
            itemOpen = false;
            //Debug.Log("Gesti�n de mejoras cerrada al salir");
        }

        interactions = false;
    }
}