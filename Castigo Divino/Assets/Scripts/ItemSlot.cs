using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private int maxUpgrades = 3; // Límite de mejoras que se pueden aplicar
    private List<GameObject> appliedUpgrades = new List<GameObject>();
    private PowerUpManager powerUpManager;
    public Canvas canvas;
    public Rotation rotation;
    private void Awake()
    {
        canvas.gameObject.SetActive(false);
    }
    private void Start()
    {
        powerUpManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpManager>();
    }

    public void OnItemClick(GameObject item)
    {
        if (appliedUpgrades.Contains(item))
        {
            // Si la mejora ya está aplicada, la removemos
            RemovePowerUpEffect(item);
            appliedUpgrades.Remove(item);
            DeselectItem(item);
        }
        else
        {
            // Si no está aplicada y aún no alcanzamos el límite, la aplicamos
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

    private void SelectItem(GameObject item)
    {
        // Cambia visualmente el item para mostrar que está seleccionado
        item.GetComponent<Image>().color = Color.green; // Cambia el color o usa otra indicación visual
    }

    private void DeselectItem(GameObject item)
    {
        // Restablece el estado visual del item
        item.GetComponent<Image>().color = Color.white; // Color original o lo que prefieras
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



