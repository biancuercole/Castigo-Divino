using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;

    [SerializeField] public float maxBar = 15;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void UpdateHealthBar(float maxBar, float health)
    {
        barImage.fillAmount = health / maxBar;
    }

    public void UpdateHealth(float health, float maxBar)
    {
        barImage.fillAmount = health / maxBar;
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
