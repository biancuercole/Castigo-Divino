using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private int maxUpgrades = 3; // L�mite de mejoras que se pueden aplicar
    public List<GameObject> appliedUpgrades = new List<GameObject>();
    private PowerUpManager powerUpManager;
    public Canvas canvas;
    public Rotation rotation;
    private void Awake()
    {
        canvas.gameObject.SetActive(false);
        LoadItemColors();
    }
    private void Start()
    {
        powerUpManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpManager>();
    }

    public void OnItemClick(GameObject item)
    {
        if (appliedUpgrades.Contains(item))
        {
            // Si la mejora ya est� aplicada, la removemos
            RemovePowerUpEffect(item);
            appliedUpgrades.Remove(item);
            DeselectItem(item);
        }
        else
        {
            // Si no est� aplicada y a�n no alcanzamos el l�mite, la aplicamos
            if (appliedUpgrades.Count < maxUpgrades)
            {
                ApplyPowerUpEffect(item);
                appliedUpgrades.Add(item);
                SelectItem(item);
            }
        }
    }

    private void ApplyPowerUpEffect(GameObject item)
    {
        PowerUp powerUp = item.GetComponent<PowerUp>();
        if (powerUp != null)
        {
            powerUpManager.ApplyPowerUp(powerUp.powerUpEffect);
        }
    }

    private void RemovePowerUpEffect(GameObject item)
    {
        PowerUp powerUp = item.GetComponent<PowerUp>();
        if (powerUp != null)
        {
            powerUpManager.RemovePowerUp(powerUp.powerUpEffect);
        }
    }

    public void SelectItem(GameObject item)
    {
        Color selectedColor = Color.green;
        item.GetComponent<Image>().color = selectedColor;

        // Guarda el color en PlayerPrefs usando el nombre del item como clave
        string itemName = item.name;
        PlayerPrefs.SetString(itemName + "_Color", ColorUtility.ToHtmlStringRGBA(selectedColor));
        PlayerPrefs.Save();
        //.Log(itemName + "_Color " + selectedColor);
    }

    private void DeselectItem(GameObject item)
    {
        item.GetComponent<Image>().color = Color.white;

        // Elimina el color guardado en PlayerPrefs
        string itemName = item.name;
        PlayerPrefs.DeleteKey(itemName + "_Color");
        PlayerPrefs.Save();
        //Debug.Log(itemName + "_Color " + item.GetComponent<Image>().color);
    }

    private void LoadItemColors()
    {
        foreach (Transform child in transform)
        {
            GameObject item = child.gameObject;
            string itemName = item.name;
            string savedColor = PlayerPrefs.GetString(itemName + "_Color", null);

            if (!string.IsNullOrEmpty(savedColor))
            {
                Color loadedColor;
                if (ColorUtility.TryParseHtmlString("#" + savedColor, out loadedColor))
                {
                    item.GetComponent<Image>().color = loadedColor;
                }
            }
        }

    }

    public void Show()
    {
        canvas.gameObject.SetActive(true);
        rotation.canShoot = false;
    }
    public void Hide()
    {
        canvas.gameObject.SetActive(false);
        rotation.canShoot = true;
    }
}



