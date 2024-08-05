using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void UpdateHealthBar(float maxHealth, float health)
    {
        barImage.fillAmount = health / maxHealth;
    }

    public void ShowBar()
    {
        gameObject.SetActive(true);
    }

    public void HideBar()
    {
        gameObject.SetActive(false);
    }
}
