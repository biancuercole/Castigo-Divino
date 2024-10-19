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
    private bool shopOpen = false;
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
        if (shopOpen && Input.GetKeyDown(KeyCode.E))
        {
            CloseShop();  
            return;  
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();  
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
        if (dialogue.IsDialogueFinished())  
        {
            if (shopOpen)
            {
                CloseShop(); 
            }
            else
            {
                OpenShop();  
            }
        }
        else if (!dialogue.IsTyping) 
        {
            dialogue.nextDialogueLine();
        }
    }

    private void OpenShop()
    {
        dialogue.didDialogueStart = true; 
        uiShop.Show();  
        targetObject.SetActive(false);  
        shopOpen = true;
    }
    private void CloseShop()
    {
        uiShop.Hide();
        dialogue.didDialogueStart = false;
        shopOpen = false;
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
        if (gameObject.CompareTag("Level1") || gameObject.CompareTag("Level2"))
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