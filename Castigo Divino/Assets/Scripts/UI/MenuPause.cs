using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject MusicAndSoundMenu;
    [SerializeField] private ManagerData managerData;
    private bool gamePaused = false;
    private GameMaster gm;
    [SerializeField] PlayerHealth player;
    [SerializeField] Rotation rotation;

    private bool inmortality = false;
    public Toggle toggle;
    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        gamePaused = false;
        managerData = FindObjectOfType<ManagerData>();
        if (managerData == null)
        {
            //Debug.LogError("No se encontr� una instancia de ManagerData en la escena.");
        }

        toggle.isOn = false;

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
        //Debug.Log("Pausa");
        gamePaused = true;
        Time.timeScale = 0f;
        menuPause.SetActive(true);
        rotation.canShoot = false;
    }

    public void Resume()
    {
        //Debug.Log("Reanudar");
        gamePaused = false;
        Time.timeScale = 1f;
        menuPause.SetActive(false);
        MusicAndSoundMenu.SetActive(false);
        rotation.canShoot = true;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        //Debug.Log("Salir al menu");
        managerData.ResetGameData();
        SceneManager.LoadScene("Menu");
        gm.lastCheckpoint = Vector2.zero;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        //Debug.Log("reset");
        managerData.ResetGameData();
        SceneManager.LoadScene("PacificZone");
        gm.lastCheckpoint = Vector2.zero;
    }

    public void Inmortality()
    {
        inmortality = !inmortality; 

        if (inmortality)
        {
          
            player.esInmune = true;
            toggle.isOn = true;
            //Debug.Log("Inmortalidad activada");
        }
        else
        {
            player.esInmune = false;
            toggle.isOn = false;
            //Debug.Log("Inmortalidad desactivada");
        }
    }

    public void MusicAndSound()
    {
        MusicAndSoundMenu.SetActive(true);
    }

    public void BackToMenu()
    {
        MusicAndSoundMenu.SetActive(false);
    }
}
