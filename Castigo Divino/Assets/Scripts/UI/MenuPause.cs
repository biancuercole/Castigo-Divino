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
    [SerializeField] BoxCollider2D playerBC;

    private bool inmortality = false;
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
        if (Input.GetKeyDown(KeyCode.Escape))
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
        gm.lastCheckpoint = Vector2.zero;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        Debug.Log("reset");
        managerData.ResetGameData();
        SceneManager.LoadScene("PacificZone");
        gm.lastCheckpoint = Vector2.zero;
    }

    public void Inmortality()
    {
        inmortality = !inmortality; 

        if (inmortality)
        {
            playerBC.isTrigger = true;  
            Debug.Log("Inmortalidad activada");
        }
        else
        {
            playerBC.isTrigger = false; 
            Debug.Log("Inmortalidad desactivada");
        }
    }
}
