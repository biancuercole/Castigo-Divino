using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    [SerializeField] GameObject menuPause;
    [SerializeField] private ManagerData managerData;
    private bool gamePaused = false;

    private void Start()
    {
     managerData = FindObjectOfType<ManagerData>(); 
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused) 
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Debug.Log("Pausa");
        gamePaused = true;
        Time.timeScale = 0f;
        menuPause.SetActive(true);
    }

    public void Resume()
    {
        Debug.Log("Reanudar");
        gamePaused = false;
        Time.timeScale = 1f;
        menuPause.SetActive(false);
    }

    public void Menu()
    {
        Debug.Log("Salir al menu");
        PlayerPrefs.DeleteAll();
        managerData.ResetPoints();
        managerData.isBulletPowerUpCollected = false;
        managerData.isTripleShotBought = false;
        managerData.speed = 25;
        managerData.speedBullet = 3;
        managerData.damageBullet = 1;
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        managerData.ResetPoints();
        managerData.isBulletPowerUpCollected = false;
        managerData.isTripleShotBought= false;
        managerData.speed = 25;
        managerData.speedBullet= 3;
        managerData.damageBullet = 1;   
        SceneManager.LoadScene("PacificZone");
        Time.timeScale = 1f;
    }
}
