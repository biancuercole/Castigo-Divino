using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIshop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    public PointsUI pointsUI; // Referencia al CoinManager
    public PowerUpEffect[] powerUps;
  

    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton("Speed + 5", Item.GetCost(Item.ItemType.Speed), 0, powerUps[0]);
        CreateItemButton("BDamage + 5", Item.GetCost(Item.ItemType.BulletDamage), 1, powerUps[1]);
        CreateItemButton("Heart + 1", Item.GetCost(Item.ItemType.Heart), 2, powerUps[2]);
        CreateItemButton("BSpeed + 5", Item.GetCost(Item.ItemType.BulletSpeed), 3, powerUps[3]);
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
        button.onClick.AddListener(() => TryBuyItem(itemCost, powerUp, button));


    }

    private void TryBuyItem(int itemCost, PowerUpEffect powerUp, Button button)
    {
        if (pointsUI.SpendPoints(itemCost))
        {
            Debug.Log("Purchased: " + powerUp.name);
            // Assuming the player object has a tag "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            powerUp.Apply(player);
            button.interactable = false;
        }
        else
        {
            Debug.Log("Not enough coins");
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
