using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton("Velocidad + 2", Item.GetCost(Item.ItemType.Speed), 0, powerUps[0]);
        CreateItemButton("Daño + 1", Item.GetCost(Item.ItemType.BulletDamage), 1, powerUps[1]);
        CreateItemButton("Velocidad Bala + 2", Item.GetCost(Item.ItemType.BulletSpeed), 2, powerUps[3]);
        // CreateItemButton("Triple", Item.GetCost(Item.ItemType.TripleShot), 2, powerUps[2]);
    }

    private void CreateItemButton(string itemName, int itemCost, int positionIndex, PowerUpEffect powerUp)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemReactTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 100;
        shopItemReactTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        shopItemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("ItemCost").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        Button button = shopItemTransform.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => TryBuyItem(itemCost, powerUp, button, shopItemTransform));
    }

    private void TryBuyItem(int itemCost, PowerUpEffect powerUp, Button button, Transform shopItemTransform)
    {
        if (pointsUI.SpendPoints(itemCost))
        {
            if (powerUp != null)
            {
                // Verifica si el PowerUp es para el arma o para el jugador
                if (System.Array.Exists(weaponUpgradeItems, item => item.GetComponent<PowerUp>().powerUpEffect == powerUp))
                {
                    EnableWeaponUpgradeItem(powerUp);
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
                        Debug.LogError("Player not found");
                    }
                }

                button.interactable = false;
                UpdateButtonAppearance(shopItemTransform);
            }
            else
            {
                Debug.LogError("PowerUpEffect is null");
            }
        }
        else
        {
            ToolTipManager.instance.SetAndShowToolTip(messeage);
            Debug.Log("Not enough coins");
        }
    }

    private void EnableWeaponUpgradeItem(PowerUpEffect powerUp)
    {
        foreach (GameObject item in weaponUpgradeItems)
        {
            if (item.GetComponent<PowerUp>().powerUpEffect == powerUp)
            {
                item.SetActive(true);
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

    public void Show()
    {
        container.gameObject.SetActive(true);
        rotation.canShoot = false;
    }

    public void Hide()
    {
       
        container.gameObject.SetActive(false);
        rotation.canShoot = true;
    }

}
