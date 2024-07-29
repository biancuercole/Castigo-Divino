using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIshop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    public PointsUI pointsUI; 
    public PowerUpEffect[] powerUps;
    public GameObject[] weaponUpgradeItems;
    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton("Velocidad + 2", Item.GetCost(Item.ItemType.Speed), 0, powerUps[0]);
        CreateItemButton("Daño + 2", Item.GetCost(Item.ItemType.BulletDamage), 1, powerUps[1]);
        CreateItemButton("Corazón + 1", Item.GetCost(Item.ItemType.Heart), 2, powerUps[2]);
        CreateItemButton("Velocidad Bala + 3", Item.GetCost(Item.ItemType.BulletSpeed), 3, powerUps[3]);
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
            if (powerUp is PowerUpEffect)
            {
                EnableWeaponUpgradeItem(powerUp);
                button.interactable = false;
            }
            else
            {
                // Aplica la mejora automáticamente si no es para el arma
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                powerUp.Apply(player);
                button.interactable = false;
            }


            foreach (Transform child in shopItemTransform)
            {
                Image childImage = child.GetComponent<Image>();
                if (childImage != null)
                {
                    childImage.color = Color.grey;
                }
            }

        }
        else
        {
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

    public void Show()
    {
        container.gameObject.SetActive(true);
    }

    public void Hide()
    {
       
        container.gameObject.SetActive(false);

    }

}
