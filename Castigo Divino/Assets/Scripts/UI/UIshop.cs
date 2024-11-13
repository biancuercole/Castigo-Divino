using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class UIshop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    public PointsUI pointsUI; 
    public PowerUpEffect[] powerUps;
    public GameObject[] weaponUpgradeItems;
    public string messeage;
    public Rotation rotation;
   private bool isBought; 

    private ManagerData managerData;
    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
        managerData = ManagerData.Instance;
    }

    private void Start()
    {
        CreateItemButton(Item.GetSprite(Item.ItemType.Speed),"Velocidad", Item.GetCost(Item.ItemType.Speed), 0, powerUps[0]);
        CreateItemButton(Item.GetSprite(Item.ItemType.BulletDamage),"Dano", Item.GetCost(Item.ItemType.BulletDamage), 1, powerUps[1]);
        CreateItemButton(Item.GetSprite(Item.ItemType.BulletSpeed), "Velocidad Bala", Item.GetCost(Item.ItemType.BulletSpeed), 2, powerUps[3]);
        // CreateItemButton("Triple", Item.GetCost(Item.ItemType.TripleShot), 2, powerUps[2]);
    }

    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex, PowerUpEffect powerUp)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemReactTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemWidth = 350;  
        shopItemReactTransform.anchoredPosition = new Vector2(shopItemWidth * positionIndex, 0);  

        shopItemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("ItemCost").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;

        Button button = shopItemTransform.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => TryBuyItem(itemCost, powerUp, button, shopItemTransform, itemName));

        if (managerData.IsItemBought(itemName))
        {
            button.interactable = false;
            UpdateButtonAppearance(shopItemTransform);
        }
    }

    private void TryBuyItem(int itemCost, PowerUpEffect powerUp, Button button, Transform shopItemTransform, string itemName)
    {
        if (pointsUI.SpendPoints(itemCost))
        {
            if (powerUp != null)
            {
                // Verifica si el PowerUp es para el arma o para el jugador
                if (System.Array.Exists(weaponUpgradeItems, item => item.GetComponent<PowerUp>().powerUpEffect == powerUp))
                {
                    EnableWeaponUpgradeItem(powerUp, itemName);
                }
                else
                {
                    // Aplica la mejora autom�ticamente si no es para el arma
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        powerUp.Apply(player);
                    }

                    else
                    {
                        //Debug.LogError("Player not found");
                    }
                }

                managerData.MarkItemAsBought(shopItemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().text);
                button.interactable = false;
                UpdateButtonAppearance(shopItemTransform);
            }
            else
            {
                //Debug.LogError("PowerUpEffect is null");
            }
        }
        else
        {
            ToolTipManager.instance.SetAndShowToolTip(messeage);
            //Debug.Log("Not enough coins");
        }
    }
    
    private void EnableWeaponUpgradeItem(PowerUpEffect powerUp, string itemName)
    {
        foreach (GameObject item in weaponUpgradeItems)
        {
            if (item.GetComponent<PowerUp>().powerUpEffect == powerUp)
            {
                item.SetActive(true);
                managerData.IsBought(itemName);
                //Debug.Log("Item en UIshop " + itemName);
            }
        }
    }

    private void UpdateButtonAppearance(Transform shopItemTransform)
    {
        foreach (Transform child in shopItemTransform)
        {
            Image childImage = child.GetComponent<Image>();
            if (childImage != null)
            {
                childImage.color = Color.grey;
            }
        }
    }

    public void ResetShop()
    {
        foreach (Transform shopItemTransform in container)
        {
            if (shopItemTransform == null) continue; // Verificar si el objeto aún existe

            Button button = shopItemTransform.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = true;
            }

            // Restaurar la apariencia original del botón
            foreach (Transform child in shopItemTransform)
            {
                if (child == null) continue; // Verificar si el objeto aún existe

                Image childImage = child.GetComponent<Image>();
                if (childImage != null)
                {
                    childImage.color = Color.white; // O el color original
                }
            }
        }

        // Reiniciar el estado de isBought
        isBought = false;
    }

    public void Show()
    {
    //    Debug.Log("Tienda abriendose");
        container.gameObject.SetActive(true);
        rotation.canShoot = false;
    }

    public void Hide()
    {
       // Debug.Log("Tienda cerrandose");
        container.gameObject.SetActive(false);
        rotation.canShoot = true;
    }

}
