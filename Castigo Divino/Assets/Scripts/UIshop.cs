using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIshop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = transform.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void createItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemReactTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 30;
        shopItemReactTransform.anchoredPosition = new Vector2 (0, -shopItemHeight * positionIndex);

        shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().text = itemName;
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().text = itemCost.ToString();

    }
}
