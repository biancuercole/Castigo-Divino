using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerOfGod : MonoBehaviour
{

    [SerializeField] private Image barImage;
    [SerializeField] public float maxBar = 40;
    public float currentPower = 0f;
    private PlayerMovement playerMovement; 
    private void Start()
    {
        barImage.color = Color.gray;
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
    public void UpdatePowerUpBar(float Power)
    {
        barImage.color = Color.white;
        currentPower += Power;
        currentPower = Mathf.Clamp(currentPower, 0, maxBar);

        barImage.fillAmount = currentPower / maxBar;
        Debug.Log("currentPower = " + currentPower);
        Debug.Log("Bar fill amount = " + barImage.fillAmount);
        if (currentPower >= maxBar)
        {
            playerMovement.canSpecialAttack = true;
        }
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