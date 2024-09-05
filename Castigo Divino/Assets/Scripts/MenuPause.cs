using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    [SerializeField] GameObject menuPause;
    [SerializeField] private ManagerData managerData;
    private bool gamePaused = false;
    private GameMaster gm;

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        gamePaused = false;
        managerData = FindObjectOfType<ManagerData>();
        if (managerData == null)
        {
            Debug.LogError("No se encontr� una instancia de ManagerData en la escena.");
        }
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
        Time.timeScale = 1f;
        Debug.Log("Salir al menu");
        managerData.ResetGameData();
        SceneManager.LoadScene("Menu");
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        Debug.Log("reset");
        managerData.ResetGameData();
        SceneManager.LoadScene("PacificZone");
        gm.lastCheckpoint = Vector2.zero;
    }
}
