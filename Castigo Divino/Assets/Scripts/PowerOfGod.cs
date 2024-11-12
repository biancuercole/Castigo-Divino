using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerOfGod : MonoBehaviour
{

    [SerializeField] private Image barImage;
    [SerializeField] public float maxBar = 40;
    public float currentPower = 1f;
    private PlayerMovement playerMovement;

    public float flickerTime = 0.2f;
    private bool isFlickering = false;

    [SerializeField] private GameObject powerUpText;
    private void Start()
    {
        barImage.color = Color.gray;
        playerMovement = FindObjectOfType<PlayerMovement>();
        // Cargar los puntos desde ManagerData
        ManagerData.Instance.LoadPoints();

        // Asignar el valor de currentPower desde ManagerData
        currentPower = ManagerData.Instance.currentPower;

        // Actualizar la barra de poder con el valor actual
        UpdatePowerUpBar(currentPower);

        powerUpText.SetActive(false);
    }

    private void Update()
    {
        if (currentPower >= maxBar && !isFlickering)
        {
            playerMovement.canSpecialAttack = true;
            StartCoroutine(Flicker());
        }
    }

    public void TakePower(float Power)
    {
        ManagerData.Instance.AddCurrentPower(Power);
        currentPower += Power;
        UpdatePowerUpBar(Power);
        //Debug.Log("currentPower = " + currentPower);
    }
    public void UpdatePowerUpBar(float Power)
    {
        
        barImage.color = Color.white;
   
        currentPower = Mathf.Clamp(currentPower, 0, maxBar);
        barImage.fillAmount = currentPower / maxBar;
        //Debug.Log("Bar fill amount = " + barImage.fillAmount);
       /* if (currentPower >= maxBar)
        {
            playerMovement.canSpecialAttack = true;
        }*/
    }

    private IEnumerator Flicker()
    {
        isFlickering = true;

        powerUpText.SetActive(true);


        while (currentPower >= maxBar)
        {
            barImage.enabled = !barImage.enabled;
            powerUpText.SetActive(!powerUpText.activeSelf);
            yield return new WaitForSeconds(flickerTime);
        }

        barImage.enabled = true;
        isFlickering = false;
        powerUpText.SetActive(false);
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